using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

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
        Debug.Log($"remain enemy Count : {remainEnemyCount}, current enemy Count : " + currentEnemyCount);
        if (remainEnemyCount == 0 && currentEnemyCount == 0)
        {
            // ���� ������ üũ
            if (currentRoundIndex == rounds.Count - 1)
            {
                // ���� �������� ���� �Ϸ� ó��
                WindowSystem.Instance.OpenWindow(GameClearUI, false);
            }
            // �ƴϸ� ���׷��̵� ����
            else
            {
                StartCoroutine(SafeAnimation());
            }
        }
    }

    // ���� ���� ����
    public void NextRound()
    {
        StartCoroutine(WarningAnimation());
    }

    IEnumerator WarningAnimation()
    {
        // ���� ���� �ִϸ��̼�
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
        // ����Ʈ���� ���� ������ �а� �����Ѵ�
        CurrentRound = rounds[currentRoundIndex];

        SpawnStart();
    }

    IEnumerator SafeAnimation()
    {
        // ���� �� �ִϸ��̼�
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

        // ���׷��̵� �� �� �ִ� ���� ������ ����
        WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
        UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
    }

    private void SpawnStart()
    {
        var enemySpawns = CurrentRound.enemySpawns;
        // ó�� ������ �� spawn Wait Time�� ���� �ð����� �����
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
