using UnityEngine;
using TMPro;

public class UIMessageSwapper : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI pauseMessageText;
    public TextMeshProUGUI gameOverMessageText;

    [Header("Pause Screen Messages")]
    public string[] pauseMessages = {
        "Placeholder",
        "Wife thinks we should've stayed in the city. And I said, in this economy?",
        "Would it be tampering with evidence if we ate the cookies?",
        "The house could use a bigger cabinet.",
	"Have we turned off the gas?"
    };

    [Header("Game Over Messages")]
    public string[] gameOverMessages = {
        "Lost you there for a sec, boss, let's get donuts?",
        "No no it's okay, back pain, right? ",
        "Family's in the station. Practically just one person, her kid, I think.",
        "Could use a drink anyway."
    };

    void Start()
    {
        // Safety: If you forgot to drag the objects in, this looks for them by name
        if (pauseMessageText == null) 
            pauseMessageText = GameObject.Find("PauseMessage")?.GetComponent<TextMeshProUGUI>();

        if (gameOverMessageText == null) 
            gameOverMessageText = GameObject.Find("GameOverMessage")?.GetComponent<TextMeshProUGUI>();
    }

    // Call this via the Pause Button's OnClick()
    public void SetRandomPauseMessage()
    {
        if (pauseMessageText != null && pauseMessages.Length > 0)
        {
            int index = Random.Range(0, pauseMessages.Length);
            pauseMessageText.text = pauseMessages[index];
            Debug.Log("New Pause Message: " + pauseMessages[index]);
        }
    }

    // Call this when the Game Over panel is activated
    public void SetRandomGameOverMessage()
    {
        if (gameOverMessageText != null && gameOverMessages.Length > 0)
        {
            int index = Random.Range(0, gameOverMessages.Length);
            gameOverMessageText.text = gameOverMessages[index];
            Debug.Log("New Game Over Message: " + gameOverMessages[index]);
        }
    }
}