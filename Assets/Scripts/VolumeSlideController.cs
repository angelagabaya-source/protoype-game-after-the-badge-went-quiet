using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    private Slider volumeSlider;

    void Start()
    {
        volumeSlider = GetComponent<Slider>();

        if (PersistentMusic.Instance != null)
        {
            // Sync the slider handle to the current volume
            volumeSlider.value = PersistentMusic.Instance.defaultVolume;
            
            // Hook up the event via code to ensure it's "Dynamic"
            volumeSlider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
        }
    }

    public void OnSliderValueChange()
    {
        if (PersistentMusic.Instance != null)
        {
            PersistentMusic.Instance.SetGlobalVolume(volumeSlider.value);
        }
    }
}