using UnityEngine;
using TMPro; // Required for TextMeshPro

public class LifeSystem : MonoBehaviour
{
    public TextMeshProUGUI livesText; // TextMeshPro UI element to display lives
    public int maxLives = 3; // Maximum number of lives
    private int currentLives;

    void Start()
    {
        currentLives = 1; // Start with 1 extra life
        UpdateLivesUI();
    }

    // Adds an extra life (up to maxLives)
    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI();
        }
    }

    // Removes an extra life (used when colliding with walls)
    public void RemoveLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateLivesUI();
        }

        if (currentLives == 0)
        {
            Debug.Log("Game Over!");
            // Handle game over logic here
        }
    }

    // Updates the UI to reflect the current number of lives
    private void UpdateLivesUI()
    {
        livesText.text = $"{currentLives}";
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
