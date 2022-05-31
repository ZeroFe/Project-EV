using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �� �Ա����� ���� �����ϸ� ��� ���� �˸��� ����
/// </summary>
public class SpawnNotifier : MonoBehaviour
{
    [Header("Wall Light")]
    // ���� ���� 
    private Light[] wallLights;
    [SerializeField]
    private Color warnColor = Color.red;
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField] private float duration = 0.5f;
    private float halfDuration;
    [SerializeField] private Vector2 intensities;

    [Header("Others")]
    [SerializeField, Tooltip("�̴ϸʿ� ���� ���������� �˸��� ǥ�� ")] 
    private GameObject minimapSpawnIndicatorPrefab;
    [SerializeField, Tooltip("���� ��ġ ǥ��")]
    private Transform spawnPosTr;
    [SerializeField, Tooltip("�˸� �����")] 
    private AudioClip spawnSound;

    public Transform SpawnPosTr => spawnPosTr;

    void Awake()
    {
        wallLights = GetComponentsInChildren<Light>();

        Debug.Assert(spawnPosTr, $"Error : not set spawn pos tr in {gameObject.name}");
        Debug.Assert(minimapSpawnIndicatorPrefab, $"Error : not set minimap spawn indicator in {gameObject.name}");
        Debug.Assert(spawnSound, $"Error : not set spawn sound in {gameObject.name}");

        halfDuration = duration / 2;
    }

    public void NotifySpawn()
    {
        AudioManager.Instance.PlaySfxNonSpatial(spawnSound);
        var spawnIndicator = Instantiate(minimapSpawnIndicatorPrefab);
        spawnIndicator.transform.position = spawnPosTr.position;
        foreach (var light in wallLights)
        {
            light.color = warnColor;
            Sequence s = DOTween.Sequence();
            s.Append(light.DOIntensity(intensities.y, halfDuration));
            s.Insert(halfDuration, light.DOIntensity(intensities.x, halfDuration));
            s.SetLoops(6, LoopType.Yoyo);
            s.onComplete = () =>
            {
                light.color = normalColor;
                light.intensity = (intensities.x + intensities.y) / 2;
            };
        }
    }
}
