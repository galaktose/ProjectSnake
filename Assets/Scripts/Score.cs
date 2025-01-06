using UnityEngine;
using TMPro;
using static UnityEngine.Mathf;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    

    float score = 0;

    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            score += 100;
            scoreText.text = RoundToInt(score).ToString();
            timer = 0;
        }
    }

    public void AddScore(float bonus)
    {
        score += bonus;
        scoreText.text = RoundToInt(score).ToString();
    }


    
}
