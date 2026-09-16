using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioClip titleMusic;
    public AudioClip gameplayMusic;
    public AudioClip snakeMusic;

    [Header("SFX — Pool")]
    public AudioClip ballHitClip;
    public AudioClip ballPottedClip;
    public AudioClip wrongBallClip;
    public AudioClip cueBallScratchClip;
    public AudioClip timerWarningClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    [Header("SFX — Snake")]
    public AudioClip snakeBallCorrectClip;
    public AudioClip snakeBallWrongClip;
    public AudioClip snakeLevelCompleteClip;
    public AudioClip snakeLoseLivesClip;

    [Header("Settings")]
    public float musicVolume = 0.5f;
    public float sfxVolume = 1f;
    public float fadeDuration = 0.5f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool timerWarnPlayed = false;

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

        // Create music source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        // Create SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;

        // Apply saved music setting
        ApplyMusicSetting();
    }

    void ApplyMusicSetting()
    {
        bool isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        musicSource.mute = !isOn;
        sfxSource.mute = !isOn;
    }

    public void SetMusicOn(bool isOn)
    {
        musicSource.mute = !isOn;
        sfxSource.mute = !isOn;
    }

    // ---- Music Methods ----

    public void PlayTitleMusic()
    {
        if (titleMusic == null) return;
        if (musicSource.clip == titleMusic && musicSource.isPlaying) return;
        StartCoroutine(FadeToMusic(titleMusic));
    }

    public void PlayGameplayMusic()
    {
        if (gameplayMusic == null) return;
        if (musicSource.clip == gameplayMusic && musicSource.isPlaying) return;
        StartCoroutine(FadeToMusic(gameplayMusic));
    }

    public void PlaySnakeMusic()
    {
        if (snakeMusic == null) return;
        if (musicSource.clip == snakeMusic && musicSource.isPlaying) return;
        StartCoroutine(FadeToMusic(snakeMusic));
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    IEnumerator FadeToMusic(AudioClip newClip)
    {
        // Fade out current
        if (musicSource.isPlaying)
        {
            float startVol = musicSource.volume;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVol, 0f, elapsed / fadeDuration);
                yield return null;
            }
            musicSource.Stop();
        }

        // Switch clip and fade in
        musicSource.clip = newClip;
        musicSource.volume = 0f;
        musicSource.Play();

        float fadeElapsed = 0f;
        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, fadeElapsed / fadeDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    IEnumerator FadeOutMusic()
    {
        float startVol = musicSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, elapsed / fadeDuration);
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = musicVolume;
    }

    // ---- Pool SFX ----

    public void PlayBallHit() => PlaySFX(ballHitClip);
    public void PlayBallPotted() => PlaySFX(ballPottedClip);
    public void PlayWrongBall() => PlaySFX(wrongBallClip);
    public void PlayCueBallScratch() => PlaySFX(cueBallScratchClip);
    public void PlayWin() => PlaySFX(winClip);
    public void PlayLose() => PlaySFX(loseClip);

    public void PlayTimerWarning()
    {
        if (!timerWarnPlayed)
        {
            timerWarnPlayed = true;
            PlaySFX(timerWarningClip);
        }
    }

    public void ResetTimerWarning()
    {
        timerWarnPlayed = false;
    }

    // ---- Snake SFX ----

    public void PlaySnakeBallCorrect() => PlaySFX(snakeBallCorrectClip);
    public void PlaySnakeBallWrong() => PlaySFX(snakeBallWrongClip);
    public void PlaySnakeLevelComplete() => PlaySFX(snakeLevelCompleteClip);
    public void PlaySnakeLoseLives() => PlaySFX(snakeLoseLivesClip);

    // ---- Helper ----

    void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
        musicSource.volume = vol;
    }

    public void SetSFXVolume(float vol)
    {
        sfxVolume = vol;
        sfxSource.volume = vol;
    }
}