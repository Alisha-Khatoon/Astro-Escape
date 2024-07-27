using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import UnityEngine.UI namespace for standard Text

public enum GameType
{
    Moves
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public GameObject moveLabel;
    public GameObject tryAgainPanel;
    public Text counter; // Standard Text component for counter
    public TextMeshProUGUI scoreText; // TextMeshProUGUI component to display the score

    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private int score = 0;
    private int targetScore = 100; // The score required to win the game

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        SetUpGame();
    }

    void SetUpGame()
    {
        currentCounterValue = 15; // Initialize moves to 15
        if (requirements.gameType == GameType.Moves)
        {
            moveLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
        UpdateScoreText();
    }

    public void DecreaseCounterValue()
    {
        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            Debug.Log("Current Counter Value: " + currentCounterValue); // Debug log
            if (currentCounterValue <= 0)
            {
                CheckGameEndCondition();
            }
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        Debug.Log("Increased Score: " + score); // Debug log
        UpdateScoreText();
        if (score >= targetScore)
        {
            Debug.Log("Score reached target: " + score); // Debug log
            WinGame(); // Call the WinGame method when the target score is reached
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void CheckGameEndCondition()
    {
        if (score >= targetScore)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    public void WinGame()
    {
        Debug.Log("LoseGame called"); // Debug log
        tryAgainPanel.SetActive(true);
        Debug.Log("tryAgainPanel should be active now"); // Debug log

        // Deactivate the board and its objects
        if (board != null)
        {
            board.gameObject.SetActive(false); // Assuming `board` is a GameObject or contains all board elements
        }

        board.currentState = GameState.lose;
        Debug.Log("You Lose!");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
    }

    public void LoseGame()
    {
        
        Debug.Log("WinGame called. Loading next scene..."); // Debug log
        board.currentState = GameState.win;
        // Stop the game by deactivating all relevant game objects
        if (board != null)
        {
            board.gameObject.SetActive(false);
        }
        // Load the next scene immediately
        SceneManager.LoadScene("Space_Quiz");
    }

    // Restart the game by reloading the current scene
    public void RestartGame()
    {
        Debug.Log("RestartGame called"); // Debug log
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        // No time component to update
    }
}
