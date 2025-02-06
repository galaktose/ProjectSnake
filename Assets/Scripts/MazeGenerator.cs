using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 30;   // Width of the maze
    public int height = 30;  // Height of the maze
    public GameObject wallPrefab;  // Wall prefab for visual representation
    public GameObject pathPrefab;  // Path prefab for visual representation
    public GameObject specialItemPrefab;  // Special Item prefab to spawn in the maze

    private Cell[,] grid;

    void Start()
    {
        grid = new Cell[width, height];
        GenerateMaze();
    }

    // Generate the maze using recursive backtracking algorithm
    void GenerateMaze()
    {
        // Initialize the grid (fill it with walls)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell(x, y);
                Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }

        // Pick a random starting point (always start at (1, 1))
        CarvePath(1, 1);

        // Spawn special items randomly along the open paths
        SpawnSpecialItems();
    }

    // Recursive backtracking to carve paths through the maze
    void CarvePath(int x, int y)
    {
        grid[x, y].isVisited = true;

        // Directions: Up, Down, Left, Right
        int[] directions = new int[] { 0, 1, 2, 3 };  // 0: up, 1: down, 2: left, 3: right
        ShuffleArray(directions);

        foreach (int direction in directions)
        {
            int nx = x, ny = y;

            // Move in the chosen direction
            switch (direction)
            {
                case 0: ny -= 1; break;  // Up
                case 1: ny += 1; break;  // Down
                case 2: nx -= 1; break;  // Left
                case 3: nx += 1; break;  // Right
            }

            // Check if the new position is valid and not visited yet
            if (IsValid(nx, ny))
            {
                grid[nx, ny].isVisited = true;
                Instantiate(pathPrefab, new Vector3(nx, 0, ny), Quaternion.identity);  // Instantiate path prefab
                CarvePath(nx, ny);
            }
        }
    }

    // Check if a position is within bounds and unvisited
    bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height && !grid[x, y].isVisited;
    }

    // Helper function to shuffle directions
    void ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    // Spawn special items randomly in open paths
    void SpawnSpecialItems()
    {
        int itemCount = Random.Range(5, 15);  // Number of items to spawn

        for (int i = 0; i < itemCount; i++)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);

            // Only spawn items on open paths (not walls)
            if (grid[x, y].isVisited)
            {
                Instantiate(specialItemPrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
}

// Cell class to store information about each grid cell
public class Cell
{
    public int x, y;
    public bool isVisited;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.isVisited = false;
    }
}
