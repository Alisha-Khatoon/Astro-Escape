using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    void Update()
    {
        
    }

    public void Pause1(){
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue(){
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
