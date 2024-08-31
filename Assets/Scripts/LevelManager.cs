using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float timeLimit = 30f; // Set time limit for the level
    private float timer;
    private bool gameOver = false; // Track game over state
    private bool isPaused = false; // Track pause state

    public PlayerInventory playerInventory;
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public ThoughtBubbleController thoughtBubbleController;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inventoryText;

    private int currentLevel;

    void Start()
    {
        currentLevel = int.Parse(SceneManager.GetActiveScene().name.Replace("L", ""));

        timer = timeLimit;
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Ensure the game is not paused at the start

        Debug.Log("Game started. Current Level: L" + currentLevel);
        DisplayObjective();
    }

    void Update()
    {
        if (gameOver || isPaused) return;

        timer -= Time.deltaTime;
        UpdateUI();

        if (timer <= 0)
        {
            timer = 0;
            Debug.Log("Time's up! Checking level completion condition.");
            CheckCondition(); // Check if the player meets the conditions to advance
        }
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        inventoryText.text = "Inventory:\n" + string.Join("\n", playerInventory.GetInventoryItems());
    }

    void DisplayObjective()
    {
        switch (currentLevel)
        {
            case 1:
                thoughtBubbleController.ShowThought("I'm hungry");
                Debug.Log("Level 1 Objective set: Collect 10 fruits.");
                break;
            case 2:
                thoughtBubbleController.ShowThought("I need fire.");
                Debug.Log("Level 2 Objective set: Collect 2 rocks and 1 wood.");
                break;
            case 3:
                thoughtBubbleController.ShowThought("wild animal!");
                Debug.Log("Level 3 Objective set: Build an arch with rock, wood, and vine.");
                break;
            case 4:
                thoughtBubbleController.ShowThought("Let's finish this!");
                Debug.Log("Level 4 Objective set: Final task.");
                break;
        }
    }

    void CheckCondition()
    {
        Debug.Log("Checking condition for Level L" + currentLevel);
        Debug.Log("Current Inventory: " + string.Join(", ", playerInventory.GetInventoryItems()));

        bool conditionMet = false;

        switch (currentLevel)
        {
            case 1:
                if (playerInventory.HasItem("Fruit", 10))
                {
                    Debug.Log("Level 1 completed. 10 fruits collected.");
                    conditionMet = true;
                }
                break;
            case 2:
                if (playerInventory.HasItem("Rock", 2) && playerInventory.HasItem("Wood", 1))
                {
                    Debug.Log("Level 2 completed. 2 rocks and 1 wood collected.");
                    conditionMet = true;
                }
                break;
            case 3:
                // Check if the player has crafted the Arch and defeated the Dinosaur
                if (playerInventory.HasItem("Vine") && playerInventory.HasItem("Rock") && playerInventory.HasItem("Wood"))
                {
                    Debug.Log("Level 3 completed. Arch crafted and Dinosaur defeated.");
                    conditionMet = true;
                }
                break;
            case 4:
                // check if dinosaur is destroyed
                if (GameObject.Find("Dinosaur") == null)
                {
                    Debug.Log("Level 4 completed. Dinosaur destroyed.");
                    conditionMet = true;
                }
                break;
        }

        if (conditionMet)
        {
            LoadNextLevel();
        }
        else
        {
            GameOver();
        }
    }

    void LoadNextLevel()
    {
        currentLevel++;
        Debug.Log("Loading next level. New Level: L" + currentLevel);

        if (currentLevel > 4)
        {
            Debug.Log("All levels completed. Loading Victory Scene.");
            SceneManager.LoadScene("VictoryScene");
        }
        else
        {
            string nextLevelName = "L" + currentLevel;
            Debug.Log("Loading scene: " + nextLevelName);
            SceneManager.LoadScene(nextLevelName);
        }
    }

    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;

        Debug.Log("GameOver triggered. Freezing time and showing GameOver menu.");

        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }

    public void PauseGame()
    {
        if (gameOver) return;
        isPaused = true;

        Debug.Log("Game paused. Freezing time and showing Pause menu.");

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        Debug.Log("Game resumed. Resuming time and hiding Pause menu.");

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        gameOver = false;
        isPaused = false;
        Time.timeScale = 1f;

        Debug.Log("Level restarted. Reloading current level.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Debug.Log("Returning to Main Menu.");
        SceneManager.LoadScene("MainMenuScene");
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
