using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public List<PowerUp> powerUps;  // List of all available power-ups
    public List<SpecialItem> specialItems;  // List of all available special items
    public Transform maze;  // Reference to your maze
    public int numberOfItemsToSpawn = 10;  // Number of items to spawn in the game

    private void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        // Get all the valid path tiles in the maze (You should already have a way to track these)
        List<Vector3> validTiles = GetValidPathTiles();

        // Spawn powerups
        foreach (PowerUp powerUp in powerUps)
        {
            SpawnItemOnRandomTile(powerUp.prefab, powerUp.rarity, validTiles);
        }

        // Spawn special items
        foreach (SpecialItem specialItem in specialItems)
    // Spawn each power-up on a random valid tile based on its rarity
    foreach (PowerUp powerUp in powerUps)
    {
        SpawnItemOnRandomTile(powerUp.prefab, powerUp.rarity, validTiles);
        {
            SpawnItemOnRandomTile(specialItem.prefab, specialItem.rarity, validTiles);
        }
    }
    }

    private void SpawnItemOnRandomTile(GameObject itemPrefab, ItemRarity rarity, List<Vector3> validTiles)
    {
        // Calculate spawn chance based on rarity
        float spawnChance = GetSpawnChanceForRarity(rarity);

        // Randomly choose a tile from the valid ones
        Vector3 spawnPosition = validTiles[Random.Range(0, validTiles.Count)];

        // Spawn each special item on a random valid tile based on its rarity
        foreach (SpecialItem specialItem in specialItems)
        {
            SpawnItemOnRandomTile(specialItem.prefab, specialItem.rarity, validTiles);
        }
    

        // Roll to see if we spawn the item (based on its rarity)
        if (Random.Range(0f, 1f) <= spawnChance)
        {
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private float GetSpawnChanceForRarity(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common:
                return 0.6f;  // 60% chance for common items
            case ItemRarity.Uncommon:
                return 0.25f;  // 25% chance for uncommon items
            case ItemRarity.Rare:
                return 0.1f;  // 10% chance for rare items
            case ItemRarity.SuperRare:
                return 0.04f;  // 4% chance for super rare items
            case ItemRarity.ExtremelyRare:
                return 0.01f;  // 1% chance for extremely rare items
            default:
                return 0f;  // Default: no chance to spawn
        }
    }

    // This is where you would get your valid path tiles, modify as needed based on your maze generation logic
    private List<Vector3> GetValidPathTiles()
    {
        List<Vector3> validTiles = new List<Vector3>();

        // Example: Iterate through maze and add valid path tiles
        foreach (Transform child in maze)
        {
            if (child.CompareTag("Path"))  // Assume path tiles are tagged with "Path"
            {
                validTiles.Add(child.position);
            }
        }

        return validTiles;
    }
}
