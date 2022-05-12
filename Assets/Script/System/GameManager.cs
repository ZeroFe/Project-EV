using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        //Debug.Log(GameManager);
        SetCursorDisplay(false);
    }

    public void SetCursorDisplay(bool display)
    {
        Time.timeScale = display ? 0 : 1;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }

    public void GameClear()
    {
        Debug.Log("Game Clear");
    }
}
