using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpearProjectile : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 force)
    {
        rb.velocity = force;
    }

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    private void Update()
    {
        transform.forward = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var status = collision.gameObject.GetComponent<CharacterStatus>();
        if (status)
        {
            status.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
