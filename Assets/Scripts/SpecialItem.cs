using UnityEngine;

[System.Serializable]
public class SpecialItem : MonoBehaviour
{
    public string itemName;  // Name of the item
    public string caption;   // Caption to display when collected
    public GameObject prefab;  // The prefab for the special item
    public ItemRarity rarity;  // The rarity of the special item
    private UIManager uiManager; // Reference to the UIManager to update the caption text

    private void Start()
    {
        // Check if the item has already been collected
        if (PlayerPrefs.GetInt(itemName + "_Collected", 0) == 1)
        {
            Destroy(gameObject); // Destroy the item if it has been collected
        }

        // Dynamically find the UIManager in the scene
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark item as collected in PlayerPrefs
            PlayerPrefs.SetInt(itemName + "_Collected", 1);
            PlayerPrefs.Save();  // Ensure the data is saved

            // Call the UIManager to update the caption
            uiManager.DisplaySpecialItemCaption(itemName, caption);

            // Destroy the special item after collection
            Destroy(gameObject);
        }
    }
}
