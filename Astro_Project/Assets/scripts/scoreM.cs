using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class script : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;

    void Start()
    {
        
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
    }
}
