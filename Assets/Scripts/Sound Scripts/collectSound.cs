using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public AudioClip collectSound;
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to the game object
        audioSource = gameObject.AddComponent<AudioSource>();
        // Assign the collect sound clip to the AudioSource component
        audioSource.clip = collectSound;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object collecting this one has a Player tag
        if (other.CompareTag("Player"))
        {
            // Check if the collect sound clip is assigned
            if (collectSound != null)
            {
                // Play the collect sound clip
                audioSource.Play();
            }

            // You can add any other collect logic here
            // For example, destroying the collected object
            Destroy(gameObject);
        }
    }
}