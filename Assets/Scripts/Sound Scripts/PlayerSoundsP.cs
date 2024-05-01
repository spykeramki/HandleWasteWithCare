using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsP : MonoBehaviour
{
    public AudioClip jumpSound; // Jump sound effect
    public AudioClip landSound; // Land sound effect
    public AudioSource audioSource; // Reference to the AudioSource component

    private bool isGrounded = true; // Flag to track if the player is grounded

    void Update()
    {
        // Check for player jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            PlayJumpSound();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            PlayLandSound();
        }
    }

    void PlayJumpSound()
    {
        // Play the jump sound effect
        audioSource.PlayOneShot(jumpSound);
    }

    void PlayLandSound()
    {
        // Play the land sound effect
        audioSource.PlayOneShot(landSound);
    }
}