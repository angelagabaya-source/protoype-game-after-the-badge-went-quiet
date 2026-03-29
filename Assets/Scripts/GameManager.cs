using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameObject winPanel;
    public GameObject pausePanel;
    public List<TextMeshProUGUI> itemTextUI;

    [Header("Settings")]
    public float timeRemaining = 60f;
    
    private bool gameEnded = false;
    private bool timerIsRunning = true;
    private int itemsFound = 0;

    void Start()
    {
        Time.timeScale = 1f;
        if (winPanel) winPanel.SetActive(false);
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
                timerIsRunning = false;
                Debug.Log("Game Over!");
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
    }

    public void CrossOffItem(string itemName)
    {
        foreach (TextMeshProUGUI textElement in itemTextUI)
        {
            if (textElement.gameObject.name == itemName)
            {
                textElement.color = Color.gray;
                textElement.fontStyle = FontStyles.Strikethrough;
                itemsFound++;
                CheckWinCondition();
            }
        }
    }

    void CheckWinCondition()
    {
        if (itemsFound >= itemTextUI.Count && !gameEnded)
        {
            gameEnded = true;
            timerIsRunning = false;
            if(winPanel) winPanel.SetActive(true);
        }
    }
}