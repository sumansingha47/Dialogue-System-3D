using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    public RectTransform nextDialogueHint;

    public Button yesButton;
    public Button noButton;

    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public static bool isActive = false;

    private void Start()
    {
        // Ensure buttons are hidden initially
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // Initialize the UI elements to be invisible
        backgroundBox.localScale = Vector3.zero;
        nextDialogueHint.localScale = Vector3.zero;

        // Register button click listeners
        noButton.onClick.AddListener(NoButtonClicked);
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, Payable payable)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

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
            actorImage.sprite = actorToDisplay.sprite;

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

        // Hide the Yes/No buttons
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

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
