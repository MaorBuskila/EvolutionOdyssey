using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Button gameOverRestartButton;
    public Button gameOverMainMenuButton;

    [Header("Pause Panel")]
    public GameObject pausePanel;
    public Button pauseResumeButton;
    public Button pauseRestartButton;
    public Button pauseMainMenuButton;

    [Header("Game Controls")]
    public Button pauseButton;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindReferences();
        SetupButtonListeners();
        // ResumeGame(); // Ensure the game is not paused when a new scene loads
    }

    private void FindReferences()
    {
        gameOverPanel = GameObject.Find("GameOverCanvas")?.gameObject;
        pausePanel = GameObject.Find("PausedCanvas")?.gameObject;
        pauseButton = GameObject.Find("Canvas")?.transform.Find("PauseButton")?.GetComponent<Button>();

        if (gameOverPanel != null)
        {
            gameOverRestartButton = gameOverPanel.transform.Find("Buttons/RestartButton")?.GetComponent<Button>();
            gameOverMainMenuButton = gameOverPanel.transform.Find("Buttons/MainMenuButton")?.GetComponent<Button>();
        }
        else
        {
            Debug.Log("gameOverPanel not found");

        }

        if (pausePanel != null)
        {
            pauseResumeButton = pausePanel.transform.Find("Buttons/ResumeButton")?.GetComponent<Button>();
            pauseRestartButton = pausePanel.transform.Find("Buttons/RestartButton")?.GetComponent<Button>();
            pauseMainMenuButton = pausePanel.transform.Find("Buttons/MainMenuButton")?.GetComponent<Button>();
        }
        else
        {
            Debug.Log("pausePanel not found");

        }

    }

    private void SetupButtonListeners()
    {
        if (gameOverRestartButton != null)
            gameOverRestartButton.onClick.AddListener(RestartGame);
        if (gameOverMainMenuButton != null)
            gameOverMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (pauseResumeButton != null)
            pauseResumeButton.onClick.AddListener(ResumeGame);
        if (pauseRestartButton != null)
            pauseRestartButton.onClick.AddListener(RestartGame);
        if (pauseMainMenuButton != null)
            pauseMainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);
        else
        {
            Debug.Log("pauseButton not found");
        }

        Debug.Log("Button listeners set up");
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over panel activated");
        }
        else
        {
            Debug.LogError("Game Over panel is null");
        }
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame called");
        isPaused = true;
        Time.timeScale = 0;
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            Debug.Log("Pause panel activated");
        }
        else
        {
            Debug.LogError("Pause panel is null");
        }
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame called");
        isPaused = false;
        Time.timeScale = 1;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Debug.Log("Pause panel deactivated");
        }
        else
        {
            Debug.LogError("Pause panel is null");
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame called");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("ReturnToMainMenu called");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene"); // Make sure you have a scene named "MainMenuScene"
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}