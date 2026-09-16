using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public string[] levelIsomerismTypes = {
        "Chain Isomerism",
        "Position Isomerism",
        "Functional Group Isomerism",
        "Metamerism",
        "Geometrical Isomerism",
        "Optical Isomerism"
    };

    void Start()
    {
        // Don't auto start — wait for Enter button
        // TitlePanelManager handles this
    }

    private MoleculeData savedMolecule; // Save molecule for retry

    // Fragment types introduced per level
    private Ball.FragmentType[][] levelFragmentTypes = new Ball.FragmentType[][]
{
    // Level 1 - Chain Isomerism
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C
    },

    // Level 2 - Position Isomerism (ketones use C for carbonyl)
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C,
        Ball.FragmentType.OH
    },

    // Level 3 - Functional Group (aldehydes use CH, ketones use C)
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C,
        Ball.FragmentType.OH,
        Ball.FragmentType.COOH
    },

    // Level 4 - Metamerism
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C,
        Ball.FragmentType.OH,
        Ball.FragmentType.COOH,
        Ball.FragmentType.NH2
    },

    // Level 5 - Geometrical Isomerism
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C,
        Ball.FragmentType.OH,
        Ball.FragmentType.COOH
    },

    // Level 6 - Optical Isomerism
    new Ball.FragmentType[] {
        Ball.FragmentType.CH3,
        Ball.FragmentType.CH2,
        Ball.FragmentType.CH,
        Ball.FragmentType.C,
        Ball.FragmentType.OH,
        Ball.FragmentType.COOH,
        Ball.FragmentType.NH2
    },
};

    public int currentLevelIndex = 0;
    public MoleculeData currentMolecule;

    [Header("Ball Prefabs")]
    public GameObject[] allBallPrefabs; // 6 prefabs

    [Header("Cue Ball")]
    public GameObject cueBall;
    public Vector3 cueBallSpawnPoint = new Vector3(-2f, 0f, 0f);

    [Header("Timer")]
    public float timeRemaining = 180f;
    public bool timerRunning = false;
    public TextMeshProUGUI timerText;

    [Header("Score")]
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;

    [Header("UI")]
    public TextMeshProUGUI fragmentsText;
    public TextMeshProUGUI moleculeNameText;

    [Header("Ball Legend")]
    public GameObject[] legendRows;       // 6 legend rows max
    public Image[] legendImages;          // 6 legend images
    public TextMeshProUGUI[] legendTexts; // 6 legend texts

    [Header("Game State")]
    public bool isGameOver = false;

    // Fragment tracking
    private Dictionary<Ball.FragmentType, int> requiredCounts = new Dictionary<Ball.FragmentType, int>();
    private Dictionary<Ball.FragmentType, int> collectedCounts = new Dictionary<Ball.FragmentType, int>();
    private List<Ball.FragmentType> pottedOrder = new List<Ball.FragmentType>();

    // Color assignments
    private Dictionary<Ball.FragmentType, GameObject> fragmentToPrefab = new Dictionary<Ball.FragmentType, GameObject>();
    private List<GameObject> wrongBallPrefabs = new List<GameObject>();
    private List<GameObject> spawnedBalls = new List<GameObject>();

    // Current legend level types
    private Ball.FragmentType[] currentLegendTypes;

    // Fragment display names
    private Dictionary<Ball.FragmentType, string> fragmentNames = new Dictionary<Ball.FragmentType, string>()
    {
        { Ball.FragmentType.CH3, "CH₃" },
        { Ball.FragmentType.CH2, "CH₂" },
        { Ball.FragmentType.CH,  "CH"  },
        { Ball.FragmentType.C,   "C"   },
        { Ball.FragmentType.OH,  "OH"  },
        { Ball.FragmentType.COOH,"COOH"},
        { Ball.FragmentType.NH2, "NH₂" },
    };

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (timerRunning && !isGameOver)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();

                if (timeRemaining <= 30f && AudioManager.Instance != null)
                    AudioManager.Instance.PlayTimerWarning();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                TimerExpired();
            }
        }
    }

    public void LoadLevel(int index, bool isRetry = false)
    {
        pottedOrder.Clear();
        currentLevelIndex = index;
        isGameOver = false;
        currentScore = 0;
        timeRemaining = 180f;
        timerRunning = false;

        // Reset cue ball
        ResetCueBall();

        // If retry use SAME molecule, otherwise pick new one
        if (isRetry && savedMolecule != null)
            currentMolecule = savedMolecule;
        else
        {
            currentMolecule = MoleculeDatabase.GetRandomMolecule(index);
            savedMolecule = currentMolecule; // Save for retry
        }

        if (moleculeNameText != null)
            moleculeNameText.text = levelIsomerismTypes[index] +
                ": " + currentMolecule.moleculeName +
                " (" + currentMolecule.formula + ")";

        BuildRequiredCounts();
        AssignRandomColors();
        UpdateFragmentsUI();
        UpdateScoreUI();
        SpawnBalls();

        if (AudioManager.Instance != null)
            AudioManager.Instance.ResetTimerWarning();

        timerRunning = true;
    }

    void BuildRequiredCounts()
    {
        requiredCounts.Clear();
        collectedCounts.Clear();

        if (currentMolecule.ch3Count > 0)  { requiredCounts[Ball.FragmentType.CH3]  = currentMolecule.ch3Count;  collectedCounts[Ball.FragmentType.CH3]  = 0; }
        if (currentMolecule.ch2Count > 0)  { requiredCounts[Ball.FragmentType.CH2]  = currentMolecule.ch2Count;  collectedCounts[Ball.FragmentType.CH2]  = 0; }
        if (currentMolecule.chCount > 0)   { requiredCounts[Ball.FragmentType.CH]   = currentMolecule.chCount;   collectedCounts[Ball.FragmentType.CH]   = 0; }
        if (currentMolecule.cCount > 0)    { requiredCounts[Ball.FragmentType.C]    = currentMolecule.cCount;    collectedCounts[Ball.FragmentType.C]    = 0; }
        if (currentMolecule.ohCount > 0)   { requiredCounts[Ball.FragmentType.OH]   = currentMolecule.ohCount;   collectedCounts[Ball.FragmentType.OH]   = 0; }
        if (currentMolecule.coohCount > 0) { requiredCounts[Ball.FragmentType.COOH] = currentMolecule.coohCount; collectedCounts[Ball.FragmentType.COOH] = 0; }
        if (currentMolecule.nh2Count > 0)  { requiredCounts[Ball.FragmentType.NH2]  = currentMolecule.nh2Count;  collectedCounts[Ball.FragmentType.NH2]  = 0; }
    }

    void AssignRandomColors()
    {
        fragmentToPrefab.Clear();
        wrongBallPrefabs.Clear();

        // Get fragment types for this level
        Ball.FragmentType[] levelTypes = levelFragmentTypes[currentLevelIndex];
        currentLegendTypes = levelTypes;

        // Shuffle all prefabs
        List<GameObject> shuffled = new List<GameObject>(allBallPrefabs);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rand = Random.Range(i, shuffled.Count);
            GameObject temp = shuffled[i];
            shuffled[i] = shuffled[rand];
            shuffled[rand] = temp;
        }

        // Assign prefabs to fragment types
        for (int i = 0; i < levelTypes.Length && i < shuffled.Count; i++)
            fragmentToPrefab[levelTypes[i]] = shuffled[i];

        // Rest are wrong balls
        for (int i = levelTypes.Length; i < shuffled.Count; i++)
            wrongBallPrefabs.Add(shuffled[i]);

        // Update legend UI
        UpdateLegendUI();
    }

    void UpdateLegendUI()
    {
        // Hide and clear all rows first
        for (int i = 0; i < legendRows.Length; i++)
        {
            if (legendRows[i] != null)
                legendRows[i].SetActive(false);

            if (legendTexts[i] != null)
                legendTexts[i].text = "";

            if (legendImages[i] != null)
                legendImages[i].sprite = null;
        }

        if (currentLegendTypes == null) return;

        int rowIdx = 0;
        foreach (Ball.FragmentType ft in currentLegendTypes)
        {
            // Only show if this molecule actually uses this fragment
            if (!requiredCounts.ContainsKey(ft)) continue;
            if (rowIdx >= legendRows.Length) break;

            if (legendRows[rowIdx] != null)
                legendRows[rowIdx].SetActive(true);

            if (legendImages[rowIdx] != null && fragmentToPrefab.ContainsKey(ft))
                legendImages[rowIdx].sprite = fragmentToPrefab[ft].GetComponent<SpriteRenderer>().sprite;

            int required = requiredCounts[ft];
            int collected = collectedCounts.ContainsKey(ft) ? collectedCounts[ft] : 0;
            string name = fragmentNames.ContainsKey(ft) ? fragmentNames[ft] : ft.ToString();

            if (legendTexts[rowIdx] != null)
                legendTexts[rowIdx].text = $"{name} : {collected} / {required}";

            rowIdx++;
        }
    }

    void SpawnBalls()
    {
        // Clear old balls
        foreach (GameObject b in spawnedBalls)
            if (b != null) Destroy(b);
        spawnedBalls.Clear();

        // Build correct balls list
        List<(GameObject prefab, Ball.FragmentType type)> correctBalls =
            new List<(GameObject, Ball.FragmentType)>();

        foreach (var kvp in requiredCounts)
        {
            if (!fragmentToPrefab.ContainsKey(kvp.Key)) continue;
            for (int i = 0; i < kvp.Value; i++)
                correctBalls.Add((fragmentToPrefab[kvp.Key], kvp.Key));
        }

        // Build wrong balls list
        List<(GameObject prefab, Ball.FragmentType type)> wrongBalls =
            new List<(GameObject, Ball.FragmentType)>();
        foreach (var wb in wrongBallPrefabs)
            wrongBalls.Add((wb, Ball.FragmentType.Wrong));

        // Combine: correct balls + wrong balls
        List<(GameObject, Ball.FragmentType)> allBalls =
            new List<(GameObject, Ball.FragmentType)>(correctBalls);
        allBalls.AddRange(wrongBalls);

        // Shuffle
        for (int i = 0; i < allBalls.Count; i++)
        {
            int rand = Random.Range(i, allBalls.Count);
            var temp = allBalls[i];
            allBalls[i] = allBalls[rand];
            allBalls[rand] = temp;
        }

        // Get triangle positions
        Vector3[] positions = GetTrianglePositions(allBalls.Count);

        // Spawn
        for (int i = 0; i < allBalls.Count; i++)
        {
            GameObject ball = Instantiate(allBalls[i].Item1, positions[i], Quaternion.identity);
            Ball ballScript = ball.GetComponent<Ball>();

            if (allBalls[i].Item2 == Ball.FragmentType.Wrong)
            {
                ball.tag = "WrongBall";
                if (ballScript != null) ballScript.fragmentType = Ball.FragmentType.Wrong;
            }
            else
            {
                ball.tag = "Ball";
                if (ballScript != null) ballScript.fragmentType = allBalls[i].Item2;
            }

            spawnedBalls.Add(ball);
        }
    }

    // Triangle formation
    Vector3[] GetTrianglePositions(int ballCount)
    {
        float ballSpacing = 0.95f;
        float rowSpacingX = ballSpacing * 0.866f;
        float apexX = 0.5f;

        // Calculate rows needed
        int rows = 0;
        while (rows * (rows + 1) / 2 < ballCount) rows++;

        List<Vector3> positions = new List<Vector3>();
        int ballIdx = 0;

        for (int row = 0; row < rows && ballIdx < ballCount; row++)
        {
            int ballsInRow = row + 1;
            float xPos = apexX + row * rowSpacingX;
            float totalHeight = (ballsInRow - 1) * ballSpacing;
            float startY = totalHeight / 2f;

            for (int col = 0; col < ballsInRow && ballIdx < ballCount; col++)
            {
                float yPos = startY - col * ballSpacing;
                positions.Add(new Vector3(xPos, yPos, 0f));
                ballIdx++;
            }
        }

        return positions.ToArray();
    }

    public void BallPotted(Ball.FragmentType fragmentType)
    {
        if (!collectedCounts.ContainsKey(fragmentType)) return;

        currentScore += 10;
        collectedCounts[fragmentType]++;
        pottedOrder.Add(fragmentType); // Track order

        UpdateScoreUI();
        UpdateFragmentsUI();
        UpdateLegendUI(); // update per-row count in legend

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBallPotted();

        Debug.Log("Correct! " + fragmentType + " Score: " + currentScore);

        bool allDone = true;
        foreach (var kvp in requiredCounts)
        {
            if (!collectedCounts.ContainsKey(kvp.Key) ||
                collectedCounts[kvp.Key] < kvp.Value)
            {
                allDone = false;
                break;
            }
        }

        if (allDone) LevelComplete();
    }

    public void WrongBallPotted(GameObject wrongBall)
    {
        currentScore -= 10;
        if (currentScore < 0) currentScore = 0;
        timeRemaining -= 5f;
        if (timeRemaining < 0) timeRemaining = 0;

        UpdateScoreUI();
        UpdateTimerUI();
        RespawnBall(wrongBall);
        ResetCueBall();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWrongBall();

        Debug.Log("Wrong ball! -10 score -5 sec");
    }

    public void CueBallPotted()
    {
        timeRemaining -= 5f;
        if (timeRemaining < 0) timeRemaining = 0;
        UpdateTimerUI();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCueBallScratch();
    }

    public void RespawnBall(GameObject ball)
    {
        Vector3[] positions = GetTrianglePositions(10);
        Vector3 randomPos = positions[Random.Range(0, positions.Length)];

        ball.transform.localScale = Vector3.one * 0.9f;
        ball.transform.position = randomPos;
        ball.SetActive(true);

        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript != null) ballScript.isPotted = false;

        SpriteRenderer sr = ball.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void ResetCueBall()
    {
        if (cueBall == null) return;
        cueBall.transform.position = cueBallSpawnPoint;
        Rigidbody2D rb = cueBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

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

    void UpdateFragmentsUI()
    {
        if (fragmentsText == null) return;
        string txt = "";
        foreach (var kvp in requiredCounts)
        {
            int collected = collectedCounts.ContainsKey(kvp.Key) ? collectedCounts[kvp.Key] : 0;
            string name = fragmentNames.ContainsKey(kvp.Key) ? fragmentNames[kvp.Key] : kvp.Key.ToString();
            txt += name + ": " + collected + " / " + kvp.Value + "\n";
        }
        fragmentsText.text = txt.TrimEnd();
    }

    void TimerExpired()
    {
        isGameOver = true;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayLose();
        if (UIManager.Instance != null) UIManager.Instance.ShowLosePanel(currentScore);
        Debug.Log("Time up!");
    }

    void LevelComplete()
{
    timerRunning = false;
    isGameOver = true;

    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayWin();

    // Build CORRECT chemical order instead of potting order
    List<Ball.FragmentType> correctOrder = BuildCorrectMoleculeOrder();

    GameData.Clear();
    GameData.pottedBallsInOrder = correctOrder;
    GameData.fragmentToPrefab = new Dictionary<Ball.FragmentType, GameObject>(fragmentToPrefab);
    GameData.moleculeName = currentMolecule.moleculeName;
    GameData.moleculeFormula = currentMolecule.formula;
    GameData.isomerismType = levelIsomerismTypes[currentLevelIndex];
    GameData.currentLevelIndex = currentLevelIndex;
    GameData.currentScore = currentScore;
    GameData.currentMolecule = currentMolecule;

    if (UIManager.Instance != null)
        UIManager.Instance.ShowWinPanelThenSnake(currentScore);
}

List<Ball.FragmentType> BuildCorrectMoleculeOrder()
{
    // If molecule has explicit structural order, use it
    if (currentMolecule.structuralOrder != null &&
        currentMolecule.structuralOrder.Length > 0)
    {
        return new List<Ball.FragmentType>(currentMolecule.structuralOrder);
    }

    // Fallback — grouped order (old behavior)
    List<Ball.FragmentType> order = new List<Ball.FragmentType>();

    if (currentMolecule.ch3Count > 0)
        for (int i = 0; i < currentMolecule.ch3Count; i++)
            order.Add(Ball.FragmentType.CH3);
    if (currentMolecule.ch2Count > 0)
        for (int i = 0; i < currentMolecule.ch2Count; i++)
            order.Add(Ball.FragmentType.CH2);
    if (currentMolecule.chCount > 0)
        for (int i = 0; i < currentMolecule.chCount; i++)
            order.Add(Ball.FragmentType.CH);
    if (currentMolecule.cCount > 0)
        for (int i = 0; i < currentMolecule.cCount; i++)
            order.Add(Ball.FragmentType.C);
    if (currentMolecule.ohCount > 0)
        for (int i = 0; i < currentMolecule.ohCount; i++)
            order.Add(Ball.FragmentType.OH);
    if (currentMolecule.coohCount > 0)
        for (int i = 0; i < currentMolecule.coohCount; i++)
            order.Add(Ball.FragmentType.COOH);
    if (currentMolecule.nh2Count > 0)
        for (int i = 0; i < currentMolecule.nh2Count; i++)
            order.Add(Ball.FragmentType.NH2);

    return order;
}
}