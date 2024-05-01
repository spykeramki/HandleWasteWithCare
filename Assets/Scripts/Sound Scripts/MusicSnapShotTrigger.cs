using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSnapShotTrigger : MonoBehaviour
{

    public AudioMixerSnapshot targetSnapshot;
    public AudioMixerSnapshot silentSnapshot;
    public float fadeDuration = 1.0f;
    private bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetSnapshot.TransitionTo(fadeDuration);
            
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"))
        {
        playerInside = false;
        silentSnapshot.TransitionTo(fadeDuration);
        }
    }
    

}
