using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenByTrigger : MonoBehaviour
{
    public Transform door;
    public Vector3 openPositionOffset;
    public float doorOpenSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private void Start()
    {
        closedPosition = door.localPosition;
        openPosition = closedPosition + openPositionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openPosition));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(closedPosition));
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.localPosition, targetPosition) > 0.01f)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, targetPosition, Time.deltaTime * doorOpenSpeed);
            yield return null;
        }

        door.localPosition = targetPosition;
    }
}
