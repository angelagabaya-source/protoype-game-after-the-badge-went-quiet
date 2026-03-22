using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandlerLevelUI : MonoBehaviour
{
    // This will be called when the Play button is clicked
    public void LoadMainMenuGUI()
    {
        SceneManager.LoadScene("MainMenuGUI");
    }
}