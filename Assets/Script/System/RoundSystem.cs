using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class RoundSystem : Singleton<RoundSystem>
{
    private static readonly float SPAWN_CHECK_TIME = 0.1f;

    public int enemySpawnStartCount = 20;
    // 라운드마다 증가한다
    public int enemySpawnIncreaseCount = 5;
    
    private int currentEnemyCount;
    private int remainEnemyCount;

    [SerializeField] 
    private AnimationCurve spawnTimeCurve;
    [SerializeField] 
    private float spawnBaseTime = 5.0f;

    private float spawnWaitTime;

    // 임시 코드 : 지수승 방식 몬스터 강화
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
        // 임시 코드 : 첫 스테이지는 강화하지 않게 처리
        currentEnemyPowerup /= enemyPowerupRate;

        // 원래는 게임 시작 시 무기를 선택한 후, 라운드를 시작해야 함
        // 일단 간이 테스트로 시작하자마자 라운드 진행
        NextRound();
    }

    public void Update()
    {
        DebugExecute();
    }

    public void CheckRoundEnd()
    {
        // 몹이 음수 개만큼 존재해선 안 된다
        Debug.Assert(remainEnemyCount >= 0 && currentEnemyCount >= 0);

        currentEnemyCount--;
        Debug.Log("current enemy Count : " + currentEnemyCount);
        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // 게임 끝났나 체크
            if (currentRound == MaxRound)
            {
                // 게임 끝났으면 게임 완료 처리
                GameClearUI.SetActive(true);
            }
            // 아니면 업그레이드 띄우기
            else
            {
                WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
                UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
            }
        }
    }

    // 다음 라운드 시작
    public void NextRound()
    {
        currentRound++;

        remainEnemyCount = enemySpawnStartCount + currentRound * enemySpawnIncreaseCount;
        spawnWaitTime = spawnBaseTime;

        // 임시 강화
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
                // 초기엔 몬스터 스폰 간격이 넓어야 한다
                // 이후 마지막에 가까워질수록 스폰 시간을 줄인다
                // => 스폰 간격을 커브 식으로 조정
                spawnWaitTime = spawnTimeCurve.Evaluate((float)remainEnemyCount / totalSpawnMonster) * spawnBaseTime;
                Debug.Log("spawn wait time : " + spawnWaitTime);
                remainEnemyCount--;
                // 몬스터 스폰
                Spawn();
            }

            // 몬스터를 처치하면 스폰 간격 좀 줄인다 - 보류

            yield return new WaitForSeconds(SPAWN_CHECK_TIME);
            spawnWaitTime -= SPAWN_CHECK_TIME;
        }

        Debug.Log("create end");
    }

    private void Spawn()
    {
        // 중간 중간 설정한 몬스터가 나와야 한다
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
