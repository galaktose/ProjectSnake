using UnityEngine;

public static class GameData
{
    public static float score = 0f; 
    public static string lastScene;
    public static int stars = 0;

    // High score variable
    public static float highScore = 0f;

    // Load high score from PlayerPrefs
    static GameData()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
    }

    // Save high score
    public static void SaveHighScore(float newScore)
    {
        if (newScore > highScore)
        {
            highScore = newScore;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }
}
