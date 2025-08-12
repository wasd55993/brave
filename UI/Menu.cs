using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    //新游戏按钮
    public GameObject NewGameButton;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(NewGameButton);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
