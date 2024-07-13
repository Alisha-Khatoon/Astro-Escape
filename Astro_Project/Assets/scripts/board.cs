using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState{
    wait, 
    move
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
    private bgTile[,] allTiles;
    public GameObject[,] allDots;
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
    private void DestroyMatchesAt(int column, int row){
        GameObject pieceToDestroy = allDots[column, row];
        if (pieceToDestroy != null && pieceToDestroy.GetComponent<dot>().isMatched){
        // Collect all dots to destroy
            List<GameObject> matchingDots = new List<GameObject>();
        
        // Horizontal check
        if (column > 1 && allDots[column - 1, row] != null && allDots[column - 2, row] != null &&
            allDots[column - 1, row].tag == pieceToDestroy.tag && allDots[column - 2, row].tag == pieceToDestroy.tag){
            matchingDots.Add(allDots[column - 1, row]);
            matchingDots.Add(allDots[column - 2, row]);
        }
        
        // Vertical check
        if (row > 1 && allDots[column, row - 1] != null && allDots[column, row - 2] != null &&
            allDots[column, row - 1].tag == pieceToDestroy.tag && allDots[column, row - 2].tag == pieceToDestroy.tag){
            matchingDots.Add(allDots[column, row - 1]);
            matchingDots.Add(allDots[column, row - 2]);
        }

        // Include the current dot itself
        matchingDots.Add(pieceToDestroy);

        // Destroy all matching dots
        foreach (GameObject dot in matchingDots){
            findMatches .currentMatches.Remove(allDots[column, row]);
            GameObject particle  = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            Destroy(dot);
            // Clear the array entry
            int dotColumn = Mathf.RoundToInt(dot.transform.position.x);
            int dotRow = Mathf.RoundToInt(dot.transform.position.y);
            allDots[dotColumn, dotRow] = null;
        }
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
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }

}
