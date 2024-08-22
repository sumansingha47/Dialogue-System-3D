using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction instance;

    public new Camera camera;
    public GameObject eKeyHint;

    private JsonDialogueTrigger trigger;
    private bool isPlayerInRange = false;

    public Transform npcTranform;
    public string _tag;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        RayCastInteraction();

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (trigger != null)
            {
                trigger.StartDialogue();
            }
        }
    }

    private void RayCastInteraction()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.transform.CompareTag("Bryce NPC"))
            {
                eKeyHint.gameObject.SetActive(true);
                npcTranform = hit.transform;

                JsonDialogueTrigger newTrigger = hit.transform.GetComponent<JsonDialogueTrigger>();
                if (newTrigger != trigger)
                {
                    trigger = newTrigger;
                }
                isPlayerInRange = true;

                _tag = "Bryce NPC";
            }
            else if (hit.transform.CompareTag("NPC"))
            {
                eKeyHint.gameObject.SetActive(true);
                npcTranform = hit.transform;
                JsonDialogueTrigger newTrigger = hit.transform.GetComponent<JsonDialogueTrigger>();
                if (newTrigger != trigger)
                {
                    trigger = newTrigger;
                }
                isPlayerInRange = true;

                _tag = "NPC";
            }
            else
            {
                eKeyHint.gameObject.SetActive(false);
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        trigger = null;
        isPlayerInRange = false;
    }

    public void NPCLookAt(bool isNPCLookAt)
    {
        if (isNPCLookAt)
        npcTranform.LookAt(transform.position);
    }
}
