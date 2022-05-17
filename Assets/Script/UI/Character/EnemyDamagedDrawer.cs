using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 적이 입은 피해를 그려주는 Object
/// </summary>
public class EnemyDamagedDrawer : MonoBehaviour
{
    private static readonly float INIT_GRAVITY = 3.0f;
    private static readonly float INIT_LIFETIME = 2.0f;

    [SerializeField] private TextMeshProUGUI damageText;

    private Vector3 moveVec = Vector3.zero;

    private float _gravity = 3.0f;
    private float _lifeTime = 2.0f;

    private void Start()
    {
        Invoke(nameof(Destroy), _lifeTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetDrawInfo(int damage, Color color, Vector3 position)
    {
        //SetDrawInfo(damage, color, );
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
