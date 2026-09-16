using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeGameManager : MonoBehaviour
{
    public static SnakeGameManager Instance;

    [Header("Scene Setup")]
    public GameObject snakeBgSprite;
    public Camera mainCamera;

    [Header("Ball Prefabs")]
    public GameObject[] allBallPrefabs;

    [Header("Snake")]
    public SnakeController2D snakeController;

    [Header("Timer")]
    public float timeRemaining = 120f;
    public bool timerRunning = false;
    public TextMeshProUGUI timerText;

    [Header("Lives")]
    public int lives = 2;
    public GameObject[] heartObjects;

    [Header("UI")]
    public TextMeshProUGUI fragmentsCollectedText;
    public TextMeshProUGUI moleculeNameText;
    public TextMeshProUGUI scoreText;

    [Header("Required Order UI")]
    public GameObject[] requiredOrderSlots;
    public Image[] requiredOrderImages;
    public TextMeshProUGUI[] requiredOrderTexts;

    [Header("Win Panel")]
    public GameObject winPanel;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winFactText;
    public Button nextButton;
    public TextMeshProUGUI nextButtonText;
    public GameObject inputBlocker;

    [Header("Lose Panel")]
    public GameObject losePanel;
    public TextMeshProUGUI loseScoreText;
    public TextMeshProUGUI loseFactText;
    public Button retryButton;

    [Header("Congratulations")]
    public GameObject congratsPanel;
    public Image congratsImage;
    public Sprite congratsSprite;

    [Header("Game State")]
    public bool isGameOver = false;

    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

    private int currentOrderIndex = 0;
    private List<Ball.FragmentType> requiredOrder = new List<Ball.FragmentType>();
    private int currentScore = 0;
    private List<GameObject> spawnedBalls = new List<GameObject>();
    private bool isLastLevel = false;

    private Dictionary<Ball.FragmentType, string> fragmentNames =
        new Dictionary<Ball.FragmentType, string>()
    {
        { Ball.FragmentType.CH3,  "CH₃"  },
        { Ball.FragmentType.CH2,  "CH₂"  },
        { Ball.FragmentType.CH,   "CH"   },
        { Ball.FragmentType.C,    "C"    },
        { Ball.FragmentType.OH,   "OH"   },
        { Ball.FragmentType.COOH, "COOH" },
        { Ball.FragmentType.NH2,  "NH₂"  },
    };

    private string[] isomerFacts = new string[]
    {
        "Isomers have the same molecular formula but different structural formulas!",
        "Chain isomers differ in the arrangement of the carbon skeleton.",
        "Position isomers have the same functional group at different positions.",
        "Functional group isomers have different functional groups.",
        "Geometrical isomers arise due to restricted rotation around a double bond.",
        "Optical isomers are non-superimposable mirror images of each other.",
        "Metamers differ in alkyl groups on either side of a functional group.",
        "Butane and isobutane are the simplest chain isomers!",
        "Lactic acid exists as two optical isomers: D and L forms.",
        "cis and trans isomers have different physical properties!",
        "The word isomer comes from Greek: isos (equal) and meros (part).",
        "Isomers can have completely different chemical properties!",
    };

    string GetRandomFact() =>
        isomerFacts[Random.Range(0, isomerFacts.Length)];

    // ─────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Hide all panels
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (congratsPanel != null) congratsPanel.SetActive(false);
        if (inputBlocker != null) inputBlocker.SetActive(false);

        // Assign button listeners
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryClicked);

        // Check if this is the last level
        isLastLevel = GameData.currentLevelIndex >= 5;

        // Play snake music
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySnakeMusic();

        LoadFromGameData();
    }

    void Update()
    {
        if (!timerRunning || isGameOver) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            LoseLives();
        }
    }

    // ─────────────────────────────────────────
    //  INIT
    // ─────────────────────────────────────────

    void LoadFromGameData()
    {
        lives = 2;
        timeRemaining = 120f;
        currentOrderIndex = 0;
        isGameOver = false;
        currentScore = GameData.currentScore;

        requiredOrder = new List<Ball.FragmentType>(
            GameData.pottedBallsInOrder);

        if (moleculeNameText != null)
            moleculeNameText.text = GameData.isomerismType +
                ": " + GameData.moleculeName +
                " (" + GameData.moleculeFormula + ")";

        UpdateScoreUI();
        UpdateRequiredOrderUI();
        UpdateFragmentsCollectedUI();
        UpdateHeartsUI();

        SpawnBalls();

        if (snakeController != null)
            snakeController.Initialize();

        timerRunning = true;
    }

    // ─────────────────────────────────────────
    //  BALL SPAWNING
    // ─────────────────────────────────────────

    void SpawnBalls()
    {
        foreach (GameObject b in spawnedBalls)
            if (b != null) Destroy(b);
        spawnedBalls.Clear();

        foreach (Ball.FragmentType ft in requiredOrder)
        {
            if (!GameData.fragmentToPrefab.ContainsKey(ft)) continue;

            GameObject prefab = GameData.fragmentToPrefab[ft];
            Vector3 pos = GetRandomSpawnPos();

            GameObject ball = Instantiate(prefab, pos, Quaternion.identity);
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.fragmentType = ft;
                ballScript.isPotted = false;
            }

            // Kinematic in snake scene
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }

            spawnedBalls.Add(ball);
        }
    }

    Vector3 GetRandomSpawnPos()
    {
        Vector3 pos;
        int attempts = 0;
        do
        {
            pos = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                0f);
            attempts++;
        }
        while (IsTooClose(pos) && attempts < 20);
        return pos;
    }

    bool IsTooClose(Vector3 pos)
    {
        foreach (GameObject b in spawnedBalls)
            if (b != null && Vector3.Distance(b.transform.position, pos) < 0.8f)
                return true;
        return false;
    }

    // ─────────────────────────────────────────
    //  BALL EATING
    // ─────────────────────────────────────────

    public void BallEaten(GameObject ball, Ball.FragmentType fragmentType)
    {
        if (isGameOver) return;
        if (currentOrderIndex >= requiredOrder.Count) return;

        Ball.FragmentType required = requiredOrder[currentOrderIndex];

        if (fragmentType == required)
        {
            currentScore += 10;
            currentOrderIndex++;

            // Add ball to snake body
            if (snakeController != null)
                snakeController.AddBallToBody(ball);

            // Play correct SFX
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySnakeBallCorrect();

            UpdateScoreUI();
            UpdateRequiredOrderUI();
            UpdateFragmentsCollectedUI();

            if (currentOrderIndex >= requiredOrder.Count)
                LevelComplete();
        }
        else
        {
            // Play wrong SFX immediately
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySnakeBallWrong();

            StartCoroutine(WrongBallEaten(ball));
        }
    }

    IEnumerator WrongBallEaten(GameObject ball)
    {
        lives--;
        UpdateHeartsUI();

        if (lives <= 0)
        {
            LoseLives();
            yield break;
        }

        // Blink snake
        if (snakeController != null)
            yield return StartCoroutine(snakeController.BlinkSnake());

        // Respawn ball
        if (ball != null)
        {
            ball.SetActive(true);
            ball.transform.position = GetRandomSpawnPos();

            SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

            ball.transform.localScale = Vector3.one * 0.9f;

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.zero;
            }

            Ball bs = ball.GetComponent<Ball>();
            if (bs != null) bs.isPotted = false;
        }
    }

    // ─────────────────────────────────────────
    //  WIN / LOSE
    // ─────────────────────────────────────────

    void LevelComplete()
    {
        isGameOver = true;
        timerRunning = false;
        GameData.currentScore = currentScore;

        if (snakeController != null)
            snakeController.StopSnake();

        // Play win SFX
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySnakeLevelComplete();

        if (winPanel != null) winPanel.SetActive(true);
        if (inputBlocker != null) inputBlocker.SetActive(true);

        if (winScoreText != null)
            winScoreText.text = "CURRENT SCORE: " + currentScore;
        if (winFactText != null)
            winFactText.text = GetRandomFact();

        // Change button text for last level
        if (nextButtonText != null)
            nextButtonText.text = isLastLevel ? "CONGRATULATIONS!" : "NEXT";
    }

    void LoseLives()
    {
        isGameOver = true;
        timerRunning = false;

        if (snakeController != null)
            snakeController.StopSnake();

        // Play lose SFX
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySnakeLoseLives();

        if (losePanel != null) losePanel.SetActive(true);
        if (inputBlocker != null) inputBlocker.SetActive(true);

        if (loseScoreText != null)
            loseScoreText.text = "CURRENT SCORE: " + currentScore;
        if (loseFactText != null)
            loseFactText.text = GetRandomFact();
    }

    // ─────────────────────────────────────────
    //  UI UPDATES
    // ─────────────────────────────────────────

    void UpdateTimerUI()
    {
        if (timerText == null) return;
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = seconds.ToString();
        timerText.color = seconds <= 30 ? Color.red : Color.white;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE:\n" + currentScore;
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartObjects.Length; i++)
            if (heartObjects[i] != null)
                heartObjects[i].SetActive(i < lives);
    }

    void UpdateRequiredOrderUI()
    {
        // Hide all slots first
        for (int i = 0; i < requiredOrderSlots.Length; i++)
        {
            if (requiredOrderSlots[i] != null)
                requiredOrderSlots[i].SetActive(false);

            if (requiredOrderTexts != null &&
                i < requiredOrderTexts.Length &&
                requiredOrderTexts[i] != null)
                requiredOrderTexts[i].text = "";

            if (requiredOrderImages != null &&
                i < requiredOrderImages.Length &&
                requiredOrderImages[i] != null)
                requiredOrderImages[i].sprite = null;
        }

        if (requiredOrder == null) return;

        for (int i = 0; i < requiredOrder.Count; i++)
        {
            if (i >= requiredOrderSlots.Length) break;

            Ball.FragmentType ft = requiredOrder[i];

            if (requiredOrderSlots[i] != null)
                requiredOrderSlots[i].SetActive(true);

            string name = fragmentNames.ContainsKey(ft) ?
                fragmentNames[ft] : ft.ToString();

            if (requiredOrderTexts != null &&
                i < requiredOrderTexts.Length &&
                requiredOrderTexts[i] != null)
            {
                if (i == currentOrderIndex)
                    requiredOrderTexts[i].text =
                        "<color=yellow><b>" + name + "</b></color>";
                else if (i < currentOrderIndex)
                    requiredOrderTexts[i].text =
                        "<color=green>" + name + "</color>";
                else
                    requiredOrderTexts[i].text = name;
            }

            if (requiredOrderImages != null &&
                i < requiredOrderImages.Length &&
                requiredOrderImages[i] != null)
            {
                Sprite sprite = GetFragmentSprite(ft);
                if (sprite != null)
                    requiredOrderImages[i].sprite = sprite;
            }
        }
    }

    Sprite GetFragmentSprite(Ball.FragmentType ft)
    {
        if (GameData.fragmentToPrefab == null ||
            !GameData.fragmentToPrefab.ContainsKey(ft)) return null;
        GameObject prefab = GameData.fragmentToPrefab[ft];
        if (prefab == null) return null;
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        return sr != null ? sr.sprite : null;
    }

    void UpdateFragmentsCollectedUI()
    {
        if (fragmentsCollectedText == null) return;

        var required = new Dictionary<Ball.FragmentType, int>();
        var collected = new Dictionary<Ball.FragmentType, int>();

        foreach (Ball.FragmentType ft in requiredOrder)
        {
            if (!required.ContainsKey(ft)) required[ft] = 0;
            required[ft]++;
            if (!collected.ContainsKey(ft)) collected[ft] = 0;
        }

        for (int i = 0; i < currentOrderIndex; i++)
        {
            Ball.FragmentType ft = requiredOrder[i];
            if (!collected.ContainsKey(ft)) collected[ft] = 0;
            collected[ft]++;
        }

        string txt = "";
        foreach (var kvp in required)
        {
            string name = fragmentNames.ContainsKey(kvp.Key) ?
                fragmentNames[kvp.Key] : kvp.Key.ToString();
            int col = collected.ContainsKey(kvp.Key) ?
                collected[kvp.Key] : 0;
            txt += name + ": " + col + " / " + kvp.Value + "\n";
        }

        fragmentsCollectedText.text = txt.TrimEnd();
    }

    // ─────────────────────────────────────────
    //  BUTTON HANDLERS
    // ─────────────────────────────────────────

    public void OnNextClicked()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (inputBlocker != null) inputBlocker.SetActive(false);

        if (isLastLevel)
        {
            // All 6 levels done
            StartCoroutine(ShowCongratsAndGoToTitle());
        }
        else
        {
            // Move to next level
            GameData.currentLevelIndex++;
            GameData.savedMolecule = null;
            GameData.currentScore = currentScore;

            // Switch to gameplay music
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayGameplayMusic();

            SceneManager.LoadScene("GameplayScene");
        }
    }

    public void OnRetryClicked()
    {
        if (losePanel != null) losePanel.SetActive(false);
        if (inputBlocker != null) inputBlocker.SetActive(false);

        // Retry snake scene with same data
        SceneManager.LoadScene("SnakeScene");
    }

    IEnumerator ShowCongratsAndGoToTitle()
    {
        // Show congratulations
        if (congratsPanel != null)
        {
            congratsPanel.SetActive(true);
            if (congratsImage != null && congratsSprite != null)
                congratsImage.sprite = congratsSprite;
        }

        // Wait 3 seconds
        yield return new WaitForSeconds(3f);

        // Reset all data
        GameData.ResetAll();

        // Play title music
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayTitleMusic();

        // Go to gameplay scene — title panel shows automatically
        SceneManager.LoadScene("GameplayScene");
    }
}