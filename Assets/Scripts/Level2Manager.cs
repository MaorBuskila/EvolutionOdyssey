using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public float timeLimit = 180f; // 3 minutes for Level 2
    private float timer;
    private bool timerEnded = false;

    public PlayerInventory playerInventory;
    public GameObject gameOverMenu;
    public ThoughtBubbleController thoughtBubbleController;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI objectiveText;

    private string currentObjective = "Gather materials for a shelter";

    void Start()
    {
        timer = timeLimit;
        gameOverMenu.SetActive(false);

        // Show the initial thought bubble
        thoughtBubbleController.ShowThought("I need to build a shelter before nightfall...");
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
        objectiveText.text = "Objective: " + currentObjective;
    }

    void CheckCondition()
    {
        // Check if the player has gathered all necessary materials for a shelter
        if (playerInventory.HasItem("Wood", 3) && playerInventory.HasItem("Leaves", 5) && playerInventory.HasItem("Vine", 2))
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
        SceneManager.LoadScene("Level3"); // Assuming there's a Level 3
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
        CheckProgressAndUpdateObjective();
    }

    void CheckProgressAndUpdateObjective()
    {
        if (playerInventory.HasItem("Wood", 3) && !playerInventory.HasItem("Leaves", 5))
        {
            currentObjective = "Collect leaves for the shelter";
            thoughtBubbleController.ShowThought("I need more leaves to cover the shelter...");
        }
        else if (playerInventory.HasItem("Wood", 3) && playerInventory.HasItem("Leaves", 5) && !playerInventory.HasItem("Vine", 2))
        {
            currentObjective = "Find vines to tie the shelter together";
            thoughtBubbleController.ShowThought("Some vines would help secure everything...");
        }
        else if (playerInventory.HasItem("Wood", 3) && playerInventory.HasItem("Leaves", 5) && playerInventory.HasItem("Vine", 2))
        {
            currentObjective = "Find a good spot to build the shelter";
            thoughtBubbleController.ShowThought("I have everything I need. Time to build!");
        }
    }
}