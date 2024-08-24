using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip npcAudioClip;
    public AudioClip playerAudioClip;

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
    private Sprite currentNPCSprite;
    private Bargaining bargainingData;
    private int activeMessage = 0;
    public static bool isActive = false;
    private int attemptsLeft;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        priceSlider.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        backgroundBox.localScale = Vector3.zero;
        nextDialogueHint.localScale = Vector3.zero;

        yesButton.onClick.AddListener(YesButtonClicked);
        noButton.onClick.AddListener(NoButtonClicked);
        submitButton.onClick.AddListener(SubmitButtonClicked);

        priceSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, Sprite npcSprite, Bargaining bargaining)
    {
        currentMessages = messages;
        currentActors = actors;
        currentNPCSprite = npcSprite;
        bargainingData = bargaining;
        activeMessage = 0;
        isActive = true;
        attemptsLeft = bargainingData.attempts;

        priceAmount.text = "Rupees " + bargaining.min_price.ToString();
        priceSlider.minValue = bargaining.min_price;
        priceSlider.maxValue = bargaining.max_price;

        Debug.Log("Start conversation! Loaded messages: " + messages.Length);
        DisplayMessage();

        nextDialogueHint.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    private void DisplayMessage()
    {
        if (currentMessages != null && activeMessage < currentMessages.Length)
        {
            PlayerInteraction.instance.NPCLookAt(true);
            Message messageToDisplay = currentMessages[activeMessage];
            messageText.text = messageToDisplay.message;

            Actor actorToDisplay = currentActors[messageToDisplay.actorId];
            actorName.text = actorToDisplay.name;

            if (messageToDisplay.actorId == 0)
            {
                actorImage.sprite = playerSprite;
                audioSource.PlayOneShot(playerAudioClip);

                BryceNPCAnimation.instance.NPCTalking(false);
                BryceNPCAnimation.instance.NPCTalking2(false);
            }
            else if (messageToDisplay.actorId == 1)
            {
                actorImage.sprite = currentNPCSprite;

                if (PlayerInteraction.instance._tag == "Bryce NPC")
                {
                    audioSource.PlayOneShot(npcAudioClip);
                    BryceNPCAnimation.instance.NPCTalking2(true);
                    StartCoroutine(WaitAndStopNPCTalking2(2f));
                }
                else if (PlayerInteraction.instance._tag == "NPC")
                {
                    audioSource.PlayOneShot(npcAudioClip);
                    NPCAnimation.instance.NPCTalking2(true);
                    StartCoroutine(WaitAndStopNPCTalking2(2f));
                }
            }

            AnimateTextColor();
        }
        else
        {
            Debug.LogWarning("Attempted to display a message that does not exist.");
        }
    }

    private IEnumerator WaitAndStopNPCTalking2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BryceNPCAnimation.instance.NPCTalking2(false);
        NPCAnimation.instance.NPCTalking2(false);
    }

    public void NextMessage()
    {
        if (activeMessage < currentMessages.Length - 1)
        {
            activeMessage++;
            DisplayMessage();
        }
        else if (activeMessage >= currentMessages.Length - 1 && currentMessages[activeMessage].message == "Do you want to buy?")
        {
            audioSource.PlayOneShot(npcAudioClip);
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
        }
        else
        {
            nextDialogueHint.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            PlayerInteraction.instance.NPCLookAt(false);
            isActive = false;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        priceAmount.text = "Rupees " + Mathf.RoundToInt(value) + "/-";
    }

    public void SubmitButtonClicked()
    {
        float offeredPrice = priceSlider.value;
        string response = GetResponseForPrice(offeredPrice);

        messageText.text = response;

        attemptsLeft--;

        if (attemptsLeft <= 0 || response == "Okay, Deal.")
        {
            audioSource.PlayOneShot(npcAudioClip);
            BryceNPCAnimation.instance.NPCTalking2(false);
            BryceNPCAnimation.instance.NPCTalking(true);
            StartCoroutine(WaitAndEndConversation(2f));
        }
    }

    private IEnumerator WaitAndEndConversation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        NoButtonClicked();
        BryceNPCAnimation.instance.NPCTalking(false);
        BryceNPCAnimation.instance.NPCTalking2(false);
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

    public void YesButtonClicked()
    {
        messageText.text = "Select Amount Through slider!";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        priceSlider.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
    }

    public void NoButtonClicked()
    {
        Debug.Log("Conversation ended!");

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

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        priceSlider.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        isActive = false;
        priceAmount.text = "Rupees 0/-";
        priceSlider.minValue = 0;
        priceSlider.maxValue = 0;
    }

    private void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isActive)
        {
            NextMessage();
        }
    }
}
