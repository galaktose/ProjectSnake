using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public static class SaveSystem
{
    private static string saveKey = "CollectedItemDatas";
    private static string filePath = Path.Combine(Application.persistentDataPath, "special_items.json");

    // Save a new collected item
    public static void SaveCollectedItem(CollectedItemData newItem)
    {
        List<CollectedItemData> collectedItems = LoadCollectedItems();

        // Prevent duplicates
        if (!collectedItems.Any(item => item.itemName == newItem.itemName))
        {
            collectedItems.Add(newItem);
            string json = JsonUtility.ToJson(new SaveData(collectedItems), true); // Pretty JSON formatting

            // Save to PlayerPrefs
            PlayerPrefs.SetString(saveKey, json);
            PlayerPrefs.Save();

            // Save to a physical file
            File.WriteAllText(filePath, json);
            Debug.Log("JSON saved at: " + filePath);
        }
    }

    // Load all collected items
    public static List<CollectedItemData> LoadCollectedItems()
    {
        if (File.Exists(filePath))  // Check if the JSON file exists
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.collectedItems;
        }
        else if (PlayerPrefs.HasKey(saveKey)) // Fallback to PlayerPrefs
        {
            string json = PlayerPrefs.GetString(saveKey);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.collectedItems;
        }
        return new List<CollectedItemData>();
    }

    // Delete all saved items (for testing)
    public static void ClearCollectedItems()
    {
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Deleted saved file: " + filePath);
        }
    }
}

// Wrapper class for saving lists
[System.Serializable]
public class SaveData
{
    public List<CollectedItemData> collectedItems;

    public SaveData(List<CollectedItemData> items)
    {
        this.collectedItems = items;
    }
}
