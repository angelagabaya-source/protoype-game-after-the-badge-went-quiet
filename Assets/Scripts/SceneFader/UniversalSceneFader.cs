using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UniversalSceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Ensure the image starts fully visible before fading in
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;
        
        StartCoroutine(FadeIn());
    }

    // Dynamic function: pass the scene name from the Button component!
    public void TriggerSceneFade(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        // Turn off Raycast Target so we can click buttons underneath
        fadeImage.raycastTarget = false;
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Turn on Raycast Target so the player can't click things during the fade
        fadeImage.raycastTarget = true;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}