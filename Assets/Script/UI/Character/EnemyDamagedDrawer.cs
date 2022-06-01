using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 적이 입은 피해를 그려주는 Object
/// </summary>
public class EnemyDamagedDrawer : MonoBehaviour
{
    private static readonly float INIT_GRAVITY = 3.0f;
    private static readonly float INIT_LIFETIME = 2.0f;

    [SerializeField] private TextMeshPro damageText;

    [SerializeField]
    private Vector3 moveVec = Vector3.zero;
    [SerializeField]
    private float _gravity = 3.0f;
    [SerializeField]
    private float _lifeTime = 2.0f;
    [SerializeField]
    private float _bonusUpVelocity = 3.0f;

    private float _initRange = 1.0f;

    private void Start()
    {
        Invoke(nameof(Destroy), _lifeTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetDrawCritical(Vector3 position)
    {
        damageText.text = "Critical";
        damageText.color = Color.red;
        _gravity = -1.0f;
    }

    // 나머지 값들은 Prefab에서 설정하도록 만듦
    public void SetDraw(int damage)
    {
        damageText.text = damage.ToString();
        damageText.transform.localScale = Vector3.one * Mathf.Log10(damage);
    }

    public void SetDrawInfo(int damage, Color color, Vector3 position)
    {
        Vector3 initVec = new Vector3(
            Random.Range(-_initRange, _initRange),
            Random.Range(-_initRange, _initRange) + _bonusUpVelocity,
            Random.Range(-_initRange, _initRange)
            );
        SetDrawInfo(damage, color, position, initVec, _gravity, _lifeTime);
    }

    public void SetDrawInfo(int damage, Color color, Vector3 position, 
        Vector3 initVec, float gravity, float lifeTime)
    {
        moveVec = initVec;
        damageText.transform.localScale = Vector3.one * Mathf.Log10(damage);
        damageText.text = damage.ToString();
        damageText.color = color;
        _gravity = gravity;
        _lifeTime = lifeTime;
    }

    private void Update()
    {
        transform.Translate(moveVec * Time.deltaTime, Space.World);
        moveVec -= Vector3.up * (_gravity * Time.deltaTime);
    }
}
