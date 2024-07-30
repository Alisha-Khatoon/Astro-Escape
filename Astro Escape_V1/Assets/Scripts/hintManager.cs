using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
    }

    // Update is called once per frame
    void Update()
    {
       hintDelaySeconds -= Time.deltaTime;
       if(hintDelaySeconds <= 0 && currentHint == null)
       {
        Markhint();
        hintDelaySeconds = hintDelay;
       } 
    }
    List<GameObject> FindAllMatches(){
        List<GameObject> possibleMoves = new List<GameObject>();
        for(int i = 0; i < board.width; i++){
            for(int j = 0; j< board.height; j++){
                if(board.allDots[i, j] != null){
                    if(i < board.width - 1){
                        if(board.SwitchAndCheck(i, j, Vector2.right)){
                            possibleMoves.Add(board.allDots[i, j]);
                        }
                    }
                    if(j < board.height - 1){
                        if(board.SwitchAndCheck(i, j, Vector2.up)){
                            possibleMoves.Add(board.allDots[i, j]);
                        }
                    }
                }
            }
        }
        return possibleMoves;
        }

    GameObject PickOneRandomly(){
        List<GameObject> possibleMoves= new List<GameObject>();
        possibleMoves = FindAllMatches();
        if(possibleMoves.Count > 0){
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }

    private void Markhint(){
        GameObject move = PickOneRandomly();
        if(move != null){
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }

    public void DestroyHint(){
        if(currentHint != null){
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }

}


