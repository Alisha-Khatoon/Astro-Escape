using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager
using System.Collections; // Import System.Collections for IEnumerator

public class RocketCrash : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab for explosion effect

    private bool hasCrashed = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlanetSurface") && !hasCrashed)
        {
            hasCrashed = true;
            Debug.Log("Collision occurred");
            CrashEffect();
        }
    }

    void CrashEffect()
    {
        // Instantiate explosion prefab at rocket's position
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 3f); // Destroy explosion effect after 3 seconds         
        }
        
        // Play crash sound effect
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Load the Intermediate_Scene1
        StartCoroutine(LoadIntermediateSceneAfterDelay(3f)); // Adjust the delay if needed
    }

    IEnumerator LoadIntermediateSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene("Intermediate_Scene1"); // Load the scene
    }
}
