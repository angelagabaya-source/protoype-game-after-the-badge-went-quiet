using UnityEngine;
using TMPro; // Needed for the UI Text

public class GameManager : MonoBehaviour
{
    public float timeRemaining = 60f; // Start with 60 seconds
    public bool timerIsRunning = true;
    public TextMeshProUGUI timerText; // We will link this in a second

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                // Trigger Game Over logic here
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // This is the function our ObjectFinder will call!
    public void SubtractTime(float amount)
    {
        timeRemaining -= amount;
        Debug.Log("Penalty! -" + amount + " seconds.");
    }
}