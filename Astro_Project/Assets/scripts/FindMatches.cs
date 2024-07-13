using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches(){
        currentMatches.Clear();
        StartCoroutine(FindAllMatchesCo());
    }
    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board.width; i++){
            for (int j = 0; j < board.height; j++){
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null){
                    if(i > 0 && i < board.width - 1){
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null){
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag){
                                if(currentDot.GetComponent<dot>().isRowBomb || leftDot.GetComponent<dot>().isRowBomb || rightDot.GetComponent<dot>().isRowBomb){
                                    currentMatches.Union(GetRowPieces(j));
                                } 
                                if(currentDot.GetComponent<dot>().isColumnBomb) {
                                    currentMatches.Union(GetcolumnPieces(i));
                                }
                                if(leftDot.GetComponent<dot>().isColumnBomb) {
                                    currentMatches.Union(GetcolumnPieces(i - 1));
                                }
                                if(rightDot.GetComponent<dot>().isColumnBomb) {
                                    currentMatches.Union(GetcolumnPieces(i + 1));
                                }

                                if(!currentMatches.Contains(leftDot)){
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<dot>().isMatched = true;
                                if(!currentMatches.Contains(rightDot)){
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<dot>().isMatched = true;
                                if(!currentMatches.Contains(currentDot)){
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<dot>().isMatched = true;
                            }
                        }
                    }
                    if( j > 0 && j < board.height - 1){
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if(upDot != null && downDot != null){
                            if(upDot.tag == currentDot.tag && downDot.tag == currentDot.tag){
                                if(currentDot.GetComponent<dot>().isColumnBomb || upDot.GetComponent<dot>().isColumnBomb || downDot.GetComponent<dot>().isColumnBomb){
                                    currentMatches.Union(GetcolumnPieces(i));
                                } 
                                if(currentDot.GetComponent<dot>().isRowBomb) {
                                    currentMatches.Union(GetRowPieces(j));
                                }  
                                if(upDot.GetComponent<dot>().isRowBomb) {
                                    currentMatches.Union(GetRowPieces(j + 1));
                                }  
                                if(downDot.GetComponent<dot>().isRowBomb) {
                                    currentMatches.Union(GetRowPieces(j - 1));
                                }  
                                if(!currentMatches.Contains(upDot)){
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<dot>().isMatched = true;
                                if(!currentMatches.Contains(downDot)){
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<dot>().isMatched = true;
                                if(!currentMatches.Contains(currentDot)){
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<dot>().isMatched = true;
                            }
                        }
                    }                      
                }

            }
        }

    }
    List<GameObject> GetcolumnPieces(int column){
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0; i< board.height; i++){
            if(board.allDots[column, i] != null){
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row){
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0; i< board.width; i++){
            if(board.allDots[i, row] != null){
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<dot>().isMatched = true;
            }
        }
        return dots;
    }
    
}
