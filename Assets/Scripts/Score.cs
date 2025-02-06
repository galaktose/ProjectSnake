using UnityEngine;
using TMPro;
using static UnityEngine.Mathf;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    

    float score = 0;

    public bool isEndTriggered = false;
    private float timer = 0;

    // Add a new flag to track countdown status
    public bool isCountdownFinished = false;

    void Update()
    {
        // Only update score if countdown is finished
        if (isCountdownFinished)
        {
            timer += Time.deltaTime;
            if (timer >= 1f && isEndTriggered == false)
            {
                score += 10;
                scoreText.text = RoundToInt(score).ToString();
                timer = 0;
            }
        }
    }

    public void AddScore(float bonus)
    {
        score += bonus;
        scoreText.text = RoundToInt(score).ToString();
    }

    public float GetScore() 
    {
        return score;
    }

    // This method should be called once the countdown ends
    public void SetCountdownFinished()
    {
        isCountdownFinished = true;
    }
}
