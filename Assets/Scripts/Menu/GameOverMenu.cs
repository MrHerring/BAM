using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Controlles GameOver_Canvas
 */
public class GameOverMenu : MonoBehaviour
{

    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _highScore;

    public void SetScores(int score, int highScore)
    {
        _score.text = score.ToString("0000");
        _highScore.text = highScore.ToString("0000");
    }

}
