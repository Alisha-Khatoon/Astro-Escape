using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
public class dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private hintManager hintManager;
    private FindMatches findMatches;
    private Board board;
    public GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    [Header("Swipe stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Power Stuff")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjBomb;
    public GameObject adjMarker;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;

    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjBomb = false;
        hintManager = FindObjectOfType<hintManager>();

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        }

    //for testing and debug only
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1)){
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;

        }
    }
    void Update()
    {
        /*
        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        if (isMatched)
        {
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        */
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
            board.currentState = GameState.move;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
            board.currentState = GameState.move;
        }
    }

    public IEnumerator CheckMoveCo(){
        if(isColorBomb){
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }
        else if(otherDot.GetComponent<dot>().isColorBomb){
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<dot>().isMatched = true;
        }        
        yield return new WaitForSeconds(.5f);
        if (otherDot!= null){
            if(!isMatched && !otherDot.GetComponent<dot>().isMatched){
                otherDot.GetComponent<dot>().row = row;
                otherDot.GetComponent<dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }else{
                board.DestroyMatches();
        }
            //otherDot = null;
        }
    }

    private void OnMouseDown()
    {
        if(hintManager != null){
            hintManager.DestroyHint();
        }
        if( board.currentState == GameState.move){
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if( board.currentState == GameState.move){
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist){
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentDot = this;
        }else{
            board.currentState = GameState.move;
        }
    }

    void MovePiecesActual(Vector2 direction)
    {
    // Calculate target position
    int targetColumn = column + (int)direction.x;
    int targetRow = row + (int)direction.y;

    // Check if target position is within bounds
    if (targetColumn >= 0 && targetColumn < board.width && targetRow >= 0 && targetRow < board.height){
        otherDot = board.allDots[targetColumn, targetRow];
        previousRow = row;
        previousColumn = column;
        if (otherDot != null)
        {
            otherDot.GetComponent<dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<dot>().row += -1 * (int)direction.y;
        }
        column = targetColumn;
        row = targetRow;
        StartCoroutine(CheckMoveCo());
        }
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //right Swipe
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up Swipe
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //left swipe
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            MovePiecesActual(Vector2.down);
        }
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if( leftDot1 != null && rightDot1 != null){
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<dot>().isMatched = true;
                    rightDot1.GetComponent<dot>().isMatched = true;
                        isMatched = true;
                   
                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if( upDot1 != null && downDot1 != null){
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<dot>().isMatched = true;
                    downDot1.GetComponent<dot>().isMatched = true;
                    if(!isColorBomb){
                        isMatched = true;
                    }
                }
            }
        }
    }
    public void MakeRowBomb(){
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
    public void MakeColumnBomb(){
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
    public void MakeColorBomb(){
        isColorBomb = true;
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
        this.gameObject.tag = "Color";
    }
    public void MakeAdjBomb(){
        isAdjBomb = true;
        GameObject marker = Instantiate(adjMarker, transform.position, Quaternion.identity);
        marker.transform.parent = this.transform;
    }
}



