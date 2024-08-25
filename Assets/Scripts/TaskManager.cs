using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public int taskCompletionGoal = 2;
    public int currentTaskProgress = 0;
    public float timeLimit = 90f;
    private float timeRemaining;

    void Start()
    {
        timeRemaining = timeLimit;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            if (currentTaskProgress >= taskCompletionGoal)
            {
                // Load next level
                SceneManager.LoadScene("NextLevel");
            }
            else
            {
                // Load losing game window
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void CollectObject()
    {
        currentTaskProgress++;
    }
}
