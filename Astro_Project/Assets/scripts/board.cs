using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum GameState{
    wait, 
    move
}

public enum TileKind{
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class TileType{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    public TileType[] boardLayout;
    private bgTile[,] allTiles;
    public GameObject[,] allDots;
    public dot currentDot;
    private FindMatches findMatches;


    void Start(){
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new bgTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }
    private void SetUp(){
        for (int i = 0; i< width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i,j + offSet);
                GameObject bgTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                bgTile.transform.parent = this.transform;
                bgTile.name = "(" + i + ", " + j + ")";
                int dotToUse = Random.Range(0, dots.Length);
                
                int maxIterations = 0;
                while(MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100){
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<dot>().row = j;
                dot.GetComponent<dot>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + ")";
                allDots[i, j] = dot;
            }
        }   
    }  
    private bool MatchesAt(int column, int row, GameObject piece){
        if (column > 1 && allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag){
            return true; // Horizontal match of 3 or more
        }
        if (row > 1 && allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag){
            return true; // Vertical match of 3 or more
        }
        return false;
    }
    private bool ColumnOrRow(){
        int numberHorizontal = 0;
        int numberVertical = 0;
        dot firstPiece = findMatches.currentMatches[0].GetComponent<dot>();
        if (firstPiece != null){
            foreach(GameObject currentPiece in findMatches.currentMatches){
                dot dot = currentPiece.GetComponent<dot>();
                if(dot.row == firstPiece.row){
                    numberHorizontal++;
                }
                if(dot.column == firstPiece.column){
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void checkToMakeBombs(){
        if(findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7){
            findMatches.CheckBombs();
        }
        if(findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8){
            if(ColumnOrRow()){
                if(currentDot != null){
                    if(currentDot.isMatched){
                        if(!currentDot.isColorBomb){
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else{
                        if(currentDot.otherDot != null){
                            dot otherDot = currentDot.otherDot.GetComponent<dot>();
                            if(otherDot.isMatched){
                                if(!otherDot.isColorBomb){
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }  
                    }
                }
            }
            else{
                if(currentDot != null){
                    if(currentDot.isMatched){
                        if(!currentDot.isAdjBomb){
                            currentDot.isMatched = false;
                            currentDot.MakeAdjBomb();
                        }
                    }
                    else{
                        if(currentDot.otherDot != null){
                            dot otherDot = currentDot.otherDot.GetComponent<dot>();
                            if(otherDot.isMatched){
                                if(!otherDot.isAdjBomb){
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjBomb();
                                }    
                            }
                        }  
                    }
                }   
            }
        }
    }
    private void DestroyMatchesAt(int column, int row){
        if(allDots[column, row].GetComponent<dot>().isMatched){
            if(findMatches.currentMatches.Count >= 4){
                checkToMakeBombs();
            }
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }

    }
    public void DestroyMatches(){
    for(int i = 0; i < width; i++){
        for (int j = 0; j < height; j++){
            if (allDots[i, j] != null && allDots[i, j].GetComponent<dot>().isMatched){
                DestroyMatchesAt(i, j);
            }
        }
    }
    findMatches.currentMatches.Clear();
    StartCoroutine(DecreaseRowCo());
}
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for(int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                if (allDots[i, j] == null){
                    nullCount++;
                }else if (nullCount > 0){
                    allDots[i, j].GetComponent<dot>().row -= nullCount;
                    allDots[i, j - nullCount] = allDots[i, j];
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        } 
        yield return new WaitForSeconds(.2f);
        StartCoroutine(FillBoardCo());
    }
    private void Refillboard(){
        for( int i = 0; i< width; i++){
            for(int j = 0; j< height; j++){
                if (allDots[i,j] == null){
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<dot>().row = j;
                    piece.GetComponent<dot>().column = i;                }
            }
        }
    }
    private bool MatchesOnBoard(){
        for( int i = 0; i< width; i++){
            for(int j = 0; j< height; j++){
                if(allDots[i, j] != null){
                    if(allDots[i,j].GetComponent<dot>().isMatched){
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo(){
        Refillboard();
        yield return new WaitForSeconds(.5f);
        while(MatchesOnBoard()){
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
