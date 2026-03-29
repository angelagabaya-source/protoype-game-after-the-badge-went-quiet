using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("UI Text References")]
    public TextMeshProUGUI timerText; 
    public List<TextMeshProUGUI> itemTextUI = new List<TextMeshProUGUI>();

    [Header("Game Settings")]
    public float timeRemaining = 60f;
    
    private bool gameEnded = false;
    private bool timerIsRunning = true;
    private int itemsFound = 0;

    void Awake()
    {
        // Auto-assign the item list from the container
        itemTextUI.Clear();
        GameObject container = GameObject.Find("Item_List_Container");
        if (container != null)
        {
            itemTextUI.AddRange(container.GetComponentsInChildren<TextMeshProUGUI>());
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        
        // Ensure all panels are hidden at the start
        if (winPanel) winPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning && !gameEnded)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                UpdateTimerDisplay(0);
                GameOver();
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if(timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SubtractTime(float amount)
    {
        if (gameEnded) return;
        timeRemaining -= amount;
        Debug.Log("<color=orange>Penalty Applied!</color>");
    }

    public void CrossOffItem(string itemName)
    {
        if (gameEnded) return;

        foreach (TextMeshProUGUI textElement in itemTextUI)
        {
            if (textElement.gameObject.name == itemName && textElement.color != Color.gray)
            {
                textElement.color = Color.gray;
                textElement.fontStyle = FontStyles.Strikethrough;
                itemsFound++;
                Debug.Log($"Found: {itemsFound}/{itemTextUI.Count}");
                CheckWinCondition();
                break;
            }
        }
    }

    void CheckWinCondition()
    {
        if (itemsFound >= itemTextUI.Count && !gameEnded)
        {
            gameEnded = true;
            timerIsRunning = false;
            TriggerEndPanel(winPanel);
        }
    }

    void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        timerIsRunning = false;
        TriggerEndPanel(gameOverPanel);
    }

    private void TriggerEndPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                StartCoroutine(FadeIn(cg, 1.5f));
            }
            else
            {
                // Fallback if no CanvasGroup is found
                Debug.LogWarning(panel.name + " is missing a Canvas Group component!");
            }
        }
    }

    IEnumerator FadeIn(CanvasGroup cg, float duration)
    {
        float time = 0;
        cg.alpha = 0;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime; 
            cg.alpha = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }
        cg.alpha = 1;
        // Optional: Freeze the game physics/logic after the fade
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}