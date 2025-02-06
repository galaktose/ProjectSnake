using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;
    private MazeGenerator mazeGenerator;

    void Start()
    {
        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
        Vector3 spawnPosition = mazeGenerator.snakeSpawnPosition;
        Instantiate(snakePrefab, spawnPosition, Quaternion.identity);
    }
}

