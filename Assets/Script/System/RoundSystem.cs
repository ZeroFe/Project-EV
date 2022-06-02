using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

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
    [SerializeField] private GameObject WarningUI;
    [SerializeField]
    private float warningDuration = 0.5f;
    [SerializeField] private Image SafeImage;
    [SerializeField] private float safeDrawDuration = 0.5f;

    [Header("Sound")] 
    [SerializeField] private AudioClip RoundStartSound;

    [SerializeField] private AudioClip UpgradeStartSound;

    [Header("Game Clear")]
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Image victoryTextImage;
    [SerializeField] private Vector2 victoryAnimScales = new Vector2(1.5f, 2.5f);
    [SerializeField] private float victoryAnimDuration = 0.3f;
    [SerializeField] private CanvasGroup victoryButtonGroup;
    [SerializeField] private AudioClip victorySFX;
    [SerializeField] private AudioClip victoryBGM;

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
                //WindowSystem.Instance.OpenWindow(GameClearUI, false);
                GameClear();
            }
            // 아니면 업그레이드 띄우기
            else
            {
                Upgrade();
            }
        }
    }

    #region Battle

    // 다음 라운드 시작
    public void NextRound()
    {
        // 게임 시작 알림음
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySfxNonSpatial(RoundStartSound, 0.75f);

        WarningUI.SetActive(true);
        var rectTransform = WarningUI.GetComponent<RectTransform>();
        var group = WarningUI.GetComponent<CanvasGroup>();

        // Animation
        Sequence s = DOTween.Sequence();
        var origin = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(0, origin.y);
        group.alpha = 1.0f;
        s.Append(rectTransform.DOSizeDelta(origin, warningDuration));
        s.Append(group.DOFade(0.0f, warningDuration).SetLoops(4));

        s.onComplete = () =>
        {
            WarningUI.SetActive(false);

            currentRoundIndex++;
            // 리스트에서 라운드 정보를 읽고 시작한다
            CurrentRound = rounds[currentRoundIndex];

            SpawnStart();
        };
    }

    private void SpawnStart()
    {
        AudioManager.Instance.PlayBGM(AudioManager.BgmType.Battle);

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
        enemySpawn.spawnPos.NotifySpawn();

        if (enemySpawn.spawnType == Round.EnemySpawn.SpawnType.Sequence)
        {
            int remainCount = enemySpawn.Count;
            while (remainCount > 0)
            {
                CreateEnemy(enemySpawn);
                remainCount--;
                remainEnemyCount--;
                yield return new WaitForSeconds(enemySpawn.intervalScaleCurve.Evaluate(
                    (float)remainCount / enemySpawn.Count) * enemySpawn.interval);
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
        EnemyManager.Instance.InitEnemy(enemy, enemySpawn.spawnPos.gameObject);
        enemy.GetComponent<EnemyStatus>().PowerUp(enemySpawn.enemyPowerupRate);
        currentEnemyCount++;
    }

    #endregion

    #region Upgrade

    private void Upgrade()
    {
        // 라운드 끝남 알림음
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySfxNonSpatial(UpgradeStartSound, 0.75f);
        
        SafeImage.gameObject.SetActive(true);
        SafeImage.fillAmount = 0.0f;
        var group = SafeImage.GetComponent<CanvasGroup>();
        group.alpha = 1.0f;

        // Animation
        Sequence s = DOTween.Sequence();
        s.Append(SafeImage.DOFillAmount(1.0f, safeDrawDuration));
        s.Insert(safeDrawDuration + 4.0f, group.DOFade(0.0f, safeDrawDuration / 2));

        s.onComplete = () =>
        {
            // 업그레이드 할 수 있다 띄우고 윈도우 띄우기
            AudioManager.Instance.PlayBGM(AudioManager.BgmType.Upgrade);
            WindowSystem.Instance.OpenWindow(UpgradeWindow.Instance.gameObject, false);
            UpgradeWindow.Instance.SetUpgradeWindow(EnhanceSystem.Instance.GetRandomEnhances(3));
        };
    }

    #endregion

    #region Game Clear

    private void GameClear()
    {
        gameClearPanel.SetActive(true);
        var gameClearGroup = gameClearPanel.GetComponent<CanvasGroup>();

        Sequence s = DOTween.Sequence();
        //s.Append(victoryTextImage.transform.DOScale(victoryAnimScales.x, victoryAnimDuration).set)
    }

    #endregion

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
