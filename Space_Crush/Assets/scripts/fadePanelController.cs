using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadePanelController : MonoBehaviour
{
    public Animator panelAnim;
    public Animator gameInfoAnim;
    
    public void OK(){
        if(panelAnim != null && gameInfoAnim != null){
            panelAnim.SetBool("out", true);
            gameInfoAnim.SetBool("out", true);
            StartCoroutine(GameStartCo());
        }
    }
    public void GameOver(){
        panelAnim.SetBool("out", false);
        panelAnim.SetBool("GameOver", true);
        
    }
    IEnumerator GameStartCo(){
        yield return new WaitForSeconds(1f);
        Board board = FindObjectOfType<Board>();
        if(board.currentState != GameState.lose){            
            board.currentState = GameState.move;}
    }
}
