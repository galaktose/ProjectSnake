using UnityEngine;
using TMPro;
using static UnityEngine.Mathf;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    

    float score = 0;
    public bool isEndTriggered = false;
    private float timer = 0;

    public bool isCountdownFinished = false;

    void Update()
    {
        if (isCountdownFinished)
        {
            timer += Time.deltaTime;
            if (timer >= 1f && !isEndTriggered)
            {
                score += 10;
                scoreText.text = Mathf.RoundToInt(score).ToString();
                timer = 0;
            }
        }
    }

    public void AddScore(float bonus)
    {
        score += bonus;
        scoreText.text = Mathf.RoundToInt(score).ToString();
    }

    public float GetScore() 
    {
        return score;
    }

    public void SetCountdownFinished()
    {
        isCountdownFinished = true;
    }

    // Call this when the game ends to update the high score
    public void CheckAndSaveHighScore()
    {
        GameData.SaveHighScore(score);
    }
}
