using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    private List<GameObject> IsAdjacentBomb(dot dot1, dot dot2, dot dot3){
        List<GameObject> currentDots = new List<GameObject>();
        if(dot1.isAdjBomb){
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }   
        if(dot2.isAdjBomb){
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }
        if(dot3.isAdjBomb){
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot3.row));
        }
        return currentDots;
    }
    
    private List<GameObject> IsRowbomb(dot dot1, dot dot2, dot dot3){
        List<GameObject> currentDots = new List<GameObject>();
        if(dot1.isRowBomb){
            currentMatches.Union(GetRowPieces(dot1.row));
        }   
        if(dot2.isRowBomb){
            currentMatches.Union(GetRowPieces(dot2.row));
        }
        if(dot3.isRowBomb){
            currentMatches.Union(GetRowPieces(dot3.row));
        }
        return currentDots;
    }

    private List<GameObject> IsColumnbomb(dot dot1, dot dot2, dot dot3){
        List<GameObject> currentDots = new List<GameObject>();
        if(dot1.isColumnBomb){
            currentMatches.Union(GetColumnPieces(dot1.column));
        }   
        if(dot2.isColumnBomb){
            currentMatches.Union(GetColumnPieces(dot2.column));
        }
        if(dot3.isColumnBomb){
            currentMatches.Union(GetColumnPieces(dot3.column));
        }
        return currentDots;
    }

    private void AddToListAndMatch(GameObject dot){
        if(!currentMatches.Contains(dot)){
            currentMatches.Add(dot);
        }
        dot.GetComponent<dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1,GameObject dot2,GameObject dot3){
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }                              
                                
    private IEnumerator FindAllMatchesCo(){
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board.width; i++){
            for (int j = 0; j < board.height; j++){
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null){
                dot currentDotDot = currentDot.GetComponent<dot>();
                    if(i > 0 && i < board.width - 1){
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null){
                            dot rightDotDot = rightDot.GetComponent<dot>();
                            dot leftDotDot = leftDot.GetComponent<dot>();
                            if(leftDot != null && rightDot != null){
                                if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag){
                                    currentMatches.Union(IsRowbomb(leftDotDot, currentDotDot, rightDotDot));
                                    currentMatches.Union(IsColumnbomb(leftDotDot, currentDotDot, rightDotDot));
                                    currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));
                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                            }   
                        }
                    }
                    if( j > 0 && j < board.height - 1){
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if(upDot != null && downDot != null){
                            dot downDotDot = downDot.GetComponent<dot>();
                            dot upDotDot = upDot.GetComponent<dot>();
                            if(upDot != null && downDot != null){
                                if(upDot.tag == currentDot.tag && downDot.tag == currentDot.tag){
                                    currentMatches.Union(IsColumnbomb(upDotDot, currentDotDot, downDotDot));
                                    currentMatches.Union(IsRowbomb(upDotDot, currentDotDot, downDotDot));
                                    currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));
                                    GetNearbyPieces(upDot, currentDot, downDot);
                                }
                            }    
                        }
                    }                      
                }

            }
        }
    }
    public void MatchPiecesOfColor(string color){
        for(int i = 0; i < board.width; i++){
            for(int j = 0; j< board.height; j++){
                if(board.allDots[i,j] != null){
                    if(board.allDots[i, j].tag == color){
                        board.allDots[i,j].GetComponent<dot>().isMatched = true;
                    }
                }
            }
        }
    }
    List<GameObject> GetAdjacentPieces(int column, int row){
        List<GameObject> dots = new List<GameObject>();
        for(int i = column - 1; i <= column + 1; i++){
            for(int j = row - 1; j <= row + 1; j++){
                if(i >= 0 && i < board.width && j >= 0 && j < board.height){
                    if(board.allDots[i,j] != null){
                        dots.Add(board.allDots[i, j]);
                        board.allDots[i, j].GetComponent<dot>().isMatched = true; 
                    }
                }
            }
        }
        return dots;
    }


    List<GameObject> GetColumnPieces(int column){
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0; i< board.height; i++){
            if(board.allDots[column, i] != null){
                dot dot = board.allDots[column, i].GetComponent<dot>();
                if(dot.isRowBomb){
                    dots.Union(GetRowPieces(i)).ToList();
                }
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
                dot dot = board.allDots[i, row].GetComponent<dot>();
                if(dot.isColumnBomb){
                    dots.Union(GetColumnPieces(i)).ToList();
                }
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<dot>().isMatched = true;
            }
        }
        return dots;
    }
    public void CheckBombs(){
        if(board.currentDot != null){
            if(board.currentDot.isMatched){
                board.currentDot.isMatched = false;
                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135)){
                    board.currentDot.MakeRowBomb();
                }
                else{
                    board.currentDot.MakeColumnBomb();
                }
            }
            else if(board.currentDot.otherDot != null){
                dot otherDot = board.currentDot.otherDot.GetComponent<dot>();
                if(otherDot.isMatched){
                    otherDot.isMatched = false;                    
                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135)){
                    otherDot.MakeRowBomb();
                }
                else{
                    otherDot.MakeColumnBomb();
                }
                }

            }
        }
    }
}
