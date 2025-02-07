using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject pathPrefab;
    public int mazeWidth = 11; // Must be odd
    public int mazeHeight = 11; // Must be odd
    public int pathWidth = 3; // Size of paths (e.g., 3x3 for wide paths)
    public Vector2 mazeOffset = new Vector2(0, 0);

    public GameObject[] powerUpPrefabs;  // Array to store power-up prefabs
    public SpecialItem[] specialItems;  // Array to store special item prefabs
    public float specialItemSpawnChance = 0.1f;  // Chance of spawning a special item (10%)
    public int maxPowerUps = 5; // Maximum number of power-ups to spawn

    private int[,] maze;
    public Vector2 snakeSpawnPosition; // Store snake spawn position

    private List<Vector3> validSpawnPositions = new List<Vector3>(); // List to store valid spawn positions
    private HashSet<string> spawnedSpecialItems = new HashSet<string>(); // To track which special items have been spawned

    private void Start()
    {
        if (mazeWidth % 2 == 0) mazeWidth++;
        if (mazeHeight % 2 == 0) mazeHeight++;

        maze = new int[mazeWidth, mazeHeight];

        // Fill maze with walls
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = 1; // Default to walls
            }
        }
        
        // Generate maze with wider paths
        GenerateMaze(1, 1);

        // Enforce outer walls
        EnforceOuterWalls();

        // Find a valid spawn point
        FindSnakeSpawnPoint();

        // Draw maze and collect valid spawn positions
        DrawMaze();
    }

    private void GenerateMaze(int x, int y)
    {
        // Carve out a wider path
        CarvePath(x, y);

        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        Shuffle(directions);

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x * (pathWidth + 1);
            int newY = y + direction.y * (pathWidth + 1);

            if (IsInBounds(newX, newY) && maze[newX, newY] == 1)
            {
                // Create a wide path leading to newX, newY
                CarvePath(x + direction.x, y + direction.y);
                CarvePath(newX, newY);
                GenerateMaze(newX, newY);
            }
        }
    }

    private void CarvePath(int x, int y)
    {
        // Carve a path of pathWidth x pathWidth
        for (int dx = 0; dx < pathWidth; dx++)
        {
            for (int dy = 0; dy < pathWidth; dy++)
            {
                int px = x + dx;
                int py = y + dy;
                if (IsInBounds(px, py))
                {
                    maze[px, py] = 0; // Mark as path
                }
            }
        }
    }

    private void EnforceOuterWalls()
    {
        for (int y = 0; y < mazeHeight; y++)
        {
            maze[0, y] = 1;
            maze[mazeWidth - 1, y] = 1;
        }

        for (int x = 0; x < mazeWidth; x++)
        {
            maze[x, 0] = 1;
            maze[x, mazeHeight - 1] = 1;
        }
    }

    private void FindSnakeSpawnPoint()
    {
        List<Vector2> validSpawnPoints = new List<Vector2>();

        for (int x = 1; x < mazeWidth - 1; x++)
        {
            for (int y = 1; y < mazeHeight - 1; y++)
            {
                if (maze[x, y] == 0) // Path tile
                {
                    Vector3 position = new Vector3(x + mazeOffset.x, y + mazeOffset.y, 0);
                    validSpawnPoints.Add(position);
                }
            }
        }

        // Pick a random spawn point from valid locations
        if (validSpawnPoints.Count > 0)
        {
            snakeSpawnPosition = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
        }
        else
        {
            Debug.LogWarning("No valid spawn point found!");
        }
    }

    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2Int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x > 0 && x < mazeWidth - 1 && y > 0 && y < mazeHeight - 1;
    }

    private void DrawMaze()
    {
        Vector3 startPosition = new Vector3(mazeOffset.x, mazeOffset.y, 0);

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3 position = startPosition + new Vector3(x, y, 0);

                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else
                {
                    Instantiate(pathPrefab, position, Quaternion.identity);

                    // Collect valid positions for spawning power-ups and special items
                    validSpawnPositions.Add(position);
                }
            }
        }

        // Now spawn power-ups and special items at valid positions
        SpawnItems();
    }

    private void SpawnItems()
    {
        // Spawn power-ups at random positions
        if (validSpawnPositions.Count > 0)
        {
            int powerUpSpawned = 0;

            // Loop to spawn power-ups
            while (powerUpSpawned < maxPowerUps && validSpawnPositions.Count > 0)
            {
                // Pick a random position from the valid spawn list
                int randomIndex = Random.Range(0, validSpawnPositions.Count);
                Vector3 spawnPosition = validSpawnPositions[randomIndex];

                // Remove the chosen spawn position from the valid list (to avoid overlap)
                validSpawnPositions.RemoveAt(randomIndex);

                // Spawn a power-up at the selected position
                SpawnPowerUp(spawnPosition);
                powerUpSpawned++;
            }

            // Spawn special items at random positions
            foreach (var specialItem in specialItems)
            {
                if (Random.value <= specialItemSpawnChance)
                {
                    // Pick a random position from the valid spawn list for special items
                    int randomIndex = Random.Range(0, validSpawnPositions.Count);
                    Vector3 spawnPosition = validSpawnPositions[randomIndex];

                    // Remove the chosen spawn position from the valid list
                    validSpawnPositions.RemoveAt(randomIndex);

                    // Spawn the special item
                    SpawnSpecialItem(spawnPosition);
                }
            }
        }
    }

    private void SpawnPowerUp(Vector3 position)
    {
        // Randomly pick a power-up from the array
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomIndex], position, Quaternion.identity);
    }

    private void SpawnSpecialItem(Vector3 position)
    {
        // Randomly pick a special item and ensure it doesn't spawn more than once
        SpecialItem selectedItem = SelectSpecialItemBasedOnRarity();
        if (selectedItem != null && !spawnedSpecialItems.Contains(selectedItem.itemName))
        {
            Instantiate(selectedItem.prefab, position, Quaternion.identity);
            spawnedSpecialItems.Add(selectedItem.itemName); // Track the spawned special item
        }
    }

    private SpecialItem SelectSpecialItemBasedOnRarity()
    {
        List<SpecialItem> eligibleItems = new List<SpecialItem>();

        // Loop through special items and add them to the eligible list based on rarity
        foreach (SpecialItem item in specialItems)
        {
            if (Random.value <= GetRaritySpawnChance(item.rarity))
            {
                eligibleItems.Add(item);
            }
        }

        // If we have eligible items, return a random one
        if (eligibleItems.Count > 0)
        {
            return eligibleItems[Random.Range(0, eligibleItems.Count)];
        }

        return null;
    }

    private float GetRaritySpawnChance(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return 0.5f;
            case ItemRarity.Uncommon: return 0.3f;
            case ItemRarity.Rare: return 0.15f;
            case ItemRarity.Epic: return 0.05f;
            case ItemRarity.Legendary: return 0.01f;
            default: return 0.0f;
        }
    }
}
