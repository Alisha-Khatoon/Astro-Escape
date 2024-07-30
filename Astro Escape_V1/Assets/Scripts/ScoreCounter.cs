using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private float score;

    void Start()
    {
        score = 0;
        Debug.Log("ScoreManager initialized. Score reset to 0.");
        UpdateScoreText();
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            score += 1 * Time.deltaTime;
            UpdateScoreText();

            if (score >= 20)
            {
                StopGame();
            }
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = ((int)score).ToString();
    }

    void StopGame()
    {
        Debug.Log("Score reached 60. Stopping game.");
        SceneManager.LoadScene("Final_Scene");
    }

}
