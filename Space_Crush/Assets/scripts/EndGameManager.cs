using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private int score = 0;
    private int targetScore = 2000;

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
                LoseGame();
            }
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score >= targetScore)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        fadePanelController fade = FindObjectOfType<fadePanelController>();
        fade.GameOver();
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2); // Optional: Add a delay before loading the next scene
        SceneManager.LoadScene("NextSceneName"); // Replace "NextSceneName" with the actual name of your next scene
    }

    public void LoseGame()
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
    fadePanelController fade = FindObjectOfType<fadePanelController>();
    fade.GameOver();
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
