using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    // This will be called when the Play button is clicked
    public void LoadLevelUI()
    {
        SceneManager.LoadScene("LevelUI");
    }
}