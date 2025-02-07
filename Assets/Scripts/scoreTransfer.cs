using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class scoreTransfer : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI collectStarsText;
    public TextMeshProUGUI highScoreText; // Add this reference to show high score

    void Start()
    {
        float finalScore = Mathf.RoundToInt(GameData.score);

        // Display final score
        finalScoreText.text = finalScore.ToString();

        // Compare and update high score
        GameData.SaveHighScore(finalScore);

        // Display the updated high score
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + Mathf.RoundToInt(GameData.highScore).ToString();
        }

        // Handle star collection text
        if (SceneManager.GetActiveScene().name == "Win Screen")
        {
            if (GameData.stars < 3)
            {
                collectStarsText.text = "You collected " + GameData.stars + " stars.";
            }
            else
            {
                collectStarsText.text = "You collected all " + GameData.stars + " stars!";
            }
        }
    }
}
