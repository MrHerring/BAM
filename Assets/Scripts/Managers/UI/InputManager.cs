using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (!GameController.Instance.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MenuManager.Instance.Back();
            }
        }
        
    }
}
