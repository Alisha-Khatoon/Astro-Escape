using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private bgTile[,] allTiles;
    public GameObject[,] allDots;


    void Start(){
        allTiles = new bgTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }
    private void SetUp(){
        for (int i = 0; i< width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i,j);
                GameObject bgTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                bgTile.transform.parent = this.transform;
                bgTile.name = "(" + i + ", " + j + ")";
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + ")";
                allDots[i, j] = dot;

            }
        }

    }
}
