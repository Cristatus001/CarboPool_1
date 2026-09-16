using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TitlePanelManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject titlePanel;

    [Header("Buttons")]
    public Button enterButton;
    public Button settingsButton;
    public Button musicButton;
    public Button quitButton;

    [Header("Settings Dropdown")]
    public RectTransform settingsDropdown;
    public Image musicIcon;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("Animation")]
    public float animSpeed = 0.25f;

    private bool isOpen = false;
    private bool isAnimating = false;
    private Vector2 hiddenScale = new Vector2(1f, 0f);
    private Vector2 shownScale = new Vector2(1f, 1f);

    void Awake()
    {
        // Create SoundManager if needed
        if (SoundManager.Instance == null)
        {
            GameObject sm = new GameObject("SoundManager");
            sm.AddComponent<SoundManager>();
        }

        // Create AudioManager if needed
        if (AudioManager.Instance == null)
        {
            GameObject am = new GameObject("AudioManager");
            am.AddComponent<AudioManager>();
        }
    }

    void Start()
    {
        settingsDropdown.localScale = hiddenScale;
        settingsDropdown.gameObject.SetActive(false);

        if (!GameData.gameStarted)
        {
            // Fresh start — show title + play title music
            titlePanel.SetActive(true);
            DisableGameplay();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayTitleMusic();
        }
        else
        {
            // Coming from snake scene — skip title
            titlePanel.SetActive(false);
            EnableGameplay();
        }

        enterButton.onClick.AddListener(OnEnterClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        musicButton.onClick.AddListener(OnMusicClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        UpdateMusicIcon();
    }

    void DisableGameplay()
    {
        CueStick cs = FindFirstObjectByType<CueStick>();
        if (cs != null) cs.enabled = false;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.enabled = false;
    }

    void EnableGameplay()
    {
        CueStick cs = FindFirstObjectByType<CueStick>();
        if (cs != null) cs.enabled = true;

        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.enabled = true;
            gm.LoadLevel(GameData.currentLevelIndex);
        }

        // Switch to gameplay music
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameplayMusic();
    }

    void OnEnterClicked()
    {
        GameData.gameStarted = true;
        titlePanel.SetActive(false);
        EnableGameplay();
    }

    void OnSettingsClicked()
    {
        if (isAnimating) return;
        if (isOpen)
            StartCoroutine(CloseDropdown());
        else
            StartCoroutine(OpenDropdown());
    }

    IEnumerator OpenDropdown()
    {
        isAnimating = true;
        isOpen = true;
        settingsDropdown.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < animSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animSpeed);
            settingsDropdown.localScale = Vector2.Lerp(hiddenScale, shownScale, t);
            yield return null;
        }

        settingsDropdown.localScale = shownScale;
        isAnimating = false;
    }

    IEnumerator CloseDropdown()
    {
        isAnimating = true;

        float elapsed = 0f;
        while (elapsed < animSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / animSpeed);
            settingsDropdown.localScale = Vector2.Lerp(shownScale, hiddenScale, t);
            yield return null;
        }

        settingsDropdown.localScale = hiddenScale;
        settingsDropdown.gameObject.SetActive(false);
        isOpen = false;
        isAnimating = false;
    }

    void OnMusicClicked()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.ToggleMusic();
        UpdateMusicIcon();
    }

    void UpdateMusicIcon()
    {
        if (musicIcon == null) return;
        bool on = SoundManager.Instance != null &&
            SoundManager.Instance.IsMusicOn;
        musicIcon.sprite = on ? musicOnSprite : musicOffSprite;
    }

    void OnQuitClicked()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.QuitGame();
    }

    void Update()
    {
        if (!isOpen || isAnimating) return;

        bool clicked = Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began);

        if (clicked)
        {
            Vector2 clickPos = Input.touchCount > 0 ?
                Input.GetTouch(0).position :
                (Vector2)Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(
                settingsDropdown, clickPos, null))
            {
                StartCoroutine(CloseDropdown());
            }
        }
    }
}