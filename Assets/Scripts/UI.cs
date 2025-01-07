using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1"); 
    }

    
    public void QuitGame()
    {
        //Debug.Log("Quit Game"); 
        Application.Quit();      
    }

    public void Restart()
    {
        SceneManager.LoadScene(GameData.lastScene);
        GameData.lastScene = null;

    }

    public void returnMain() 
    {
        SceneManager.LoadScene("Main Menu");
        GameData.stars = 0;
        
    }
}

