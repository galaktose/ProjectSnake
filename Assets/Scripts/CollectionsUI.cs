using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class CollectionsUI : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject itemTemplatePrefab;
    public TextMeshProUGUI scoreText; 
    private const string SaveFileName = "special_items.json";

    private void Start()
    {
        LoadCollectedItems();
        if (scoreText != null)
        {
            scoreText.text = "High Score: " + Mathf.RoundToInt(GameData.highScore);
        }
    }

    private void LoadCollectedItems()
    {
        string path = Application.persistentDataPath + "/" + SaveFileName;
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        List<CollectedItemData> collectedItems = JsonUtility.FromJson<SaveData>(json).collectedItems;

        foreach (var itemData in collectedItems)
        {
            GameObject newItem = Instantiate(itemTemplatePrefab, contentPanel);
            
            // Assign text values
            newItem.transform.Find("itemName").GetComponent<TextMeshProUGUI>().text = itemData.itemName;
            newItem.transform.Find("itemCaption").GetComponent<TextMeshProUGUI>().text = itemData.caption;
            newItem.transform.Find("itemCollectionDate").GetComponent<TextMeshProUGUI>().text = "Collected on: " + itemData.collectionDate;

            // Assign rarity color
            TextMeshProUGUI rarityText = newItem.transform.Find("itemRarity").GetComponent<TextMeshProUGUI>();
            rarityText.text = itemData.rarity.ToString();
            rarityText.color = GetRarityColor(itemData.rarity);

            // Dynamically load the prefab by name and extract its sprite
            GameObject itemPrefab = Resources.Load<GameObject>($"SpecialItemPrefabs/{itemData.itemName}");
            if (itemPrefab != null)
            {
                SpriteRenderer spriteRenderer = itemPrefab.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Assign sprite
                    Image itemSprite = newItem.transform.Find("itemSprite").GetComponent<Image>();
                    itemSprite.sprite = spriteRenderer.sprite;
                    
                    // Assign color (if applicable)
                    itemSprite.color = spriteRenderer.color;
                }
            }
            else
            {
                Debug.LogWarning($"Prefab for {itemData.itemName} not found in Resources/SpecialItemPrefabs/");
            }

        }
    }

    private Color GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return Color.white;
            case ItemRarity.Uncommon: return Color.green;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return new Color(0.6f, 0, 0.8f); // Purple
            case ItemRarity.Legendary: return Color.yellow;
            default: return Color.black;
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public List<CollectedItemData> collectedItems;
    }
}
