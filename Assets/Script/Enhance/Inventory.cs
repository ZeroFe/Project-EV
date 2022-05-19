using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class Inventory : MonoBehaviour
{
    #region Events
    public delegate void AddItemHandler(Enhance enhance);
    #endregion

    public event AddItemHandler OnAddItem;

    // int에 들어가는 값은 Enhance Code
    public class AddItemEvent : UnityEvent<int> { }
    public AddItemEvent addItemEvent { get; set; } = new AddItemEvent();

    private List<Enhance> enhances = new List<Enhance>();
    public IReadOnlyList<Enhance> Enhances => enhances;


    private void Awake()
    {

    }

    public bool AddItem(Enhance newEnhance)
    {
        Debug.Log($"add enhance - {newEnhance.name}");

        enhances.Add(newEnhance);
        AddEnhanceAbilities(newEnhance);
        RegisterPassive(newEnhance);

        //addItemEvent.Invoke(newEnhance.name.GetHashCode());

        return true;
    }

    private void RegisterPassive(Enhance enhance)
    {
        enhance.Register(gameObject);
    }

    private void AddEnhanceAbilities(Enhance newEnhance)
    {
        foreach (var ability in newEnhance.abilities)
        {
            ability.GetReference().ApplyEnhance(gameObject);
        }
    }
}