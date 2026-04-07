using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusic : MonoBehaviour
{
    public static PersistentMusic Instance;

    [Header("Audio Source Components")]
    public AudioSource musicSource;    
    public AudioSource ambientSource1; 
    public AudioSource ambientSource2; 
    public AudioSource sfxSource; // The new 4th speaker for SFX

    // Shortcut for older scripts looking for 'audioSource'
    public AudioSource audioSource => musicSource;

    [Header("Music & Ambience Tracks")]
    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioClip levelAmbience1;
    public AudioClip levelAmbience2;

    [Header("SFX Clips")]
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Volume Settings")]
    [HideInInspector] public float musicVolume = 0.5f;
    [HideInInspector] public float sfxVolume = 0.5f;

    // This is the "old" variable name your slider might still be using
    public float defaultVolume => musicVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load saved volumes
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            
            ApplyMusicVolume(musicVolume);
            ApplySFXVolume(sfxVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyMusicVolume(musicVolume);
        ApplySFXVolume(sfxVolume);

        // Scene Logic
        if (scene.name == "MainMenuGUI" || scene.name == "SettingsUI" || scene.name == "CasesUI")
        {
            PlayTrack(musicSource, menuMusic, true);
            if (ambientSource1) ambientSource1.Stop();
            if (ambientSource2) ambientSource2.Stop();
        }
        else
        {
            PlayTrack(musicSource, levelMusic, true);
            PlayTrack(ambientSource1, levelAmbience1, true);
            PlayTrack(ambientSource2, levelAmbience2, true);
        }
    }

    // --- SFX METHODS ---
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // --- VOLUME SETTERS (Called by Sliders) ---
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        ApplyMusicVolume(musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        ApplySFXVolume(sfxVolume);
    }

    // Old method support (for Master slider if you only have one)
    public void SetGlobalVolume(float volume) => SetMusicVolume(volume);

    public void UpdatePauseVolume(bool isPaused)
    {
        float targetVol = isPaused ? musicVolume * 0.3f : musicVolume;
        ApplyMusicVolume(targetVol);
    }

    private void ApplyMusicVolume(float vol)
    {
        if (musicSource) musicSource.volume = vol;
        if (ambientSource1) ambientSource1.volume = vol;
        if (ambientSource2) ambientSource2.volume = vol;
    }

    private void ApplySFXVolume(float vol)
    {
        if (sfxSource) sfxSource.volume = vol;
    }

    void PlayTrack(AudioSource source, AudioClip clip, bool loop)
    {
        if (source == null || clip == null) return;
        if (source.clip == clip && source.isPlaying) return;
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }
}