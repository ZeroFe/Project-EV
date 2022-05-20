using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 무한 탄창, 무기 하나 방식의 
/// </summary>
public class Weapon : MonoBehaviour
{
    static RaycastHit[] s_HitInfoBuffer = new RaycastHit[8];

    public enum TriggerType
    {
        Auto,
        Manual
    }

    public enum WeaponType
    {
        Raycast,
        Projectile
    }

    public enum WeaponState
    {
        Idle,
        Firing,
        Reloading
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float spreadAngle = 0.0f;
        public int projectilePerShot = 1;
        public float screenShakeMultiplier = 1.0f;
    }

    // Weapon의 경우 강화 등 외부에서 수정/변경할 일이 많으므로
    // 의도적으로 public으로 만들어서 사용하는게 적합하다 판단
    // 이후 property를 통해 수정하도록 만들 수도 있음
    [Tooltip("클릭 당 사격인지, 자동 사격인지")]
    public TriggerType triggerType = TriggerType.Manual;
    [Tooltip("총알 적용 방식")]
    public WeaponType weaponType = WeaponType.Raycast;

    // 발사 관련
    [Tooltip("발사 딜레이")]
    public float fireRate = 0.5f;
    private bool m_ShotDone;
    private float m_ShotTimer = -1.0f;

    [Tooltip("재장전 딜레이")]
    public float reloadTime = 2.0f;
    private float currentReloadTime = 0.0f;

    [Tooltip("탄창 크기")]
    public int clipSize = 4;
    private int currentAmmoCount;
    
    // 데미지, 효과
    public float damage = 1.0f;
    private float damageMultiplier = 1.0f;

    public Projectile projectilePrefab;
    public float projectileLaunchForce = 200.0f;

    public GameObject bulletEffectPrefab;
    public ParticleSystem muzzleEffect;
    public float muzzleEffectTime = 0.05f;

    public AdvancedSettings advancedSettings;

    // 그 외
    public Transform EndPoint;

    private bool m_TriggerDown;


    WeaponState m_CurrentState;
    public WeaponState CurrentState => m_CurrentState;

    public PlayerCtrl Owner => owner;
    private PlayerCtrl owner;
    private PlayerStatus ownerStatus;
    private Camera _main;

    //Animator m_Animator;

    AudioSource m_Source;

    Vector3 m_ConvertedMuzzlePos;

    class ActiveTrail
    {
        public LineRenderer renderer;
        public Vector3 direction;
        public float remainingTime;
    }

    List<ActiveTrail> m_ActiveTrails = new List<ActiveTrail>();

    Queue<Projectile> m_ProjectilePool = new Queue<Projectile>();

    int fireNameHash = Animator.StringToHash("fire");
    int reloadNameHash = Animator.StringToHash("reload");

    #region property

    public bool TriggerDown
    {
        get { return m_TriggerDown; }
        set
        {
            m_TriggerDown = value;
            if (!m_TriggerDown) m_ShotDone = false;
        }
    }

    public int CurrentAmmoCount
    {
        get => currentAmmoCount;
        set
        {
            currentAmmoCount = Math.Clamp(value, 0, clipSize);
            WeaponView.Instance.UpdateAmmoCount(currentAmmoCount);
        }
    }

    public int ClipSize
    {
        get => clipSize;
        set
        {
            clipSize = value;
            CurrentAmmoCount = Math.Min(currentAmmoCount, clipSize);
            WeaponView.Instance.UpdateClipInfo(clipSize);
        }
    }

    public float CurrentReloadTime
    {
        get => currentReloadTime;
        set
        {
            currentReloadTime = value;
            WeaponView.Instance.UpdateReloadBar(currentReloadTime / reloadTime);
        }
    }

    #endregion     

    public event Action OnFire;

    void Awake()
    {
        // 애니메이터, 소리는 후적용
        //m_Animator = GetComponentInChildren<Animator>();
        //m_Source = GetComponentInChildren<AudioSource>();
        _main = Camera.main;

        // 총알 설정
        currentAmmoCount = clipSize;

        // 투사체 방식 설정
        if (projectilePrefab != null)
        {
            //a minimum of 4 is useful for weapon that have a clip size of 1 and where you can throw a second
            //or more before the previous one was recycled/exploded.
            int size = Mathf.Max(4, clipSize) * advancedSettings.projectilePerShot;
            for (int i = 0; i < size; ++i)
            {
                Projectile p = Instantiate(projectilePrefab);
                p.gameObject.SetActive(false);
                m_ProjectilePool.Enqueue(p);
            }
        }
    }

    public void PickedUp(PlayerCtrl c)
    {
        owner = c;
        ownerStatus = owner.GetComponent<PlayerStatus>();
    }

    private void Start()
    {
        WeaponView.Instance.UpdateClipInfo(ClipSize);
        WeaponView.Instance.UpdateAmmoCount(CurrentAmmoCount);
    }

    // 총 발사
    public void Fire()
    {
        if (m_CurrentState != WeaponState.Idle || m_ShotTimer > 0 || currentAmmoCount == 0)
            return;

        // 수정 필요 : 이후 Projectile Per Shot을 기준으로 변경해야할 수도 있음
        CurrentAmmoCount -= 1;
        
        m_ShotTimer = fireRate;

        //the state will only change next frame, so we set it right now.
        m_CurrentState = WeaponState.Firing;

        StartCoroutine(DoMuzzleEffect());

        //m_Animator.SetTrigger("fire");

        // 소리
        //m_Source.pitch = Random.Range(0.7f, 1.0f);
        //m_Source.PlayOneShot(FireAudioClip);

        //CameraShaker.Instance.Shake(0.2f, 0.05f * advancedSettings.screenShakeMultiplier);

        OnFire?.Invoke();
        // 발사 
        if (weaponType == WeaponType.Raycast)
        {
            for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
            {
                RaycastShot();
            }
        }
        else
        {
            ProjectileShot();
        }
    }

    IEnumerator DoMuzzleEffect()
    {
        muzzleEffect.Stop();
        muzzleEffect.Play();
        yield return new WaitForSeconds(muzzleEffectTime);
        muzzleEffect.Stop();
    }


    private void RaycastShot()
    {

        //compute the ratio of our spread angle over the fov to know in viewport space what is the possible offset from center
        //float spreadRatio = advancedSettings.spreadAngle / Controller.Instance.MainCamera.fieldOfView;
        float spreadRatio = advancedSettings.spreadAngle / Camera.main.fieldOfView;

        Vector2 spread = spreadRatio * Random.insideUnitCircle;
        
        RaycastHit hit;
        Ray r = Camera.main.ViewportPointToRay(Vector3.one * 0.5f + (Vector3)spread);
        Vector3 hitPosition = r.origin + r.direction * 200.0f;
        
        if (Physics.Raycast(r, out hit, 1000.0f, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
        {
            Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            //ImpactManager.Instance.PlayImpact(hit.point, hit.normal, renderer == null ? null : renderer.sharedMaterial);
            Instantiate(bulletEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            //if too close, the trail effect would look weird if it arced to hit the wall, so only correct it if far
            if (hit.distance > 5.0f)
                hitPosition = hit.point;

            Debug.Log("Raycast shot hit");
            //this is a target
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // TODO : UseEffect 주기
                var targetStatus = hit.collider.gameObject.GetComponent<EnemyStatus>();
                if (targetStatus)
                {
                    targetStatus.TakeDamage((int)(ownerStatus.attackPower * damageMultiplier));
                }
                else
                {
                    Debug.LogError($"enemy {hit.collider.gameObject.name}'s status not find");
                }
            }
        }

        // VFX : 이후 적용
        //if (PrefabRayTrail != null)
        //{
        //    //var pos = new Vector3[] { GetCorrectedMuzzlePlace(), hitPosition };
        //    var pos = new Vector3[] { transform.position, hitPosition };
        //    var trail = PoolSystem.Instance.GetInstance<LineRenderer>(PrefabRayTrail);
        //    trail.gameObject.SetActive(true);
        //    trail.SetPositions(pos);
        //    m_ActiveTrails.Add(new ActiveTrail()
        //    {
        //        remainingTime = 0.3f,
        //        direction = (pos[1] - pos[0]).normalized,
        //        renderer = trail
        //    });
        //}
    }

    private void ProjectileShot()
    {
        for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
        {
            float angle = Random.Range(0.0f, advancedSettings.spreadAngle * 0.5f);
            Vector2 angleDir = Random.insideUnitCircle * Mathf.Tan(angle * Mathf.Deg2Rad);

            Vector3 dir = EndPoint.transform.forward + (Vector3)angleDir;
            dir.Normalize();

            var p = m_ProjectilePool.Dequeue();
            
            p.gameObject.SetActive(true);
            p.Launch(this, dir, projectileLaunchForce);
        }
    }

    //For optimization, when a projectile is "destroyed" it is instead disabled and return to the weapon for reuse.
    public void ReturnProjectile(Projectile p)
    {
        m_ProjectilePool.Enqueue(p);
    }

    public void Reload()
    {
        if (m_CurrentState != WeaponState.Idle || currentAmmoCount == clipSize)
            return;

        // SFX : 나중에 넣기
        //if (ReloadAudioClip != null)
        //{
        //    m_Source.pitch = Random.Range(0.7f, 1.0f);
        //    m_Source.PlayOneShot(ReloadAudioClip);
        //}

        //the state will only change next frame, so we set it right now.
        m_CurrentState = WeaponState.Reloading;

        CurrentAmmoCount = clipSize;

        currentReloadTime = reloadTime;

        //m_Animator.SetTrigger("reload");
    }

    void Update()
    {
        UpdateWeaponState();

        //Vector3[] pos = new Vector3[2];
        //for (int i = 0; i < m_ActiveTrails.Count; ++i)
        //{
        //    var activeTrail = m_ActiveTrails[i];
            
        //    activeTrail.renderer.GetPositions(pos);
        //    activeTrail.remainingTime -= Time.deltaTime;

        //    pos[0] += activeTrail.direction * 50.0f * Time.deltaTime;
        //    pos[1] += activeTrail.direction * 50.0f * Time.deltaTime;
            
        //    m_ActiveTrails[i].renderer.SetPositions(pos);
            
        //    if (m_ActiveTrails[i].remainingTime <= 0.0f)
        //    {
        //        m_ActiveTrails[i].renderer.gameObject.SetActive(false);
        //        m_ActiveTrails.RemoveAt(i);
        //        i--;
        //    }
        //}
    }

    void UpdateWeaponState()
    {
        //m_Animator.SetFloat("speed", m_Owner.Speed);
        //m_Animator.SetBool("grounded", m_Owner.Grounded);

        //var info = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (m_CurrentState == WeaponState.Firing &&  m_ShotTimer <= 0)
        {
            Debug.Log("To Idle");
            m_CurrentState = WeaponState.Idle;
        }
        else if (m_CurrentState == WeaponState.Reloading && currentReloadTime <= 0)
        {
            m_CurrentState = WeaponState.Idle;
        }

        if (CurrentState == WeaponState.Idle)
        {
            if (currentAmmoCount == 0)
                Reload();
        }

        if (TriggerDown)
        {
            if (triggerType == TriggerType.Manual)
            {
                if (!m_ShotDone)
                {
                    m_ShotDone = true;
                    Fire();
                }
            }
            else
                Fire();
        }

        if (m_ShotTimer > 0)
        {
            m_ShotTimer -= Time.deltaTime;
        }

        if (currentReloadTime > 0)
        {
            CurrentReloadTime -= Time.deltaTime;
        }
    }
}

public abstract class AmmoDisplay : MonoBehaviour
{
    public abstract void UpdateAmount(int current, int max);
}