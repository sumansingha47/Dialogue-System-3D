using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public new Camera camera;

    private JsonDialogueTrigger trigger;
    private bool isPlayerInRange = false;

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

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.CompareTag("NPC"))
            {
                JsonDialogueTrigger newTrigger = hit.transform.GetComponent<JsonDialogueTrigger>();
                if (newTrigger != trigger)
                {
                    trigger = newTrigger;
                }
                isPlayerInRange = true;
            }
            else
            {
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
}
