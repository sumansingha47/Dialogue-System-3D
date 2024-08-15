using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonDialogueTrigger : MonoBehaviour
{
    public TextAsset textJson;
    public Sprite npcSprite;

    public DialogueData dialogueData;

    private void Start()
    {
        if (textJson != null)
        {
            dialogueData = JsonUtility.FromJson<DialogueData>(textJson.text);
            Debug.Log("JSON Loaded: Min Price = " + dialogueData.bargaining.min_price);

            foreach (var response in dialogueData.bargaining.responses)
            {
                Debug.Log("Response Range: " + response.range + ", Response: " + response.response);
            }
        }
        else
        {
            Debug.LogError("Text JSON file is not assigned.");
        }
    }

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(dialogueData.messages, dialogueData.actors,npcSprite, dialogueData.bargaining);
    }
}

[System.Serializable]
public class DialogueData
{
    public Message[] messages;
    public Actor[] actors;
    public Bargaining bargaining;
}

[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
  //  public Sprite sprite;
}

[System.Serializable]
public class Bargaining
{
    public int min_price;
    public int max_price;
    public int attempts;
    public BargainingResponse[] responses;  // Fixed the property name here
}

[System.Serializable]
public class BargainingResponse
{
    public string range;
    public string response;
}
