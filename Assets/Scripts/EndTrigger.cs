using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager; 
    public SnakeBehavior snakeBehavior;
    public Score score;


    void OnTriggerEnter2D()
    {
        //gameManager.CompleteLevel(); for animations
        int segmentCount = snakeBehavior.getSegmentCount() - 3;
        float bonusScore = segmentCount * 500;
        score.AddScore(bonusScore); // each snake segment = +500 score
        Debug.Log("End Trigger activated. Bonus Score: " + bonusScore);
        GameData.score = score.GetScore();
        Debug.Log("Stars: " + GameData.stars);
        score.isEndTriggered = true;
        Invoke("LoadScene", 1.5f);
        //gameManager.EndGame();
    }

    void LoadScene() 
    {
        SceneManager.LoadScene("Win Screen");
        //GameData.stars = 0;
    }
}
