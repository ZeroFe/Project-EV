using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

// 디버그를 위해 의도적으로 DisallowMultipleComponent는 넣지 않는다
// 여러 디버그용 Round를 제작해야할 수도 있기 때문
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

    private int currentEnemyCount = 0;
    private int remainEnemyCount = 0;

    [Header("UI")] 
    [SerializeField] private Image WarningUI;
    [SerializeField] private Image SafeUI;
    public GameObject GameClearUI;

    private bool isWarningAnimation = false;
    private bool isSafeAnimation = false;

    [SerializeField]
    private GameObject spawnIndicatorPrefab;

    private void Awake()
    {
        Debug.Assert(!Instance, "There is two or more Round System");
        Instance = this;
    }

    public void Start()
    {
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
        Debug.Log($"remain enemy Count : {remainEnemyCount}, current enemy Count : " + currentEnemyCount);
        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // 게임 끝났나 체크
            if (currentRoundIndex == rounds.Count - 1)
            {
                // 게임 끝났으면 게임 완료 처리
                WindowSystem.Instance.OpenWindow(GameClearUI, false);
            }
            // 아니면 업그레이드 띄우기
            else
            {
                StartCoroutine(SafeAnimation());
            }
        }
    }

    // 다음 라운드 시작
    public void NextRound()
    {
        StartCoroutine(WarningAnimation());
    }

    IEnumerator WarningAnimation()
    {
        // 라운드 시작 애니메이션
        Color c = WarningUI.color;
        for (float alpha = 0; alpha < 1; alpha += 0.1f)
        {
            c.a = alpha;
            WarningUI.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        c.a = 1.0f;
        WarningUI.color = c;

        for (float alpha = 1.0f; alpha > 0; alpha -= 0.1f)
        {
            c.a = alpha;
            WarningUI.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        c.a = 0.0f;
        WarningUI.color = c;

        currentRoundIndex++;
        // 리스트에서 라운드 정보를 읽고 시작한다
        CurrentRound = rounds[currentRoundIndex];

        SpawnStart();
    }

    IEnumerator SafeAnimation()
    {
        // 라운드 끝 애니메이션
        Color c = SafeUI.color;
        for (float alpha = 0; alpha < 1; alpha += 0.1f)
        {
            c.a = alpha;
            SafeUI.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        c.a = 0.0f;
        SafeUI.color = c;

        for (float alpha = 1.0f; alpha > 0; alpha -= 0.1f)
        {
            c.a = alpha;
            SafeUI.color = c;
            yield return new WaitForSeconds(0.1f);
        }
        c.a = 0.0f;
        SafeUI.color = c;

        // 업그레이드 할 수 있다 띄우고 윈도우 띄우기
        WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
        UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
    }

    private void SpawnStart()
    {
        var enemySpawns = CurrentRound.enemySpawns;
        // 처음 시작할 때 spawn Wait Time을 시작 시간으로 맞춘다
        foreach (var enemySpawn in enemySpawns)
        {
            remainEnemyCount += enemySpawn.Count;

            StartCoroutine(SpawnMonsters(enemySpawn));
        }
    }

    private IEnumerator SpawnMonsters(Round.EnemySpawn enemySpawn)
    {
        yield return new WaitForSeconds(enemySpawn.startTime);
        print("Start Spawn");
        var spawnIndicator = Instantiate(spawnIndicatorPrefab);
        spawnIndicator.transform.position = enemySpawn.routeObject.transform.position;

        if (enemySpawn.spawnType == Round.EnemySpawn.SpawnType.Sequence)
        {
            int remainCount = enemySpawn.Count;
            while (remainCount > 0)
            {
                CreateEnemy(enemySpawn);
                remainCount--;
                remainEnemyCount--;
                yield return new WaitForSeconds(enemySpawn.intervalScaleCurve.Evaluate(
                    (float)enemySpawn.remainCount / enemySpawn.Count) * enemySpawn.interval);
            }
        }
        else if (enemySpawn.spawnType == Round.EnemySpawn.SpawnType.Emission)
        {
            remainEnemyCount -= enemySpawn.Count;
            for (int j = 0; j < enemySpawn.Count; j++)
            {
                CreateEnemy(enemySpawn);
            }
        }

        Debug.Log("create end");
    }

    private void CreateEnemy(Round.EnemySpawn enemySpawn)
    {
        var enemy = Instantiate(enemySpawn.enemyPrefab);
        EnemyManager.Instance.InitEnemy(enemy, enemySpawn.routeObject);
        enemy.GetComponent<EnemyStatus>().PowerUp(enemySpawn.enemyPowerupRate);
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
