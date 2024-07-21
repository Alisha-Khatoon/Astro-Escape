using UnityEngine;

public class RocketLaunchAudio : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip launchSound; // Audio clip to play when launch is triggered

    void Start()
    {
        // Ensure AudioSource is assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found or assigned.");
            }
        }
    }

    void Update()
    {
        // Example: Detect launch event (e.g., when user presses spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if AudioSource and AudioClip are assigned
            if (audioSource != null && launchSound != null)
            {
                // Play the launch sound
                audioSource.PlayOneShot(launchSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or AudioClip not assigned.");
            }
        }
    }
}
