using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSettings : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject graphicsDropdownObject;
    public Toggle fullscreenToggle; // Add this line for the fullscreen toggle
    private Dropdown graphicsDropdown;

    private void Start()
    {
        // Retrieve the Dropdown component from the GameObject
        if (graphicsDropdownObject != null)
        {
            graphicsDropdown = graphicsDropdownObject.GetComponent<Dropdown>();

            if (graphicsDropdown != null)
            {
                // Populate dropdown with quality level names
                graphicsDropdown.ClearOptions();
                List<string> options = new List<string>();
                for (int i = 0; i < QualitySettings.names.Length; i++)
                {
                    options.Add(QualitySettings.names[i]);
                }
                graphicsDropdown.AddOptions(options);

                // Set the current quality level as selected
                graphicsDropdown.value = QualitySettings.GetQualityLevel();
                graphicsDropdown.RefreshShownValue();
            }
            else
            {
                Debug.LogError("No Dropdown component found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("graphicsDropdownObject is not assigned.");
        }

        // Initialize fullscreen toggle
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
        }
        else
        {
            Debug.LogError("FullscreenToggle is not assigned.");
        }
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Volume set to: " + volume);
        audioSource.volume = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Quality set to: " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Current quality level: " + QualitySettings.GetQualityLevel());
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Debug.Log("Fullscreen set to: " + isFullscreen);
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen state: " + Screen.fullScreen);
    }
}
