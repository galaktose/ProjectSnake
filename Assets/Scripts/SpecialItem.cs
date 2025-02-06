using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    public string itemName; // Name of the item
    public string caption; // Caption to display when collected
    private UIManager uiManager; // Reference to the UIManager to update the caption text

    private void Start()
    {
        // Dynamically find the UIManager in the scene
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            // Call the UIManager to update the caption
            uiManager.DisplaySpecialItemCaption(itemName, caption);

            // Destroy the special item after collection
            Destroy(gameObject);
        }
    }
}
