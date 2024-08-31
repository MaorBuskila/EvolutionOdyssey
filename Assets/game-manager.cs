using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public float timeLimit = 20f;
    private float timer;
    private bool timerEnded = false;
    private bool gameOver = false;
    private bool isPaused = false;

    [Header("Player Inventory")]
    public PlayerInventory playerInventory;
    public Sprite archerSprite; // Add this for the archer appearance

    [Header("UI Elements")]
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public ThoughtBubbleController thoughtBubbleController;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI objectiveText;

    public Button pauseResumeButton;
    public Button pauseRestartButton;
    public Button pauseExitButton;
    public Button gameOverRestartButton;
    public Button gameOverExitButton;

    private string currentObjective;
    private List<string> requiredItems = new List<string>();
    private int requiredItemCount = 0;
    private int currentItemIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        timer = timeLimit;
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);

        SetupButtonListeners();
        SetLevelObjectives();

        if (thoughtBubbleController != null)
        {
            thoughtBubbleController.ShowThought("I'm cold");
        }

        // Subscribe to inventory changes
        playerInventory.OnInventoryChanged += HandleInventoryChange;
    }

    private void Update()
    {
        if (gameOver || timerEnded || isPaused) return;

        timer -= Time.deltaTime;
        UpdateUI();

        if (timer <= 0)
        {
            timerEnded = true;
            CheckCondition();
        }
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (inventoryText != null && playerInventory != null)
            inventoryText.text = "Inventory:\n" + string.Join("\n", playerInventory.GetInventoryItems());

        if (objectiveText != null)
            objectiveText.text = "Objective: " + currentObjective;
    }

    private void SetupButtonListeners()
    {
        // Pause Menu Buttons
        if (pauseResumeButton != null)
            pauseResumeButton.onClick.AddListener(ResumeGame);
        if (pauseRestartButton != null)
            pauseRestartButton.onClick.AddListener(RestartLevel);
        if (pauseExitButton != null)
            pauseExitButton.onClick.AddListener(ReturnToMainMenu);

        // Game Over Menu Buttons
        if (gameOverRestartButton != null)
            gameOverRestartButton.onClick.AddListener(RestartLevel);
        if (gameOverExitButton != null)
            gameOverExitButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void SetLevelObjectives()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "L1")
        {
            currentObjective = "Gather 2 Rocks and 1 Wood";
            requiredItems = new List<string> { "Rock", "Rock", "Wood" };
            requiredItemCount = requiredItems.Count;
        }
        else if (currentScene == "L2")
        {
            currentObjective = "Gather 1 Wood and 1 Zebra in Order";
            requiredItems = new List<string> { "Wood", "Zebra" };
            requiredItemCount = requiredItems.Count;
        }
        else if (currentScene == "L3")
        {
            currentObjective = "Build an Arch: 2 Wood, 1 Rock, 1 Vine";
            requiredItems = new List<string> { "Wood", "Wood", "Rock", "Vine" };
            requiredItemCount = requiredItems.Count;
        }
    }

    private void HandleInventoryChange(string item, int count)
    {
        if (item == "Arch" && count > 0)
        {
            Debug.Log("Player crafted the Arch, changing appearance to Archer.");
            GameObject player = playerInventory.gameObject;
            player.GetComponent<SpriteRenderer>().sprite = archerSprite;
        }
    }

    public void OnItemCollected(string item)
    {
        if (gameOver) return;

        if (requiredItems[currentItemIndex] == item)
        {
            currentItemIndex++;
            playerInventory.AddToInventory(item);
            UpdateUI();

            if (currentItemIndex >= requiredItemCount)
            {
                LoadNextLevel();
            }
        }
        else
        {
            Debug.Log("Collected the wrong item or out of order!");
            // Optional: Add feedback for collecting the wrong item
        }
    }

    private void CheckCondition()
    {
        if (currentItemIndex >= requiredItemCount)
        {
            LoadNextLevel();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (gameOver) return;
        gameOver = true;

        Time.timeScale = 0;
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;

        Time.timeScale = 0f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;

        Time.timeScale = 1f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        gameOver = false;
        timerEnded = false;
        isPaused = false;
        currentItemIndex = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        string nextScene = SceneManager.GetActiveScene().name == "L1" ? "L2" :
                           SceneManager.GetActiveScene().name == "L2" ? "L3" : "MainMenuScene";
        SceneManager.LoadScene(nextScene);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
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
