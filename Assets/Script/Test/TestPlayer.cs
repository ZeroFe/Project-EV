using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Inventory inventory;

    public event Action onJump;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Hit Key Jump");
            onJump?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Hit Plus");
            inventory.AddItem(ItemManager.Instance.DequeueEquipItem());
        }
    }


}
