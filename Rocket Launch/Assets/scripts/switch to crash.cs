using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Switch : MonoBehaviour
{
    public string crashSceneName = "crashScene"; 
    public float shakeDuration = 5f; 

    private bool shakeSceneCompleted = false;

    void Start()
    {
        StartCoroutine(WaitAndSwitch());
    }

    IEnumerator WaitAndSwitch()
    {
        yield return new WaitForSeconds(shakeDuration);

        shakeSceneCompleted = true;

        SwitchToCrashScene();
    }

    public void SwitchToCrashScene()
    {
        if (shakeSceneCompleted)
        {
            SceneManager.LoadScene(crashSceneName);
        }
        else
        {
            Debug.LogWarning("Cannot switch to crashScene: camera shake duration not completed yet.");
        }
    }
}
