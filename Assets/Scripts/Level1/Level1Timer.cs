using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level1Timer : MonoBehaviour
{
    public float timeLimit = 20f;
    private float timer;
    private bool timerEnded = false;  // New flag to check if the timer has ended

    public PlayerInventory playerInventory;

    public TextMeshProUGUI timerText;  // Reference to the UI Text for the timer
    public TextMeshProUGUI inventoryText;  // Reference to the UI Text for the inventory

    void Start()
    {
        timer = timeLimit;
    }

    void Update()
    {
        if (timerEnded) return;  // Stop the timer if it has ended

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

        // Display each item in the inventory on a new line
        inventoryText.text = "Inventory:\n" + string.Join("\n", playerInventory.GetInventoryItems());
    }

    void CheckCondition()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (playerInventory.HasItem("Rock") && playerInventory.HasItem("Wood"))
            {
                LoadNextLevel();
            }
            else
            {
                GameOver();
            }
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene("Level2");  // Load the next level
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");  // Load the game over scene
    }
}
