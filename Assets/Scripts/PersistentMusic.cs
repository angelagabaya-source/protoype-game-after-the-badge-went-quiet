using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusic : MonoBehaviour
{
    public static PersistentMusic Instance;

    [Header("Audio Source Components")]
    public AudioSource musicSource;    
    public AudioSource ambientSource1; 
    public AudioSource ambientSource2; 

    // Shortcut for older scripts looking for 'audioSource'
    public AudioSource audioSource => musicSource;

    [Header("Menu Track")]
    public AudioClip menuMusic;

    [Header("Level Layer Tracks")]
    public AudioClip levelMusic;
    public AudioClip levelAmbience1;
    public AudioClip levelAmbience2;

    [HideInInspector] public float defaultVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load saved volume immediately on boot
            defaultVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
            ApplyVolumeToAll(defaultVolume);
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
        // Re-apply the saved volume every time a scene loads
        ApplyVolumeToAll(defaultVolume);

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

    // This is what the Slider calls
    public void SetGlobalVolume(float volume)
    {
        defaultVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        ApplyVolumeToAll(volume);
    }

    public void UpdatePauseVolume(bool isPaused)
    {
        // If paused, drop to 30% of whatever the slider is currently set to
        float targetVol = isPaused ? defaultVolume * 0.3f : defaultVolume;
        ApplyVolumeToAll(targetVol);
    }

    private void ApplyVolumeToAll(float vol)
    {
        if (musicSource) musicSource.volume = vol;
        if (ambientSource1) ambientSource1.volume = vol;
        if (ambientSource2) ambientSource2.volume = vol;
    }
}