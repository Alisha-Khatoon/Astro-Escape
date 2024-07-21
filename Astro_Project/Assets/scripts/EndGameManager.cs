using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameType{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirements{
    public GameType gameType;
    public int counterValue;
}
public class EndGameManager : MonoBehaviour
{
    public GameObject moveLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;

    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float TimerSeconds;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        SetUpGame();
    }
    void SetUpGame(){
        currentCounterValue = requirements.counterValue;
        if(requirements.gameType == GameType.Moves){
            moveLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else{
            TimerSeconds = 1;
            moveLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }
    public void decreaseCounterValue(){
        if(board.currentState != GameState.pause){
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            if(currentCounterValue <= 0){
                LoseGame();
            }        
        }
    }
    public void WinGame(){
        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        fadePanelController fade = FindObjectOfType<fadePanelController>();
        fade.GameOver();
    }
  
  public void LoseGame(){
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("You Lose!");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        fadePanelController fade = FindObjectOfType<fadePanelController>();
        fade.GameOver();


  }
    // Update is called once per frame
    void Update()
    {
        if(requirements.gameType == GameType.Time && currentCounterValue > 0){
            TimerSeconds -= Time.deltaTime;
            if(TimerSeconds <= 0){
               decreaseCounterValue(); 
               TimerSeconds = 1;
            }
        }
    }

}
