using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] footstepSounds; // Array of footstep sounds
    public float stepInterval = 0.5f; // Interval between each footstep sound
    public float volume = 1f; // Volume of footstep sounds

    private AudioSource audioSource;
    private float stepTimer = 0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the object.");
        }
    }

    private void Update()
    {
        // Check if the player is moving (you can replace this with your own condition)
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            // Count down the timer
            stepTimer -= Time.deltaTime;

            // Check if it's time to play a footstep sound
            if (stepTimer <= 0f)
            {
                // Pick a random footstep sound from the array
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];

                // Play the footstep sound
                audioSource.PlayOneShot(footstepSound, volume);

                // Reset the timer
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset the timer if the player is not moving
            stepTimer = 0f;
        }
    }
}