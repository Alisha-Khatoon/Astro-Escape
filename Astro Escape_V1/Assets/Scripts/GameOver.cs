using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    private bool WaitingForRestart;
    void Update(){
    if (GameObject.FindGameObjectsWithTag("Player").Length == 0){
        gameOverPanel.SetActive(true);
        
    }
}

    public void Restart(){
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
}
}

