using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    // This will be called when the Play button is clicked
    public void LoadCase1InspectionScene()
    {
        SceneManager.LoadScene("Case1InspectionScene");
    }
}