using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Singleton class
 * Controlls all components on Canvas_UI
 * Listenter in Event System
 */
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _timerText;

    [SerializeField]
    private Text _currentScoreText;

    [SerializeField]
    private Text _highScoreText;

    [SerializeField]
    private Text[] _bonusText;

    private int _currentBonusIndex;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    _instance = new GameObject("UIManager").AddComponent<UIManager>();
                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        _currentBonusIndex = 0;
    }

    private void Start()
    {
        GameController.OnEnemyDestroy += UpdateScore;
        _currentScoreText.text = GameScore.CurrentScore.ToString("0000");
    }

    public void SetTimer(float startTime)
    {
        _timerText.text = startTime.ToString("00");
    }

    public void UpdateTime(float timeLeft)
    {
        _timerText.text = timeLeft.ToString("00");
    }

    public void UpdateScore(int currentScoreValue)
    {
        _currentScoreText.text = currentScoreValue.ToString("0000");
    }

    public void SetHighScore(int highScore)
    {
        _highScoreText.text = highScore.ToString("0000");
    }

    public void SetInitBonusText()
    {
        _bonusText[_currentBonusIndex].gameObject.SetActive(false);
        _currentBonusIndex = 0;
    }

    public void SetBonusText(int index)
    {
        _currentBonusIndex = index;
        _bonusText[index - 1].gameObject.SetActive(false);
        _bonusText[index].gameObject.SetActive(true);
    } 

    //Bug fix
    private void OnDestroy()
    {
        GameController.OnEnemyDestroy -= UpdateScore;
    }
}
