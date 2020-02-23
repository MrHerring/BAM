using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implements IMenu for Simple Back button feature using Stack in MenuManager Class
 */
public class PauseMenu : MonoBehaviour, IMenu
{
    public void Open()
    {
        GameController.Instance.PauseGame();
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        GameController.Instance.ContinueGame();
        this.gameObject.SetActive(false);
    }
}
