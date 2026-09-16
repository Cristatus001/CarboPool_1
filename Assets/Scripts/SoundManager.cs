using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private bool isMusicOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        ApplyMusicSetting();
    }

    public bool IsMusicOn => isMusicOn;

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMusicSetting();
    }

    void ApplyMusicSetting()
    {
        // Apply to AudioManager
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicOn(isMusicOn);
        else
            AudioListener.volume = isMusicOn ? 1f : 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}