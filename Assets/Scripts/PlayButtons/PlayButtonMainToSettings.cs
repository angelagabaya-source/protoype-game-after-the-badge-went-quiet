using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonMainToSettings : MonoBehaviour
{
    // This will be called when the Play button is clicked
    public void LoadSettingsUI()
    {
        SceneManager.LoadScene("SettingsUI");
    }
}