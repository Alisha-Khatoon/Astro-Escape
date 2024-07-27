using UnityEngine;

public class ToggleSettingsPanel : MonoBehaviour
{
    public GameObject settingsPanel;

    public void TogglePanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void ClosePanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
