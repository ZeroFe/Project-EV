using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : Singleton<RoundSystem>
{
    public int enemySpawnStartCount = 20;
    // 라운드마다 증가한다
    public int enemySpawnIncreaseCount = 5;

    private int currentEnemyCount;
    private int remainEnemyCount;

    [SerializeField]
    private int MaxRound = 10;
    private int currentRound = 0;

    [Header("UI")]
    public GameObject GameClearUI;

    public void CheckRoundEnd()
    {
        // 몹이 음수 개만큼 존재해선 안 된다
        Debug.Assert(remainEnemyCount < 0 || currentEnemyCount < 0);

        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // 게임 끝났나 체크
            // 아니면 다음 라운드로
        }
    }
}
