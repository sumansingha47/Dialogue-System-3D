using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenByTrigger : MonoBehaviour
{
    public Transform door; // The door that will be opened
    public Vector3 openPositionOffset; // The position offset when the door is open
    public float doorOpenSpeed = 2f; // Speed at which the door opens

    private Vector3 closedPosition; // The original position of the door
    private Vector3 openPosition; // The target position when the door is open

    private void Start()
    {
        closedPosition = door.localPosition;
        openPosition = closedPosition + openPositionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines(); // Stop any ongoing door movement
            StartCoroutine(MoveDoor(openPosition)); // Open the door
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines(); // Stop any ongoing door movement
            StartCoroutine(MoveDoor(closedPosition)); // Close the door
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.localPosition, targetPosition) > 0.01f)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, targetPosition, Time.deltaTime * doorOpenSpeed);
            yield return null;
        }

        door.localPosition = targetPosition; // Ensure the door is exactly in the target position
    }
}
