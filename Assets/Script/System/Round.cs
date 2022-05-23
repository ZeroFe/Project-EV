using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 라운드에 나올 몬스터 수 지정 및 나오는 타이밍 등 세부 정보를 담은 데이터
/// 특정 몬스터의 개수나 출현 개수 및 시간 조절을 담음
/// </summary>
[System.Serializable]
public class Round
{
    // 특정 몬스터에 대한 스폰 정보
    // 어떤 경로로, 어떤 몬스터가, 얼마나 생성될지
    [System.Serializable]
    public class EnemySpawn
    {
        public enum SpawnType
        {
            Sequence,
            Emission,
        }

        public GameObject routeObject;

        public SpawnType spawnType = SpawnType.Sequence;
        public GameObject enemyPrefab;
        public int count = 10;

        [Tooltip("언제부터 나올지")]
        public float startTime = 0.0f;
        [Header("Sequence Setting")]
        [Tooltip("몬스터가 나오는 주기")]
        public float period = 3.0f;
        public AnimationCurve periodScaleCurve;
        [Header("For Spawn")]
        // 디버그용으로 public으로 냅두고, 이후 HideInspector를 적용해서 안 보이게 한다
        public float spawnWaitTime = 0.0f;
        public int remainCount = 10;
    }

    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    [Header("Round Power Up Setting")]
    
    public float enemyPowerupRate = 1.2f;
    // 돌연변이 확률
    //public float enemyMutateChance = 0.0f;


}
