using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Button settingsButton; // Reference to the settings button
    public GameObject settingsPanel; // Reference to the settings panel
    public Button closeButton; // Reference to the close button

    void Start()
    {
        // Ensure the settings panel is initially disabled
        settingsPanel.SetActive(false);

        // Add listeners to the buttons
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        closeButton.onClick.AddListener(CloseSettingsPanel);
    }

    void ToggleSettingsPanel()
    {
        // Toggle the active state of the settings panel
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    void CloseSettingsPanel()
    {
        // Disable the settings panel
        settingsPanel.SetActive(false);
    }
}
