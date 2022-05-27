using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : Missile
{
    // 유도 기능
    public GameObject target;
    public float findRadius = 10.0f;
    public float rotateSpeed = 60.0f;

    private Vector3 dir = Vector3.forward;
    private float projectileSpeed = 5.0f;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        transform.forward = direction;
        dir = direction;
        projectileSpeed = speed;
    }

    void Update()
    {
        if (!target)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Enemy");
            Collider[] cols = Physics.OverlapSphere(transform.position, findRadius, layerMask);
            if (cols.Length > 0)
            {
                target = cols[0].gameObject;
                print("Find Target - " + target.name);
            }
        }
        else
        {
            Vector3 toTargetDir = (target.transform.position - transform.position).normalized;
            var newRotate = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toTargetDir),
                Time.deltaTime * rotateSpeed);
            transform.rotation = newRotate;
            dir = transform.forward;
        }

        rb.velocity = dir * projectileSpeed;
    }
}
