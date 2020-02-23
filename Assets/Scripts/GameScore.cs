using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    private static int _currentScore;
    private static int _highScore;
    private static bool _lvlCompleted;

    public static int CurrentScore
    {
        get
        {
            return _currentScore;
        }

        set
        {
            _currentScore = value;
        }
    }

    public static int HighScore
    {
        get
        {
            return _highScore;
        }

        set
        {
            _highScore = value; 
        }
    }

    public static bool LvlCompleted
    {
        get
        {
            return _lvlCompleted;
        }

        set
        {
            _lvlCompleted = value;
        }
    }
}
