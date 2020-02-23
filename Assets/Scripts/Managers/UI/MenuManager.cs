using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Controlls Canvas Changes in GameScene
 * Contains IMenu Stack for Symple Back button feature 
 * It remembers prev Canvas and knows where to go on Back button pressed
 */
public class MenuManager : MonoBehaviour
{
    private Stack<IMenu> _canvasStack;

    [SerializeField]
    private Canvas _defaultCanvas;
    
    private IMenu _currentMenu;

    [SerializeField]
    private Canvas _defaultBackCanvas;

    private static MenuManager _instance;
    public static MenuManager Instance
    {
        get
        {
            if (System.Object.ReferenceEquals(_instance, null))
            {
                _instance = GameObject.FindObjectOfType<MenuManager>();
                if (System.Object.ReferenceEquals(_instance, null))
                {
                    _instance = new GameObject("MenuManager").AddComponent<MenuManager>();
                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;

        _canvasStack = new Stack<IMenu>();
        _currentMenu = _defaultCanvas.GetComponent<IMenu>(); 
    }

    public void SwitchCanvas(Canvas canvasToOpen)  
    {
        _canvasStack.Push(_currentMenu);

        _currentMenu.Close();
        _currentMenu = canvasToOpen.GetComponent<IMenu>();

        _currentMenu.Open();
    }

    public void Back()
    {
        _currentMenu.Close();

        if (_canvasStack.Count == 0)
        {
            SwitchCanvas(_defaultBackCanvas);
        } 
        else
        {
            _currentMenu = _canvasStack.Pop();
        }

        _currentMenu.Open();
    }

}
