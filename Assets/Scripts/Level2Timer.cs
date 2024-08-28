using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelTimer : MonoBehaviour
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
    public TextMeshProUGUI temperatureText;

    private float playerTemperature = 100f; // Starting temperature
    private float temperatureDecreaseRate = 1f; // Degrees per second

    [System.Serializable]
    public class Objective
    {
        public string description;
        public List<string> requiredItems;
        public string thoughtBubble;
    }

    public List<Objective> objectives;
    private int currentObjectiveIndex = 0;

    void Start()
    {
        timer = timeLimit;
        gameOverMenu.SetActive(false);
        UpdateObjective();
    }

    void Update()
    {
        if (timerEnded) return;
        
        timer -= Time.deltaTime;
        playerTemperature -= temperatureDecreaseRate * Time.deltaTime;
        
        UpdateUI();
        CheckCurrentObjective();
        
        if (timer <= 0 || playerTemperature <= 0)
        {
            timerEnded = true;
            CheckFinalCondition();
        }
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        temperatureText.text = "Temperature: " + Mathf.Ceil(playerTemperature).ToString();
        inventoryText.text = "Inventory:\n" + string.Join("\n", playerInventory.GetInventoryItems());
        objectiveText.text = "Objective: " + objectives[currentObjectiveIndex].description;
    }

    void CheckCurrentObjective()
    {
        Objective currentObjective = objectives[currentObjectiveIndex];
        bool objectiveComplete = true;
        foreach (string item in currentObjective.requiredItems)
        {
            if (!playerInventory.HasItem(item))
            {
                objectiveComplete = false;
                break;
            }
        }

        if (objectiveComplete)
        {
            currentObjectiveIndex++;
            if (currentObjectiveIndex < objectives.Count)
            {
                UpdateObjective();
            }
        }
    }

    void UpdateObjective()
    {
        Objective currentObjective = objectives[currentObjectiveIndex];
        thoughtBubbleController.ShowThought(currentObjective.thoughtBubble);
        UpdateUI();
    }

    void CheckFinalCondition()
    {
        if (currentObjectiveIndex >= objectives.Count)
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

    public void DecreaseTemperature(float amount)
    {
        playerTemperature -= amount;
    }

    public void IncreaseTemperature(float amount)
    {
        playerTemperature = Mathf.Min(playerTemperature + amount, 100f);
    }
}