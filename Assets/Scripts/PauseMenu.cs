using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu at start
    }

    public void TogglePause()
    {
        Debug.Log("Toggle Pause");
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1; // Pause/unpause the game
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Ensure time is running before switching scenes
        SceneManager.LoadScene("Main Menu"); // Change to your actual main menu scene name
    }
}
