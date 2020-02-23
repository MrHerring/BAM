using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Controlles Canvas_UI
 * It's called by UI_Manager
 */

public class GameMenu : MonoBehaviour, IMenu
{
    [SerializeField]
    private Button _pauseButton;
    [SerializeField]
    private Sprite _pauseSpriteOn;
    [SerializeField]
    private Sprite _pauseSpriteOff;

    private Image _pauseButtonImage;

    void Awake()
    {
        _pauseButtonImage = _pauseButton.GetComponent<Image>();
    }

    public void Open()
    {
        _pauseButtonImage.sprite = _pauseSpriteOff;
        Time.timeScale = 1;
    }

    public void Close()
    {
        _pauseButtonImage.sprite = _pauseSpriteOn;
    }
}
