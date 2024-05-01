using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip doorOpenSound; // Door opening sound effect
    public AudioClip doorCloseSound; // Door closing sound effect
    public AudioSource audioSource; // Reference to the AudioSource component

    public void OpenDoor()
    {
        // Play the door opening sound effect
        audioSource.PlayOneShot(doorOpenSound);
    }

    public void CloseDoor()
    {
        // Play the door closing sound effect
        audioSource.PlayOneShot(doorCloseSound);
    }
}