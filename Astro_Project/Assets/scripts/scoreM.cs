using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class script : MonoBehaviour
{
    private Board board;
    public TextMeshProUGUI scoreText;
    public int score;
    public Image ScoreBar;

    void Start()
    {
        board = FindObjectOfType<Board>();
        UpdateBar();
    }

    void Update()
    {
        // Update the score text every frame
        if(scoreText != null){
            scoreText.text = "" + score.ToString(); // Use ToString() to convert int to string
    }
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        UpdateBar();
    }

    private void UpdateBar(){
        if(board != null && ScoreBar != null){
            int length = board.scoreGoals.Length;
            ScoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
    }
}
