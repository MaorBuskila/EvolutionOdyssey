using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public float timeLimit = 20f; // 3 minutes for Level 2
    private float timer;
    private bool timerEnded = false;

    public PlayerInventory playerInventory;
    public GameObject gameOverMenu;
    public ThoughtBubbleController thoughtBubbleController;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI objectiveText;

    private string currentObjective = "Gather materials for a fire";

    void Start()
    {
        timer = timeLimit;
        gameOverMenu.SetActive(false);

        // Show the initial thought bubble
        thoughtBubbleController.ShowThought("I'm cold");
    }

    void Update()
    {
        if (timerEnded) return;

        timer -= Time.deltaTime;
        UpdateUI();

        if (timer <= 0)
        {
            timerEnded = true;
            CheckCondition();
        }
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        inventoryText.text = "Inventory:\n" + string.Join("\n", playerInventory.GetInventoryItems());
        // objectiveText.text = "Objective: " + currentObjective;
    }

    void CheckCondition()
    {
        // Check if the player has gathered all necessary materials for a shelter
        Debug.Log(playerInventory.ToString());
        if (playerInventory.HasItem("Wood", 1) && playerInventory.HasItem("Rock", 1))
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
        SceneManager.LoadScene("L2"); // Assuming there's a Level 2
    }

    void GameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
            Debug.Log("GameOverMenu activated");
        }
        else
        {
            Debug.LogError("GameOverMenu reference is not set in the Inspector");
        }
    }

    // Call this method when the player collects an item
    public void OnItemCollected(string item)
    {
        playerInventory.AddToInventory(item);
        UpdateUI();
        
    }
}