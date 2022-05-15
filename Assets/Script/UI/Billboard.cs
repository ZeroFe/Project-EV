using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _main;

    public void Awake()
    {
        _main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = _main.transform.forward;
    }
}
