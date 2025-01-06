using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager; 

    public SnakeBehavior snakeBehavior;
    public Score score;

    void OnTriggerEnter2D()
    {
        //gameManager.CompleteLevel(); for animations
        int segmentCount = snakeBehavior.getSegmentCount();
        float bonusScore = segmentCount * 1000;
        score.AddScore(bonusScore); // Call method to add bonus score
        Debug.Log("End Trigger activated. Bonus Score: " + bonusScore);
        Time.timeScale = 0;
        //gameManager.EndGame();
    }
}
