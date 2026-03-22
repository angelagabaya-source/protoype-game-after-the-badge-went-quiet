using UnityEngine;

public class QuitManager : MonoBehaviour
{
    // This function must be 'public' to show up in the Button settings
    public void ExitGame()
    {
        // Logs a message to the console so you know it's working in the editor
        Debug.Log("Quit button pressed!");

        // This line closes the actual built game
        Application.Quit();

        // This line stops the play mode if you're inside the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}