using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public enum BgmType
    {
        Start,
        Battle,
        Upgrade,
        GameOver,
    }

    [Header("BGM")]
    private AudioSource bgm;
    [SerializeField, Tooltip("")] 
    private AudioClip[] bgmClips;

    [Header("SFX")]
    [SerializeField, Tooltip("효과음 관리 프리팹")]
    private GameObject sfxPrefab;
    [SerializeField, Tooltip("효과음 프리팹 생성 개수")] 
    private int sfxCount = 32;

    private void Awake()
    {
        Instance = this;

        bgm = GetComponent<AudioSource>();
    }

    void Start()
    {
        PoolSystem.Instance.InitPool(sfxPrefab, sfxCount);
    }

    #region BGM

    public void PlayBGM(BgmType type)
    {
        bgm.clip = bgmClips[(int)type];
        bgm.Play(0);
    }

    public void StopBGM()
    {
        bgm.Stop();
        bgm.clip = null;
    }


    #endregion

    #region SFX

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1.0f)
    {
        var sfx = PoolSystem.Instance.GetInstance<GameObject>(sfxPrefab);
        sfx.transform.position = pos;
        var audioSource = sfx.GetComponent<AudioSource>();
        //audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

    public void PlaySfxNonSpatial(AudioClip clip, float volume = 1.0f)
    {
        var sfx = PoolSystem.Instance.GetInstance<GameObject>(sfxPrefab);
        var audioSource = sfx.GetComponent<AudioSource>();
        audioSource.spatialBlend = 0.0f;
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }
    #endregion
}
