using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 라운드에 나올 몬스터 수 지정 및 나오는 타이밍 등 세부 정보를 담은 데이터
/// 특정 몬스터의 개수나 출현 개수 및 시간 조절을 담음
/// </summary>
[System.Serializable]
public class Round
{
    // 특정 몬스터에 대한 스폰 정보
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public enum SpawnType
        {
            // 하나씩,
            // 한 번에,
        }

        public GameObject enemyPrefab;
        public int spawnCount = 10;

        public float spawnInitTime = 0.0f;
        public float spawnBaseTime = 3.0f;
        public AnimationCurve spawnTimeScaleCurve;
    }

    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();

    [Header("Round Power Up Setting")]
    
    public float enemyPowerupRate = 1.2f;
    // 돌연변이 확률


}
