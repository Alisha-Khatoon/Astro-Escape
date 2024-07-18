using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blankgoal{
    int score;
}
public class GoalManager : MonoBehaviour
{
    public Blankgoal levelGoals;
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
