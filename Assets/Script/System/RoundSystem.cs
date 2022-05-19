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

    // �ӽ� �ڵ� : ������ ��� ���� ��ȭ
    [SerializeField] private float enemyPowerupRate = 1.2f;
    private float currentEnemyPowerup = 1.0f;

    [SerializeField]
    private int MaxRound = 10;
    private int currentRound = 0;

    [Header("Enemy Prefab")] 
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private Transform spawnTr;

    [Header("UI")]
    public GameObject GameClearUI;

    public void Start()
    {
        // �ӽ� �ڵ� : ù ���������� ��ȭ���� �ʰ� ó��
        currentEnemyPowerup /= enemyPowerupRate;

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

        // �ӽ� ��ȭ
        currentEnemyPowerup *= enemyPowerupRate;

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
        int randNum = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        var go = Instantiate(enemyPrefabs[randNum], spawnTr.position, Quaternion.identity);
        go.GetComponent<EnemyStatus>().PowerUp(currentEnemyPowerup);
        EnemyManager.Instance.SetEnemyTarget(go);
        currentEnemyCount++;
    }

    #region Debug

    private void DebugExecute()
    {
        // Round Skip
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
        }
    }

    #endregion
}
