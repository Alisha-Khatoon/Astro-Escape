using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Import UI namespace

public class NewBehaviourScript : MonoBehaviour
{
    public AudioSource audioSource;
    public Dropdown graphicsDropdown; // Add a public Dropdown field

    private void Start()
    {
        // Populate dropdown with quality level names
        if (graphicsDropdown != null)
        {
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
