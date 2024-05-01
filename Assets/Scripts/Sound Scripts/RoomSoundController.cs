using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSoundController : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        audioSource.loop = true; // Loop the sound continuously
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play(); // Play the sound when player enters the room
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop(); // Stop the sound when player exits the room
        }
    }
}