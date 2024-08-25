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
        Debug.Log("MainMenuController: Start method called");
        SetupBackground();
        SetupButtons();
        SetupCreditsPanel();
        SetupBlurEffect();
        ApplyCustomFont();
    }

    void SetupBackground()
    {
        Debug.Log("MainMenuController: SetupBackground method called");
        Transform backgroundTransform = mainCanvas.transform.Find("Background");
        if (backgroundTransform == null)
        {
            Debug.Log("MainMenuController: Creating new Background object");
            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(mainCanvas.transform);
            backgroundObject.transform.SetAsFirstSibling();
            backgroundImageComponent = backgroundObject.AddComponent<Image>();
        }
        else
        {
            Debug.Log("MainMenuController: Using existing Background object");
            backgroundImageComponent = backgroundTransform.GetComponent<Image>();
        }

        backgroundImageComponent.sprite = backgroundImage;
        backgroundImageComponent.color = Color.white;

        RectTransform rectTransform = backgroundImageComponent.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
        Debug.Log("MainMenuController: Background setup complete");
    }

    void SetupButtons()
    {
        Debug.Log("MainMenuController: SetupButtons method called");
        SetupButton(startButton, StartGame);
        SetupButton(creditsButton, ShowCredits);
        SetupButton(exitButton, ExitGame);
        SetupButton(backButton, BackToMainMenu);
    }

    void SetupButton(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null)
        {
            Debug.LogWarning("MainMenuController: Button is null in SetupButton");
            return;
        }

        Debug.Log($"MainMenuController: Setting up button {button.name}");
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = new Color(0, 0, 0, 0);
        }

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
        Debug.Log($"MainMenuController: Button {button.name} setup complete");
    }

    void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        Debug.Log($"MainMenuController: Adding EventTrigger listener for {eventType}");
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void OnButtonHover(Button button, bool isHovering)
    {
        Debug.Log($"MainMenuController: Button {button.name} hover state changed to {isHovering}");
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.color = isHovering ? hoverColor : normalColor;
        }
    }

    void SetupCreditsPanel()
    {
        Debug.Log("MainMenuController: SetupCreditsPanel method called");
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            Debug.Log("MainMenuController: Credits panel initialized and hidden");
        }
        else
        {
            Debug.LogWarning("MainMenuController: Credits panel is null");
        }
    }

    void SetupBlurEffect()
    {
        Debug.Log("MainMenuController: SetupBlurEffect method called");
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
        Debug.Log("MainMenuController: Blur effect setup complete");
    }

    void ApplyCustomFont()
    {
        Debug.Log("MainMenuController: ApplyCustomFont method called");
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
            Debug.Log("MainMenuController: Custom font applied to all text elements");
        }
        else
        {
            Debug.LogWarning("MainMenuController: Custom font is null");
        }
    }

    void ApplyFontToText(TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.font = customFont;
            Debug.Log($"MainMenuController: Custom font applied to text: {text.name}");
        }
        else
        {
            Debug.LogWarning("MainMenuController: Attempted to apply font to null text element");
        }
    }

    void ApplyFontToButton(Button button)
    {
        if (button == null)
        {
            Debug.LogWarning("MainMenuController: Attempted to apply font to null button");
            return;
        }
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.font = customFont;
            Debug.Log($"MainMenuController: Custom font applied to button: {button.name}");
        }
        else
        {
            Debug.LogWarning($"MainMenuController: No TextMeshProUGUI found on button: {button.name}");
        }
    }

    public void StartGame()
    {
        Debug.Log("MainMenuController: StartGame method called");
        if (GameManager.Instance != null)
        {
            Debug.Log("MainMenuController: Destroying existing GameManager");
            Destroy(GameManager.Instance.gameObject);
        }
        Debug.Log("MainMenuController: Loading GameScene");
        SceneManager.LoadScene("GameScene");
    }

    public void ShowCredits()
    {
        Debug.Log("MainMenuController: ShowCredits method called");
        if (creditsPanel != null && mainMenu != null)
        {
            creditsPanel.SetActive(true);
            mainMenu.SetActive(false);
            EnableBlurEffect(true);
            Debug.Log("MainMenuController: Credits shown, main menu hidden, blur effect enabled");
        }
        else
        {
            Debug.LogWarning("MainMenuController: creditsPanel or mainMenu is null in ShowCredits");
        }
    }

    public void BackToMainMenu()
    {
        Debug.Log("MainMenuController: BackToMainMenu method called");
        if (creditsPanel != null && mainMenu != null)
        {
            creditsPanel.SetActive(false);
            mainMenu.SetActive(true);
            EnableBlurEffect(false);
            Debug.Log("MainMenuController: Main menu shown, credits hidden, blur effect disabled");
        }
        else
        {
            Debug.LogWarning("MainMenuController: creditsPanel or mainMenu is null in BackToMainMenu");
        }
    }

    public void ExitGame()
    {
        Debug.Log("MainMenuController: ExitGame method called");
#if UNITY_EDITOR
        Debug.Log("MainMenuController: Stopping play mode in Unity Editor");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("MainMenuController: Quitting application");
        Application.Quit();
#endif
    }

    private void EnableBlurEffect(bool enable)
    {
        Debug.Log($"MainMenuController: EnableBlurEffect called with enable={enable}");
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out DepthOfField depthOfField))
        {
            depthOfField.enabled.Override(enable);
            Debug.Log($"MainMenuController: Blur effect {(enable ? "enabled" : "disabled")}");
        }
        else
        {
            Debug.LogWarning("MainMenuController: postProcessVolume or DepthOfField is null in EnableBlurEffect");
        }
    }
}