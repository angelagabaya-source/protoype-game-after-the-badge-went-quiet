using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Drag your PausePanel here
    private CanvasGroup canvasGroup;
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
        {
            canvasGroup = pausePanel.GetComponent<CanvasGroup>();
            pausePanel.SetActive(false); // Start hidden
        }
    }

    public void TogglePause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freezes the kitchen timer and physics
        StartCoroutine(FadeUI(0, 1));
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes the game
        StartCoroutine(FadeUI(1, 0, true)); // Fades out then deactivates
    }

    IEnumerator FadeUI(float start, float end, bool deactivateAtEnd = false)
    {
        float elapsed = 0;
        float duration = 0.3f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; 
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        if (deactivateAtEnd) pausePanel.SetActive(false);
    }
}