using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Win Panel")]
    public GameObject winPanel;
    public Image winBannerImage;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winFactText;
    public Button nextButton;

    [Header("Lose Panel")]
    public GameObject losePanel;
    public Image loseBannerImage;
    public TextMeshProUGUI loseScoreText;
    public TextMeshProUGUI loseFactText;
    public Button retryButton;
    public Button homeButton;
    public Button homeButtonGameplay;

    [Header("Sprites")]
    public Sprite congratsSprite;
    public Sprite youWinSprite;
    public Sprite youLoseSprite;
    public Sprite winLoseTabSprite;
    [Header("Blocker")]
public GameObject inputBlocker; // Full screen transparent panel

    
    private string[] isomerFacts = new string[]
    {
        "Isomers have the same molecular formula but different structural formulas!",
        "Chain isomers differ in the arrangement of the carbon skeleton.",
        "Position isomers have the same functional group at different positions.",
        "Functional group isomers have different functional groups.",
        "Geometrical isomers arise due to restricted rotation around a double bond.",
        "Optical isomers are non-superimposable mirror images of each other.",
        "Metamers are isomers that differ in the alkyl groups on either side of a functional group.",
        "Butane and isobutane are the simplest chain isomers!",
        "Lactic acid exists as two optical isomers: D and L forms.",
        "cis-2-Butene and trans-2-Butene have different physical properties despite the same formula.",
        "Isomers can have completely different chemical properties!",
        "The word 'isomer' comes from the Greek words isos (equal) and meros (part).",
    };

    

    void Awake()
    {
        Instance = this;

        // Hide panels at start
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Start()
    {
        // Assign button listeners
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextClicked);
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryClicked);
        if (homeButton != null)
            homeButton.onClick.AddListener(OnHomeClicked);
            if (homeButtonGameplay != null)
        homeButtonGameplay.onClick.AddListener(GoToTitle);
    }

   public void ShowWinPanel(int score)
{
    if (inputBlocker != null) inputBlocker.SetActive(true); // Block input
    if (winPanel != null) winPanel.SetActive(true);

    if (winScoreText != null)
        winScoreText.text = "CURRENT SCORE: " + score;
    if (winFactText != null)
        winFactText.text = GetRandomFact();
    if (winBannerImage != null && congratsSprite != null)
        winBannerImage.sprite = congratsSprite;

    // Disable cue stick
    CueStick cs = FindFirstObjectByType<CueStick>();
    if (cs != null) cs.enabled = false;
}

public void ShowLosePanel(int score)
{
    if (inputBlocker != null) inputBlocker.SetActive(true); // Block input
    if (losePanel != null) losePanel.SetActive(true);

    if (loseScoreText != null)
        loseScoreText.text = "CURRENT SCORE: " + score;
    if (loseFactText != null)
        loseFactText.text = GetRandomFact();
    if (loseBannerImage != null && youLoseSprite != null)
        loseBannerImage.sprite = youLoseSprite;

    // Disable cue stick
    CueStick cs = FindFirstObjectByType<CueStick>();
    if (cs != null) cs.enabled = false;
}

void OnNextClicked()
{
    if (inputBlocker != null) inputBlocker.SetActive(false);
    if (winPanel != null) winPanel.SetActive(false);

    // Re-enable cue stick
    CueStick cs = FindFirstObjectByType<CueStick>();
    if (cs != null) cs.enabled = true;

    int nextLevel = GameManager.Instance.currentLevelIndex + 1;
    if (nextLevel < 6)
        GameManager.Instance.LoadLevel(nextLevel);
    else
        ShowGameComplete();
}

void OnRetryClicked()
{
    if (losePanel != null) losePanel.SetActive(false);
    if (inputBlocker != null) inputBlocker.SetActive(false);

    CueStick cs = FindFirstObjectByType<CueStick>();
    if (cs != null) cs.enabled = true;

    // Pass isRetry = true to keep same molecule
    GameManager.Instance.LoadLevel(
        GameManager.Instance.currentLevelIndex, true
    );
}

void OnHomeClicked()
{
    if (losePanel != null) losePanel.SetActive(false);
    SceneManager.LoadScene("TitleScreen");
}

    void ShowGameComplete()
    {
        // Reuse win panel with congratulations
        if (winPanel != null) winPanel.SetActive(true);
        if (winScoreText != null)
            winScoreText.text = "You completed all levels!\nFinal Score: " + GameManager.Instance.currentScore;
        if (winFactText != null)
            winFactText.text = "Congratulations! You are an Isomerism expert!";
        if (winBannerImage != null && congratsSprite != null)
            winBannerImage.sprite = congratsSprite;
    }

    string GetRandomFact()
    {
        return isomerFacts[Random.Range(0, isomerFacts.Length)];
    }
    public void ShowWinPanelThenSnake(int score)
{
    if (inputBlocker != null) inputBlocker.SetActive(true);
    if (winPanel != null) winPanel.SetActive(true);

    if (winScoreText != null)
        winScoreText.text = "CURRENT SCORE: " + score;
    if (winFactText != null)
        winFactText.text = GetRandomFact();

    CueStick cs = FindFirstObjectByType<CueStick>();
    if (cs != null) cs.enabled = false;

    // Change NEXT button to load snake scene
    if (nextButton != null)
    {
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(LoadSnakeScene);
    }
}
void GoToTitle()
{
    // Show title panel
    TitlePanelManager title = FindFirstObjectByType<TitlePanelManager>();
    if (title != null)
        title.titlePanel.SetActive(true);

    // Stop game
    if (GameManager.Instance != null)
        GameManager.Instance.timerRunning = false;
}
void LoadSnakeScene()
{
    SceneManager.LoadScene("SnakeScene");
}
}
