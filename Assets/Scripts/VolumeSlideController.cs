using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public enum VolumeType { Music, SFX }
    public VolumeType targetType; // Choose Music or SFX in the Inspector!
    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        if (PersistentMusic.Instance != null)
        {
            // Sync slider to the correct saved volume
            float savedVol = (targetType == VolumeType.Music) ? PersistentMusic.Instance.musicVolume : PersistentMusic.Instance.sfxVolume;
            volumeSlider.value = savedVol;
            
            volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        }
    }

    public void OnSliderValueChange()
    {
        if (PersistentMusic.Instance != null)
        {
            if (targetType == VolumeType.Music)
                PersistentMusic.Instance.SetMusicVolume(volumeSlider.value);
            else
                PersistentMusic.Instance.SetSFXVolume(volumeSlider.value);
        }
    }
}