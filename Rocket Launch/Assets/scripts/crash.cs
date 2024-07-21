using UnityEngine;

public class RocketCrash : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab for explosion effect

    private bool hasCrashed = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlanetSurface") && !hasCrashed)
        {
            hasCrashed = true;
            Debug.Log("collision occurerd");
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

        // Animator animator = GetComponent<Animator>();
        // if (animator != null)
        // {
        //     animator.SetTrigger("Crash");
        // }

        // Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // if (rb != null)
        // {
        //     rb.velocity = Vector2.zero;
        //     rb.angularVelocity = 0f;
        //     rb.simulated = false; // Disable physics simulation
        // }
    }
}
