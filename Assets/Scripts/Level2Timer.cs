using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level2Timer : MonoBehaviour
{
    public float timeLimit = 20f;
    private float timer;
    private bool timerEnded = false;

    public PlayerInventory playerInventory;
    public GameObject gameOverMenu;
    public ThoughtBubbleController thoughtBubbleController;  // Reference to the thought bubble controller

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI inventoryText;

    void Start()
    {
        timer = timeLimit;
        gameOverMenu.SetActive(false);

        // Show the thought bubble at the start of the level
        thoughtBubbleController.ShowThought("I'm hungry...");
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
    }

    void CheckCondition()
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

    void LoadNextLevel()
    {
        SceneManager.LoadScene("Level1");//TODO: Change to Level3
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
}
