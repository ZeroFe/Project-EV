using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����׸� ���� �ǵ������� DisallowMultipleComponent�� ���� �ʴ´�
// ���� ����׿� Round�� �����ؾ��� ���� �ֱ� ����
public class RoundSystem : Singleton<RoundSystem>
{
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
            if (currentRoundIndex == rounds.Count)
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
            spawn.remainCount = spawn.count;
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
                        // ���� �ڵ�
                        currEnemySpawn.spawnWaitTime = 
                            currEnemySpawn.periodScaleCurve.Evaluate((float)currEnemySpawn.remainCount / currEnemySpawn.count) * 
                            currEnemySpawn.period;
                    }
                    // �� ���� ����
                    else if (currEnemySpawn.spawnType == Round.EnemySpawn.SpawnType.Emission)
                    {
                        // for�� ���� �ڵ�
                        currEnemySpawn.count = 0;
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

            // ���͸� óġ�ϸ� ���� ���� �� ���δ� - ����
            yield return new WaitForSeconds(SPAWN_CHECK_TIME);
        }

        Debug.Log("create end");
    }

    //private void Spawn()
    //{
    //    // �߰� �߰� ������ ���Ͱ� ���;� �Ѵ�
    //    Debug.Log("Monster Spawn!!!");
    //    int randNum = UnityEngine.Random.Range(0, enemyPrefabs.Length);
    //    var go = Instantiate(enemyPrefabs[randNum], spawnTr.position, Quaternion.identity);
    //    go.GetComponent<EnemyStatus>().PowerUp(currentEnemyPowerup);
    //    EnemyManager.Instance.SetEnemyTarget(go);
    //    currentEnemyCount++;
    //}

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
