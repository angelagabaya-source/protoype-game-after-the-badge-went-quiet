using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusic : MonoBehaviour
{
    public static PersistentMusic Instance;

    [Header("Audio Source Components")]
    public AudioSource musicSource;    
    public AudioSource ambientSource1; 
    public AudioSource ambientSource2; 

    // This "Shortcut" fixes the error in GameManager/PauseManager
    public AudioSource audioSource => musicSource;

    [Header("Menu Track")]
    public AudioClip menuMusic;

    [Header("Level Layer Tracks")]
    public AudioClip levelMusic;
    public AudioClip levelAmbience1;
    public AudioClip levelAmbience2;

    [Range(0, 1)] 
    public float defaultVolume = 0.5f;

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
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePauseVolume(false);

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

    void PlayTrack(AudioSource source, AudioClip clip, bool loop)
    {
        if (source == null || clip == null) return;
        if (source.clip == clip && source.isPlaying) return;

        source.clip = clip;
        source.loop = loop;
        source.Play();
    }

    public void UpdatePauseVolume(bool isPaused)
    {
        float targetVol = isPaused ? defaultVolume * 0.3f : defaultVolume;
        
        if (musicSource) musicSource.volume = targetVol;
        if (ambientSource1) ambientSource1.volume = targetVol;
        if (ambientSource2) ambientSource2.volume = targetVol;
    }

    public void SetGlobalVolume(float volume)
    {
        defaultVolume = volume;
        UpdatePauseVolume(false);
    }
}