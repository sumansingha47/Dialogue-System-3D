using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDialogueTrigger : MonoBehaviour
{
    public TextAsset textJson;

    public DialogueData dialogueData;

    private void Start()
    {
        TextAsset data = Resources.Load<TextAsset>("GameHostD1");
        dialogueData = JsonUtility.FromJson<DialogueData>(data.text);
        Debug.Log("JsonDecode: " + dialogueData.payable.amount.ToString());

    }

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(dialogueData.messages, dialogueData.actors, dialogueData.payable);
    }
}

[System.Serializable]
public class DialogueData
{
    public Message[] messages;
    public Actor[] actors;
    public Payable payable;
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
    public Sprite sprite;
}

[System.Serializable]
public class Payable
{
    public int amount;
}