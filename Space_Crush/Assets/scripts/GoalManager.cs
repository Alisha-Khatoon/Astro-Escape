using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blankgoal{
    public int numberNeeded;
    public int numberCollected;
    public string matchValue;
    public Sprite goalsprite;
}
public class GoalManager : MonoBehaviour
{
    public Blankgoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    private EndGameManager endGame;

    // Start is called before the first frame update
    void Start()
    {
        endGame = FindObjectOfType<EndGameManager>();
        SetIntroGoals();   
    }

    void SetIntroGoals(){
        for( int i = 0; i< levelGoals.Length; i++){
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform, false);
            
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalsprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
           
            GameObject gamegoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gamegoal.transform.SetParent(goalGameParent.transform);
            panel = gamegoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalsprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
           
        }
    }
    public void UpdateGoals(){
        int goalsCompleted = 0;
        for(int i = 0; i < levelGoals.Length; i++){
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if(levelGoals[i].numberCollected >= levelGoals[i].numberNeeded){
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded;
            }
        }
        if (goalsCompleted >= levelGoals.Length){
            if(endGame != null){
                endGame.WinGame();
            }
            Debug.Log("You Win!");
        }
    }
    public void CompareGoal(string goalToCompare){
        for(int i = 0; i< levelGoals.Length; i++){
            if(goalToCompare == levelGoals[i].matchValue){
                levelGoals[i].numberCollected++;
            }
        }
    }
}
