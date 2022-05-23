using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// ���忡 ���� ���� �� ���� �� ������ Ÿ�̹� �� ���� ������ ���� ������
/// Ư�� ������ ������ ���� ���� �� �ð� ������ ����
/// </summary>
[System.Serializable]
public class Round
{
    // Ư�� ���Ϳ� ���� ���� ����
    // � ��η�, � ���Ͱ�, �󸶳� ��������
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

        [Tooltip("�������� ������")]
        public float startTime = 0.0f;
        [Header("Sequence Setting")]
        [Tooltip("���Ͱ� ������ �ֱ�")]
        public float period = 3.0f;
        public AnimationCurve periodScaleCurve;
        [Header("For Spawn")]
        // ����׿����� public���� ���ΰ�, ���� HideInspector�� �����ؼ� �� ���̰� �Ѵ�
        public float spawnWaitTime = 0.0f;
        public int remainCount = 10;
    }

    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    [Header("Round Power Up Setting")]
    
    public float enemyPowerupRate = 1.2f;
    // �������� Ȯ��
    //public float enemyMutateChance = 0.0f;


}
