using UnityEngine;
using TMPro;

public class scoreTransfer : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    void Start()
    {
        finalScoreText.text = Mathf.RoundToInt(finalScore.score).ToString();
    }

}
