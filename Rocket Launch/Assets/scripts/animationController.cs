using UnityEngine;

public class RocketAnimationController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component

    private RocketLaunch rocketLaunch; // Reference to the RocketLaunch script

    void Start()
    {
        animator = GetComponent<Animator>(); // Assuming Animator is on the same GameObject
        rocketLaunch = GetComponent<RocketLaunch>(); // Assuming RocketLaunch is on the same GameObject
    }

    void Update()
    {
        if (rocketLaunch != null && rocketLaunch.launched)
        {
            animator.SetBool("IsLaunched", true); // Example: Trigger launch animation
        }
    }
}
