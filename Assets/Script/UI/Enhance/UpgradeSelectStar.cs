using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectStar : MonoBehaviour
{
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform[] routePoses;
    [SerializeField] private float moveSpeed = 50.0f;
    private int targetIndex = 0;

    void OnEnable()
    {
        transform.position = startPos.position;
        targetIndex = 0;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, routePoses[targetIndex].position) < 0.7f)
        {
            targetIndex = (targetIndex + 1) % routePoses.Length;
        }

        var dir = routePoses[targetIndex].position - transform.position;
        dir.Normalize();
        transform.position += dir * (moveSpeed * Time.unscaledDeltaTime);
    }
}
