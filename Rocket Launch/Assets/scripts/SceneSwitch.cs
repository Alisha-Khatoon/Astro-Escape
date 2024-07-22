using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneSwitch : MonoBehaviour
{
    public string sceneNameToLoad = "SampleScene"; // Default scene to load (adjust as needed)
    public GameObject rocket; // Reference to the GameObject with the Animator component
    
    private Animator rocketAnimator; // Reference to the Animator component
    
    // Dictionary to store cameras for each scene
    private Dictionary<string, GameObject[]> sceneCameras = new Dictionary<string, GameObject[]>();

    void Start()
    {
        rocketAnimator = rocket.GetComponent<Animator>();

        // Populate the dictionary with scene names and their respective cameras
        sceneCameras["SampleScene"] = new GameObject[] { GameObject.FindWithTag("Main Camera") };
        sceneCameras["shakeScene"] = new GameObject[] { GameObject.FindWithTag("Shake Camera"), GameObject.FindWithTag("Virtual Camera") };
        // Add entries for other scenes as needed
    }

    void Update()
    {
        // Check if the turn animation has completed
        if (rocketAnimator.GetCurrentAnimatorStateInfo(0).IsName("moveAhead") &&
            rocketAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            // Get the name of the current active scene
            Scene currentScene = SceneManager.GetActiveScene();
            string currentSceneName = currentScene.name;

            // Disable cameras of the current scene
            if (sceneCameras.ContainsKey(currentSceneName))
            {
                foreach (GameObject camera in sceneCameras[currentSceneName])
                {
                    camera.SetActive(false);
                }
            }

            // Load the specified scene
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);

            // Activate cameras for the new scene
            if (sceneCameras.ContainsKey(sceneNameToLoad))
            {
                foreach (GameObject camera in sceneCameras[sceneNameToLoad])
                {
                    camera.SetActive(true);
                }
            }
        }
    }
}
