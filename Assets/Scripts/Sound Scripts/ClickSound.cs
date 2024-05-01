using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to the game object
        audioSource = gameObject.AddComponent<AudioSource>();
        // Assign the sound clip to the AudioSource component
        audioSource.clip = soundClip;
    }

    void OnMouseDown()
    {
        // Check if the sound clip is assigned
        if (soundClip != null)
        {
            // Play the sound clip
            audioSource.Play();
        }
    }
}
