using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
   *Singleton Class

   * Contains All Game Logic

   * Contains Custom Event System For Score Update Notification
     Observer : this object, Listeners: UIManager, EnemySpawnManager
*/

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Canvas _gameOverCanvas;

    [SerializeField]
    private Canvas _uiCanvas;

    [SerializeField]
    private Canvas _levelCompletedCanvas;

    private float _timeLeft;

    public bool gameOver { get; set; }

    private int _bonusCnt;

    [SerializeField]
    private int _bonusCntLimit;
    private int _resetBonusCntLimit;

    [SerializeField]
    private float _bonusTimeAmount;

    private int _scoreBonus;

    [SerializeField]
    private int _maxBonus;

    [SerializeField]
    private ALevelLogic _lvlLogic;

    //Observer Pattern
    public static event Action<int> OnEnemyDestroy;

    
    //Singleton Pattern
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<GameController>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    _instance = new GameObject("GameController").AddComponent<GameController>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        PrefabPoolingSystem.Reset();

        _resetBonusCntLimit = _bonusCntLimit;

        StartGame();
    }

    private void Update()
    {

        int gameState = _lvlLogic.CheckGameState(_timeLeft);

        if (gameState == 0 && !gameOver)
        {
            _timeLeft -= Time.deltaTime;
            UIManager.Instance.UpdateTime(_timeLeft);
        }
        else if (gameState == 1 && !gameOver)
        {
            LevelComplete();
        }
        else if (gameState == -1 && !gameOver)
        {
            EndGame();
        }
    }

    private void InitFields()
    {
        gameOver = false;

        _timeLeft = _lvlLogic.GetlvlTime();
        _bonusCnt = 0;

        _scoreBonus = 1;

        if (!GameScore.LvlCompleted)
        {
            GameScore.CurrentScore = 0;
        }

        GameScore.LvlCompleted = false;
        
        Time.timeScale = 1;

        _bonusCntLimit = _resetBonusCntLimit;

        UIManager.Instance.SetInitBonusText();

        EnemyCurvesBehavior.EnableOnTap();

        
    }


    /*
     * init all states for second scene load
     * UpdateScore(0) : for UI refresh before first enemy destory
     */
    private void StartGame()
    {
        InitFields();
        UIManager.Instance.SetTimer(_timeLeft);
    }

    /*
     * Called by PauseManuScript : Open() method
     */
    public void PauseGame()
    {
        Time.timeScale = 0;
        EnemyCurvesBehavior.DisableOnTap();
    }

    /*
     * Back from Pause
     */
    public void ContinueGame()
    {
        Time.timeScale = 1;
        EnemyCurvesBehavior.EnableOnTap();
    }

    /*
     * Enemy class calls this method on destroy
     * It uses event system
     */
    public void UpdateScore(int increment)
    {
        GameScore.CurrentScore += (_scoreBonus * increment);

        _bonusCnt++;

        if (_bonusCnt == _bonusCntLimit)
        {
            _timeLeft += _bonusTimeAmount;
            _bonusCnt = 0;
        }

        OnEnemyDestroy(GameScore.CurrentScore);
    }

    public void TimePenalty(float time)
    {
        _timeLeft -= time;
    }

    public void ResetBonus()
    {
        _bonusCnt = 0;
    }

    /*
     * Called by UI button Button_Replay in Canvas_GameOver
     * OnClick() Unity Event 
     */
    public void ReplayGame()
    {
        PrefabPoolingSystem.ClearAll();

        InitFields();

        UIManager.Instance.UpdateScore(GameScore.CurrentScore);

        _gameOverCanvas.gameObject.SetActive(false);
        _uiCanvas.gameObject.SetActive(true);
    }

    /*
     * Called from Update()
     * Opend UI that Contains Home and Replay Button
    */
    private void EndGame()
    {
        gameOver = true;

        PauseGame();

        _gameOverCanvas.GetComponent<GameOverMenu>().SetScores(GameScore.CurrentScore, GameScore.HighScore);

        _uiCanvas.gameObject.SetActive(false);
        _gameOverCanvas.gameObject.SetActive(true);
    }

    /*
     * Called from Upadate()
     * Opens UI that Contains Button for next level load
    */
    private void LevelComplete()
    {
        gameOver = true;

        PauseGame();

        GameScore.LvlCompleted = true;

        _uiCanvas.gameObject.SetActive(false);
        _levelCompletedCanvas.gameObject.SetActive(true);
    }

    public void Bonus()
    {
        if (_scoreBonus < _maxBonus)
        {
            UIManager.Instance.SetBonusText(_scoreBonus);
            _scoreBonus++;
        }  

        if (_bonusCntLimit < 9)
        {
            _bonusCntLimit++;
        }
    }
}
