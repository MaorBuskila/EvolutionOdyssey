using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    
    // This method pauses the game by showing the pause menu and freezing time
    public void Pause()
    {
        Debug.Log("PauseGame called");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
    }

    // This method resumes the game by hiding the pause menu and resuming time
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    // This method restarts the game by reloading the current scene
    public void RestartGame()
    {
        Debug.Log("RestartGame called");
        Time.timeScale = 1f; // Make sure time is running again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        
        // Optionally, reset the menus to ensure they are hidden when the scene reloads
        pauseMenu.SetActive(false);
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }
    }

    // This method loads the home screen or main menu, defined by the sceneID parameter
    public void Home(int sceneID)
    {
        Time.timeScale = 1f; // Make sure time is running again
        SceneManager.LoadScene(sceneID); // Load the specified scene
    }
}
