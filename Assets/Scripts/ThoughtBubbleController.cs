using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ThoughtBubbleController : MonoBehaviour
{
    public GameObject thoughtBubblePanel;
    public TextMeshProUGUI thoughtBubbleText;
    public RectTransform bubbleRect;

    public float displayDuration = 3f;
    private float timer;

    public Vector2 padding = new Vector2(20, 20);
    public float minWidth = 100f;
    public float maxWidth = 300f;
    public float minHeight = 50f;
    public float maxHeight = 200f;

void Start()
{
    thoughtBubblePanel.SetActive(false);
    if (bubbleRect == null)
        bubbleRect = thoughtBubblePanel.GetComponent<RectTransform>();
    
    // Set initial size to match the RectTransform
    maxWidth = bubbleRect.rect.width;
    maxHeight = bubbleRect.rect.height;
}
    public void ShowThought(string message)
    {
        SetText(message);
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

    private void SetText(string text)
    {
        thoughtBubbleText.text = text;
        
        // Reset text component size
        thoughtBubbleText.rectTransform.sizeDelta = new Vector2(maxWidth - padding.x, 0);
        
        // Force layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(thoughtBubbleText.rectTransform);
        
        // Calculate sizes
        float textWidth = Mathf.Clamp(thoughtBubbleText.preferredWidth, minWidth - padding.x, maxWidth - padding.x);
        float textHeight = Mathf.Clamp(thoughtBubbleText.preferredHeight, minHeight - padding.y, maxHeight - padding.y);
        
        // Set text size
        thoughtBubbleText.rectTransform.sizeDelta = new Vector2(textWidth, textHeight);
        
        // Set bubble size
        bubbleRect.sizeDelta = new Vector2(textWidth + padding.x, textHeight + padding.y);
        
        // Final layout update
        LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleRect);
    }
}