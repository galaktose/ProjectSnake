using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI captionText; // Reference to the TextMeshProUGUI component in the UI

    public void DisplaySpecialItemCaption(string itemName, string caption)
    {
        captionText.text = $"{itemName} \n {caption}";
        StartCoroutine(HideCaptionAfterDelay());
    }

    private IEnumerator HideCaptionAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        captionText.text = ""; // Clear the caption
    }
}

