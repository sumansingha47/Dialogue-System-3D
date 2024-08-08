using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Sprite currentNPCSprite;
    public Sprite playerSprite;
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    public RectTransform nextDialogueHint;

    public Slider priceSlider;
    public Text priceAmount;
    public Button submitButton;
    public Button yesButton;
    public Button noButton;

    private Message[] currentMessages;
    private Actor[] currentActors;
    private Bargaining bargainingData;
    private int activeMessage = 0;
    public static bool isActive = false;
    private int attemptsLeft;

    private void Start()
    {
        // Ensure buttons and sliders are hidden initially
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        priceSlider.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        // Initialize the UI elements to be invisible
        backgroundBox.localScale = Vector3.zero;
        nextDialogueHint.localScale = Vector3.zero;

        // Register button click listeners
        yesButton.onClick.AddListener(YesButtonClicked);
        noButton.onClick.AddListener(NoButtonClicked);
        submitButton.onClick.AddListener(SubmitButtonClicked);

        // Register the slider value change listener
        priceSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, Bargaining bargaining)
    {
        currentMessages = messages;
        currentActors = actors;
        bargainingData = bargaining;
        activeMessage = 0;
        isActive = true;
        attemptsLeft = bargainingData.attempts; // Initialize attempts left

        Debug.Log("Start conversation! Loaded messages: " + messages.Length);
        DisplayMessage();

        // Animate the appearance of the dialogue UI
        nextDialogueHint.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    private void DisplayMessage()
    {
        if (activeMessage < currentMessages.Length)
        {
            Message messageToDisplay = currentMessages[activeMessage];
            messageText.text = messageToDisplay.message;

            Actor actorToDisplay = currentActors[messageToDisplay.actorId];
            actorName.text = actorToDisplay.name;

            if (messageToDisplay.actorId == 0)
            {
                actorImage.sprite = playerSprite;
            }
            else if (messageToDisplay.actorId == 1)
            {
                actorImage.sprite = currentNPCSprite;
            }

            AnimateTextColor();
        }
        else
        {
            Debug.LogWarning("Attempted to display a message that does not exist.");
        }
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            // Show the Yes/No buttons when the dialogue ends
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
        }
    }

    public void YesButtonClicked()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // Show the slider and submit button for bargaining
        priceSlider.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
    }

    public void OnSliderValueChanged(float value)
    {
        // Update the price amount text as the slider value changes in real-time
        priceAmount.text = "Rupees " + Mathf.RoundToInt(value) + "/-";
    }

    public void SubmitButtonClicked()
    {
        float offeredPrice = priceSlider.value;
        string response = GetResponseForPrice(offeredPrice);

        messageText.text = response;

        attemptsLeft--;

        if (attemptsLeft <= 0)
        {
            NoButtonClicked();
        }
    }

    private string GetResponseForPrice(float price)
    {
        foreach (var response in bargainingData.responses)
        {
            string[] range = response.range.Split('-');
            int min = int.Parse(range[0]);
            int max = int.Parse(range[1]);

            if (price >= min && price <= max)
            {
                return response.response;
            }
        }

        return "I don't understand the price.";
    }

    public void NoButtonClicked()
    {
        Debug.Log("Conversation ended!");

        // Ensure the UI elements are not null before scaling
        if (backgroundBox != null)
        {
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
        }
        else
        {
            Debug.LogError("backgroundBox is null.");
        }

        if (nextDialogueHint != null)
        {
            nextDialogueHint.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
        }
        else
        {
            Debug.LogError("nextDialogueHint is null.");
        }

        // Hide the Yes/No buttons and other UI elements
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        priceSlider.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        isActive = false;
    }

    private void AnimateTextColor()
    {
        // Animate the text fading in
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    private void Update()
    {
        // Handle advancing the conversation with the F key
        if (Input.GetKeyDown(KeyCode.F) && isActive)
        {
            NextMessage();
        }
    }
}
