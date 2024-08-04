using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator myAnim;
    private Rigidbody rb;

    private bool isRunning = false;
    private bool isJumping = false;
    private bool isWalking = false;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the player is running
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            isRunning = true;
            isWalking = false; // If running, the player cannot be walking
        }
        else if (Input.GetKey(KeyCode.W)) // Check if the player is walking
        {
            isRunning = false;
            isWalking = true;
        }
        else // The player is neither running nor walking
        {
            isRunning = false;
            isWalking = false;
        }

        // Check if the player is jumping
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        // Update animator parameters
        myAnim.SetBool("Run", isRunning);
        myAnim.SetBool("Jump", isJumping);
        myAnim.SetBool("Walk", isWalking);
    }
}
