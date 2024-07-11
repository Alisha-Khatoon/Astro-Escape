using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private bgTile[,] allTiles;


    void Start(){
        allTiles = new bgTile[width, height];
        SetUp();
    }
    private void SetUp(){
        for (int i = 0; i< width; i++){
            for (int j = 0; j < height; j++){
                Vector2 tempPosition = new Vector2(i,j);
                Instantiate(tilePrefab, tempPosition, Quaternion.identity);
            }
        }

    }
}
