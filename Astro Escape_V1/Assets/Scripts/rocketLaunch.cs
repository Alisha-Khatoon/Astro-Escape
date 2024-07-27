using UnityEngine;

public class RocketLaunch : MonoBehaviour
{
    public float launchForce = 10f; // Initial launch force
    public float rotationSpeed = 5f; // Rotation speed during launch
    // public float torqueDirection = 1f; // Direction of torque (1 or -1)

    private Rigidbody2D rb;
    public bool launched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !launched)
        {
            LaunchRocket();
            launched = true;
        }
    }

    void LaunchRocket()
    {
        rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        // rb.AddTorque(torqueDirection * rotationSpeed, ForceMode2D.Impulse); // Apply torque sideways
    }
}
