using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public int damage = 10;
    public float radius = 5.0f;
    public GameObject explosionEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    void Explosion()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");

        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, 0.01f, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyStatus enemy = hits[i].collider.gameObject.GetComponent<EnemyStatus>();
            if (enemy)
            {
                enemy.TakeDamage(damage);
            }
        }

        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
