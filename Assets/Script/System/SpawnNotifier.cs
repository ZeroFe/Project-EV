using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 각 입구에서 적이 출현하면 경고를 띄우고 알림을 보냄
/// </summary>
public class SpawnNotifier : MonoBehaviour
{
    [Header("Wall Light")]
    // 벽면 조명만 
    private Light[] wallLights;
    [SerializeField]
    private Color warnColor = Color.red;
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField] private float duration = 0.5f;
    private float halfDuration;
    [SerializeField] private Vector2 intensities;

    [Header("Others")]
    [SerializeField, Tooltip("미니맵에 적이 스폰됐음을 알리는 표시 ")] 
    private GameObject minimapSpawnIndicatorPrefab;
    [SerializeField, Tooltip("생성 위치 표시")]
    private Transform spawnPosTr;
    [SerializeField, Tooltip("알림 경고음")] 
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
