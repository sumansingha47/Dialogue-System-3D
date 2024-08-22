using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator myAnim;

    private bool isRunning = false;
    private bool isJumping = false;
    private bool isWalking = false;
    private bool isWalkingBack = false;
    private bool isRunningBack = false;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        if (myAnim == null)
        {
            Debug.LogError("Animator component not found!");
        }
    }

    private void Update()
    {
        if (DialogueManager.isActive)
        {
            ResetAnimationStates();
            return;
        }

        UpdateMovementStates();
        UpdateAnimationStates();
    }

    private void ResetAnimationStates()
    {
        isRunning = false;
        isJumping = false;
        isWalking = false;
        isWalkingBack = false;
        isRunningBack = false;

        UpdateAnimationStates();
    }

    private void UpdateMovementStates()
    {
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool isWPressed = Input.GetKey(KeyCode.W);
        bool isSPressed = Input.GetKey(KeyCode.S);
        bool isAPressed = Input.GetKey(KeyCode.A);
        bool isDPressed = Input.GetKey(KeyCode.D);

        // Running with Shift + W, Shift + A, or Shift + D
        isRunning = isShiftPressed && (isWPressed || isAPressed || isDPressed);

        // Walking without Shift
        isWalking = !isShiftPressed && (isWPressed || isAPressed || isDPressed);

        isJumping = Input.GetKey(KeyCode.Space);

        // Running back with Shift + S
        isRunningBack = isShiftPressed && isSPressed;

        // Walking back without Shift
        isWalkingBack = !isShiftPressed && isSPressed;
    }

    private void UpdateAnimationStates()
    {
        myAnim.SetBool("Run", isRunning);
        myAnim.SetBool("Jump", isJumping);
        myAnim.SetBool("Walk", isWalking);
        myAnim.SetBool("RunBack", isRunningBack);
        myAnim.SetBool("WalkBack", isWalkingBack);
    }
}
