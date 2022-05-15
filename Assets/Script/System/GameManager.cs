using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        //Debug.Log(GameManager);
    }

    public void GameClear()
    {
        Debug.Log("Game Clear");
    }
}
