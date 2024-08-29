using UnityEngine;
using TMPro;
public class ThoughtBubbleController : MonoBehaviour
{
    public GameObject thoughtBubblePanel;  // Reference to the thought bubble panel
    public TextMeshProUGUI thoughtBubbleText;  // Reference to the text component
    private float displayDuration = 3f;  // Duration for how long the thought bubble should be shown
    private float timer;
    void Start()
    {
        // Initially hide the thought bubble
        thoughtBubblePanel.SetActive(false);
    }
    public void ShowThought(string message)
    {
        thoughtBubbleText.text = message;
        thoughtBubblePanel.SetActive(true);
        timer = displayDuration;
    }
    void Update()
    {
        if (thoughtBubblePanel.activeSelf)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                thoughtBubblePanel.SetActive(false);
            }
        }
    }
}