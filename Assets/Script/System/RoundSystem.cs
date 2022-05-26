using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����׸� ���� �ǵ������� DisallowMultipleComponent�� ���� �ʴ´�
// ���� ����׿� Round�� �����ؾ��� ���� �ֱ� ����
public class RoundSystem : MonoBehaviour
{
    public static RoundSystem Instance { get; private set; }

    private static readonly float SPAWN_CHECK_TIME = 0.1f;
    
    public List<Round> rounds = new List<Round>();
    private int currentRoundIndex = -1;
    public Round CurrentRound
    {
        get;
        private set;
    }

    private int remainEnemyCount;

    [Header("UI")]
    public GameObject GameClearUI;

    private void Awake()
    {
        Debug.Assert(!Instance, "There is two or more Round System");
        Instance = this;
    }

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
        //Debug.Assert(remainEnemyCount >= 0 && current >= 0);

        remainEnemyCount--;
        Debug.Log("current enemy Count : " + remainEnemyCount);
        if (remainEnemyCount == 0 && CurrentRound.enemySpawns.Count == 0)
        {
            // ���� ������ üũ
            if (currentRoundIndex == rounds.Count - 1)
            {
                // ���� �������� ���� �Ϸ� ó��
                GameClearUI.SetActive(true);
            }
            // �ƴϸ� ���׷��̵� ����
            else
            {
                // ���� �� �ִϸ��̼�
                // ���׷��̵� �� �� �ִ� ���� ������ ����
                WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
                UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
            }
        }
    }

    // ���� ���� ����
    public void NextRound()
    {
        // ���� ���� �ִϸ��̼�


        currentRoundIndex++;
        // ����Ʈ���� ���� ������ �а� �����Ѵ�
        CurrentRound = rounds[currentRoundIndex];

        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        var enemySpawns = CurrentRound.enemySpawns;
        // ó�� ������ �� spawn Wait Time�� ���� �ð����� �����
        foreach (var spawn in enemySpawns)
        {
            spawn.spawnWaitTime = spawn.startTime;
            spawn.remainCount = spawn.Count;
        }

        // 
        while (CurrentRound.enemySpawns.Count > 0)
        {
            // ���� ���Ͱ� ���� ����� �����ؾ� �Ѵ�
            // ������ ��ȸ�� Remove�� ������� �� ������ ���� ���ɼ��� �����Ƿ� ������ ��ȸ�� ����Ѵ�
            for (int i = CurrentRound.enemySpawns.Count - 1; i >= 0; i--)
            {
                var currEnemySpawn = enemySpawns[i];
                if (currEnemySpawn.spawnWaitTime <= 0)
                {
                    // ���������� ����
                    if (currEnemySpawn.spawnType == Round.EnemySpawn.SpawnType.Sequence)
                    {
                        CreateEnemy(currEnemySpawn, CurrentRound.enemyPowerupRate);

                        currEnemySpawn.remainCount--;
                        currEnemySpawn.spawnWaitTime = 
                            currEnemySpawn.intervalScaleCurve.Evaluate((float)currEnemySpawn.remainCount / currEnemySpawn.Count) * 
                            currEnemySpawn.interval;
                    }
                    // �� ���� ����
                    else if (currEnemySpawn.spawnType == Round.EnemySpawn.SpawnType.Emission)
                    {
                        for (int j = 0; j < currEnemySpawn.Count; j++)
                        {
                            CreateEnemy(currEnemySpawn, CurrentRound.enemyPowerupRate);
                        }
                        currEnemySpawn.remainCount = 0;
                    }
                }

                if (currEnemySpawn.remainCount == 0)
                {
                    enemySpawns.RemoveAt(i);
                }
                else
                {
                    currEnemySpawn.spawnWaitTime -= SPAWN_CHECK_TIME;
                }
            }

            yield return new WaitForSeconds(SPAWN_CHECK_TIME);
        }

        Debug.Log("create end");
    }

    private void CreateEnemy(Round.EnemySpawn enemySpawn, float powerUp)
    {
        var enemy = Instantiate(enemySpawn.enemyPrefab);
        EnemyManager.Instance.InitEnemy(enemy, enemySpawn.routeObject);
        enemy.GetComponent<EnemyStatus>().PowerUp(powerUp);
        remainEnemyCount++;
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
