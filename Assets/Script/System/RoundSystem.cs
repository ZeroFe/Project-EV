using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : Singleton<RoundSystem>
{
    public int enemySpawnStartCount = 20;
    // ���帶�� �����Ѵ�
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
        // ���� ���� ����ŭ �����ؼ� �� �ȴ�
        Debug.Assert(remainEnemyCount < 0 || currentEnemyCount < 0);

        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // ���� ������ üũ
            // �ƴϸ� ���� �����
        }
    }
}
