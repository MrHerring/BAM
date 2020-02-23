using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogicLvl2 : ALevelLogic
{
    
    public float LvlTime { get => lvlTime; set => lvlTime = value; }

    private void Start()
    {
        lvlTime = 30f;
    }

    //  0 : game in progress
    //  1 : level complete
    // -1 : game over 
    public override int CheckGameState(float time)
    {
        if (time <= 0)
        {
            return -1;
        }
        else if (GameScore.CurrentScore > 2000)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
