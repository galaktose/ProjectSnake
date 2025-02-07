using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CollectedItemData
{
    public string itemName;
    public string caption;
    public string collectionDate;
    public ItemRarity rarity;

    public CollectedItemData(string name, string caption, string date, ItemRarity rarity)
    {
        this.itemName = name;
        this.caption = caption;
        this.collectionDate = date;
        this.rarity = rarity;
    }
}

[System.Serializable]
public class SpecialItem : MonoBehaviour
{
    public string itemName; 
    public string caption;
    public GameObject prefab;
    public ItemRarity rarity;

    private UIManager uiManager;

    private void Start()
    {
        // Load collected items
        List<CollectedItemData> collectedItems = SaveSystem.LoadCollectedItems();

        // Check if this item has already been collected
        foreach (var item in collectedItems)
        {
            if (item.itemName == itemName)
            {
                Destroy(gameObject); // Destroy the item if it has been collected
                return;
            }
        }

        // Find UI Manager
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string collectionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Create collected item instance
            CollectedItemData collectedItem = new CollectedItemData(itemName, caption, collectionDate, rarity);

            // Save item using JSON system
            SaveSystem.SaveCollectedItem(collectedItem);

            // Update UI with caption
            uiManager.DisplaySpecialItemCaption(itemName, caption);

            // Destroy the item after collection
            Destroy(gameObject);
        }
    }
}
