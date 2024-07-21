using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public Transform rocket; // Rocket's transform to follow
    public Transform earth; // Earth's transform (if needed)
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement
    public Vector3 offset; // Offset from the rocket
    public float SwitchDistance = 10f;
    private bool followRocket = false; // Flag to switch between following rocket and covering earth

    void LateUpdate()
    {
        if (!followRocket)
        {
            // Cover the flat 2D earth image until the rocket leaves that background
            Vector3 desiredPosition = earth.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(earth);
        }
        else
        {
            // Follow the rocket
            Vector3 desiredPosition = rocket.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(rocket);
        }

        // Determine when to switch from covering earth to following the rocket
        if (!followRocket)
        {
            // Example condition: switch to follow rocket when rocket is Given units above earth's surface
            if (rocket.position.y > earth.position.y + SwitchDistance)
            {
                followRocket = true;
            }
        }
    }
}
