using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverScript : MonoBehaviour
{
    public AudioClip voiceOverClip;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to the game object
        audioSource = gameObject.AddComponent<AudioSource>();

        // Assign the voice over clip to the AudioSource component
        audioSource.clip = voiceOverClip;

        // Play the voice over clip
        audioSource.Play();
    }
}