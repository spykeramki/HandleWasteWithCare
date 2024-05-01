using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to the game object
        audioSource = gameObject.AddComponent<AudioSource>();
        // Assign the music clip to the AudioSource component
        audioSource.clip = musicClip;
        // Make the music loop
        audioSource.loop = true;
        // Play the music
        audioSource.Play();
    }
}