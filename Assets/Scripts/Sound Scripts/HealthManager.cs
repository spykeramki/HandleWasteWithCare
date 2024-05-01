using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public AudioClip lowHealthSound;
    public AudioClip criticalHealthSound;
    private AudioSource audioSource;

    // Health thresholds for different sounds
    public float criticalThreshold = 0.05f;
    public float lowThreshold = 0.1f;
    public float mediumThreshold = 0.2f;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        float healthPercentage = (float)currentHealth / maxHealth;

        // Check if health is below critical threshold (5%)
        if (healthPercentage <= criticalThreshold && criticalHealthSound != null)
        {
            PlaySound(criticalHealthSound);
        }
        // Check if health is below low threshold (10%)
        else if (healthPercentage <= lowThreshold && lowHealthSound != null)
        {
            PlaySound(lowHealthSound);
        }
        // Check if health is below medium threshold (20%)
        else if (healthPercentage <= mediumThreshold)
        {
            // Play some other sound for medium health
        }
    }

    // Example function to decrease health
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        // Check if health is depleted
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Game over logic
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
