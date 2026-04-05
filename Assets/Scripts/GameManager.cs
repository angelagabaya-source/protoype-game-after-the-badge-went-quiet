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

    [Header("Message System")]
    public UIMessageSwapper messageSwapper; 

    [Header("Game Settings")]
    public float timeRemaining = 60f;
    
    private bool gameEnded = false;
    private bool timerIsRunning = true;
    private int itemsFound = 0;

    void Awake()
    {
        itemTextUI.Clear();
        GameObject container = GameObject.Find("Item_List_Container");
        if (container != null)
        {
            itemTextUI.AddRange(container.GetComponentsInChildren<TextMeshProUGUI>());
        }

        if (messageSwapper == null) messageSwapper = FindFirstObjectByType<UIMessageSwapper>();
    }

    void Start()
    {
        Time.timeScale = 1f;
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

    public void TogglePause()
    {
        if (gameEnded) return;

        bool isOpening = !pausePanel.activeSelf;
        pausePanel.SetActive(isOpening);
        Time.timeScale = isOpening ? 0f : 1f;

        // --- UPDATED: Talk to the Persistent Music instead of forcing a number ---
        if (PersistentMusic.Instance != null)
        {
            PersistentMusic.Instance.UpdatePauseVolume(isOpening);
        }

        if (isOpening && messageSwapper != null)
        {
            messageSwapper.SetRandomPauseMessage();
        }
    }

    void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        timerIsRunning = false;

        // Tell music to stop or lower when game is lost
        if (PersistentMusic.Instance != null) PersistentMusic.Instance.audioSource.Stop();

        if (messageSwapper != null) messageSwapper.SetRandomGameOverMessage();

        TriggerEndPanel(gameOverPanel);
    }

    // ... [Rest of your existing methods: TriggerEndPanel, FadeIn, RestartGame, etc.]
    private void TriggerEndPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true);
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg != null) StartCoroutine(FadeIn(cg, 0.5f));
            else Time.timeScale = 0f;
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
        Time.timeScale = 0f; 
    }

    public void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if(timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SubtractTime(float amount) { if (!gameEnded) timeRemaining -= amount; }

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
            if (PersistentMusic.Instance != null) PersistentMusic.Instance.audioSource.Stop();
            TriggerEndPanel(winPanel);
        }
    }
}