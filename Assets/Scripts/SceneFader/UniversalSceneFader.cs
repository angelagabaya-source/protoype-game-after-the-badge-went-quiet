using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UniversalSceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void TriggerSceneFade(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime; // Works while paused
            fadeImage.color = new Color(0,0,0, Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }
        fadeImage.raycastTarget = false;
    }

    IEnumerator FadeOut(string sceneName)
    {
        fadeImage.raycastTarget = true;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // Works while paused
            fadeImage.color = new Color(0,0,0, Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }
        Time.timeScale = 1f; // Safety reset
        SceneManager.LoadScene(sceneName);
    }
}