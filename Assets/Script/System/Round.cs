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
    // ���� ���� ���� ����
    // � ��η�, � ����, �󸶳� ��������
    [System.Serializable]
    public class EnemySpawn
    {
        public enum SpawnType
        {
            Sequence,
            Emission,
        }

        [Tooltip("���Ͱ� ��� ������")]
        public SpawnNotifier spawnPos;

        public SpawnType spawnType = SpawnType.Sequence;
        [Tooltip("� ���͸� ��������")]
        public GameObject enemyPrefab;
        [SerializeField, Tooltip("���� ���� ����")]
        private int count = 10;
        [Tooltip("�������� ������")]
        public float startTime = 0.0f;

        [Header("Sequence Setting")]
        [Tooltip("���Ͱ� ������ �ֱ�")]
        public float interval = 3.0f;
        [Tooltip("���� �ֱⰡ ���� �پ��� Ŀ�� ����")]
        public AnimationCurve intervalScaleCurve = AnimationCurve.EaseInOut(0.0f, 0.5f, 1.0f, 1.0f);

        [Header("Power Up Setting")]
        [Tooltip("���� �󸶳� ��ȭ����")]
        public float enemyPowerupRate = 1.2f;
        // �������� Ȯ��
        //public float enemyMutateChance = 0.0f;

        public int Count => count;
    }

    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    

}
