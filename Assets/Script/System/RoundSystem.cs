using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class RoundSystem : Singleton<RoundSystem>
{
    private static readonly float SPAWN_CHECK_TIME = 0.1f;

    public int enemySpawnStartCount = 20;
    // ���帶�� �����Ѵ�
    public int enemySpawnIncreaseCount = 5;
    
    private int currentEnemyCount;
    private int remainEnemyCount;

    [SerializeField] 
    private AnimationCurve spawnTimeCurve;
    [SerializeField] 
    private float spawnBaseTime = 5.0f;

    private float spawnWaitTime;

    [SerializeField]
    private int MaxRound = 10;
    private int currentRound = 0;

    [Header("UI")]
    public GameObject GameClearUI;

    public void Start()
    {
        // ������ ���� ���� �� ���⸦ ������ ��, ���带 �����ؾ� ��
        // �ϴ� ���� �׽�Ʈ�� �������ڸ��� ���� ����
        NextRound();
    }

    public void Update()
    {
        DebugExecute();
    }

    public void CheckRoundEnd()
    {
        // ���� ���� ����ŭ �����ؼ� �� �ȴ�
        Debug.Assert(remainEnemyCount >= 0 && currentEnemyCount >= 0);

        currentEnemyCount--;
        Debug.Log("current enemy Count : " + currentEnemyCount);
        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // ���� ������ üũ
            if (currentRound == MaxRound)
            {
                // ���� �������� ���� �Ϸ� ó��
                GameClearUI.SetActive(true);
            }
            // �ƴϸ� ���׷��̵� ����
            else
            {
                WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
                UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
            }
        }
    }

    // ���� ���� ����
    public void NextRound()
    {
        currentRound++;

        remainEnemyCount = enemySpawnStartCount + currentRound * enemySpawnIncreaseCount;
        spawnWaitTime = spawnBaseTime;
        StartCoroutine(SpawnMonster());
    }

    private IEnumerator SpawnMonster()
    {
        int totalSpawnMonster = remainEnemyCount;
        while (remainEnemyCount > 0)
        {
            if (spawnWaitTime <= 0)
            {
                // �ʱ⿣ ���� ���� ������ �о�� �Ѵ�
                // ���� �������� ����������� ���� �ð��� ���δ�
                // => ���� ������ Ŀ�� ������ ����
                spawnWaitTime = spawnTimeCurve.Evaluate((float)remainEnemyCount / totalSpawnMonster) * spawnBaseTime;
                Debug.Log("spawn wait time : " + spawnWaitTime);
                remainEnemyCount--;
                // ���� ����
                Spawn();
            }

            // ���͸� óġ�ϸ� ���� ���� �� ���δ� - ����

            yield return new WaitForSeconds(SPAWN_CHECK_TIME);
            spawnWaitTime -= SPAWN_CHECK_TIME;
        }

        Debug.Log("create end");
    }

    private void Spawn()
    {
        // �߰� �߰� ������ ���Ͱ� ���;� �Ѵ�
        Debug.Log("Monster Spawn!!!");
        currentEnemyCount++;
    }

    #region Debug

    private void DebugExecute()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CheckRoundEnd();
        }
    }

    #endregion
}
