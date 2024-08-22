using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public static NPCAnimation instance;

    public Animator npcAnim;

    private bool isWalking = false;
    private bool isTalking = false;
    private bool isTalking2 = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        npcAnim = GetComponent<Animator>();

        // Log an error if the Animator component is not found
        if (npcAnim == null)
        {
            Debug.LogError("NPCAnimator component not found on " + gameObject.name);
        }
    }

    private void Update()
    {
        // Update the Animator only if it exists
        if (npcAnim != null)
        {
            npcAnim.SetBool("Walk", isWalking);
            npcAnim.SetBool("Talking", isTalking);
            npcAnim.SetBool("Talking2", isTalking2);
        }
    }

    public void NPCTalking(bool talkBoolean)
    {
        isTalking = talkBoolean;
    }

    public void NPCWalking(bool walkBoolean)
    {
        isWalking = walkBoolean;
    }

    public void NPCTalking2(bool talkBoolean)
    {
        isTalking2 = talkBoolean;
    }
}

