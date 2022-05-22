using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���忡 ���� ���� �� ���� �� ������ Ÿ�̹� �� ���� ������ ���� ������
/// Ư�� ������ ������ ���� ���� �� �ð� ������ ����
/// </summary>
[System.Serializable]
public class Round
{
    // Ư�� ���Ϳ� ���� ���� ����
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public enum SpawnType
        {
            // �ϳ���,
            // �� ����,
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
    // �������� Ȯ��


}
