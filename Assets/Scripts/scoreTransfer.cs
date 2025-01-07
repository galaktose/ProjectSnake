using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class scoreTransfer : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI collectStarsText;
    void Start()
    {
        
        finalScoreText.text = Mathf.RoundToInt(GameData.score).ToString();

        if (SceneManager.GetActiveScene().name == "Win Screen")
        {
            if (GameData.stars < 3)
                {
                    collectStarsText.text = "You collected " + GameData.stars + " stars.";
                }
            if (GameData.stars == 3)
                {
                    collectStarsText.text = "You collected all " + GameData.stars + " stars!";
                }
        }
        
    }

}
