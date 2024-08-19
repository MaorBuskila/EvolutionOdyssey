using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Canvas mainCanvas;
    public TextMeshProUGUI titleText;
    public GameObject mainMenu;
    public Button startButton;
    public Button creditsButton;
    public Button exitButton;
    public GameObject creditsPanel;
    public TextMeshProUGUI[] authorTexts;
    public Button backButton;

    [Header("Styling")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Sprite backgroundImage;
    public TMP_FontAsset customFont;

    private Image backgroundImageComponent;
    private PostProcessVolume postProcessVolume;

    void Start()
    {
        SetupBackground();
        SetupButtons();
        SetupCreditsPanel();
        SetupBlurEffect();
        ApplyCustomFont();
    }

    void SetupBackground()
    {
        Transform backgroundTransform = mainCanvas.transform.Find("Background");
        if (backgroundTransform == null)
        {
            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(mainCanvas.transform);
            backgroundObject.transform.SetAsFirstSibling();
            backgroundImageComponent = backgroundObject.AddComponent<Image>();
        }
        else
        {
            backgroundImageComponent = backgroundTransform.GetComponent<Image>();
        }

        backgroundImageComponent.sprite = backgroundImage;
        backgroundImageComponent.color = Color.white;  // Use white color to show the image as-is

        RectTransform rectTransform = backgroundImageComponent.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    void SetupButtons()
    {
        SetupButton(startButton, StartGame);
        SetupButton(creditsButton, ShowCredits);
        SetupButton(exitButton, ExitGame);
        SetupButton(backButton, BackToMainMenu);
    }

    void SetupButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);

        // Make button background transparent
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = new Color(0, 0, 0, 0); // Fully transparent
        }

        // Set initial text color to white
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.color = normalColor;
        }

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();
        AddEventTriggerListener(trigger, EventTriggerType.PointerEnter, (data) => { OnButtonHover(button, true); });
        AddEventTriggerListener(trigger, EventTriggerType.PointerExit, (data) => { OnButtonHover(button, false); });
    }

    void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void OnButtonHover(Button button, bool isHovering)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.color = isHovering ? hoverColor : normalColor;
        }
    }

    void SetupCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    void SetupBlurEffect()
    {
        GameObject postProcessObj = new GameObject("PostProcessVolume");
        postProcessVolume = postProcessObj.AddComponent<PostProcessVolume>();
        postProcessVolume.isGlobal = true;
        postProcessVolume.priority = 1;

        PostProcessProfile profile = ScriptableObject.CreateInstance<PostProcessProfile>();
        postProcessVolume.profile = profile;

        DepthOfField depthOfField = profile.AddSettings<DepthOfField>();
        depthOfField.enabled.Override(false);
        depthOfField.focusDistance.Override(10f);
        depthOfField.aperture.Override(5.6f);
        depthOfField.focalLength.Override(50f);
    }

    void ApplyCustomFont()
    {
        if (customFont != null)
        {
            ApplyFontToText(titleText);
            ApplyFontToButton(startButton);
            ApplyFontToButton(creditsButton);
            ApplyFontToButton(exitButton);
            ApplyFontToButton(backButton);

            foreach (TextMeshProUGUI authorText in authorTexts)
            {
                ApplyFontToText(authorText);
            }
        }
    }

    void ApplyFontToText(TextMeshProUGUI text)
    {
        if (text != null)
            text.font = customFont;
    }

    void ApplyFontToButton(Button button)
    {
        if (button == null) return;
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
            buttonText.font = customFont;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowCredits()
    {
        if (creditsPanel != null && mainMenu != null)
        {
            creditsPanel.SetActive(true);
            mainMenu.SetActive(false);
            EnableBlurEffect(true);
        }
    }

    public void BackToMainMenu()
    {
        Debug.Log("BackToMainMenu called");
        if (creditsPanel != null && mainMenu != null)
        {
            creditsPanel.SetActive(false);
            mainMenu.SetActive(true);
            EnableBlurEffect(false);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void EnableBlurEffect(bool enable)
    {
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out DepthOfField depthOfField))
        {
            depthOfField.enabled.Override(enable);
        }
    }
}