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
        SceneManager.LoadScene(finalScore.lastScene);
        finalScore.lastScene = null;

    }

    public void returnMain() 
    {
        SceneManager.LoadScene("Main Menu");
        
    }
}

