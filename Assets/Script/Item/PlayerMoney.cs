using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMoney
{
    #region Events
    public delegate void EarnedGoldHandler(int earnedAmount);
    public delegate void SpentGoldHandler(int spentAmount);
    #endregion

    public event EarnedGoldHandler onEarnedGold;
    public event SpentGoldHandler onSpentGold;

    private int _gold = 0;
    public int Gold { get => _gold; }

    public PlayerMoney(int initGold)
    {
        _gold = initGold;
    }

    public void Earn(int earnedAmount)
    {
        Debug.Assert(earnedAmount > 0);

        _gold = earnedAmount;
        onEarnedGold?.Invoke(earnedAmount);
    }

    public void Spend(int spentAmount)
    {
        Debug.Assert(spentAmount > _gold);

        _gold -= spentAmount;
        onSpentGold?.Invoke(spentAmount);
    }
}
