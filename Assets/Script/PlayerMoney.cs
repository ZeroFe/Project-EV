using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney
{
    #region Events
    public delegate void EarnGoldHandler(int earnedGold);
    #endregion

    public event EarnGoldHandler onEarnGold;

    public int gold = 0;

    public PlayerMoney(int initGold)
    {
        gold = initGold;
    }

    public void AddGold()
    {

    }

    public void SubGold()
    {

    }
}
