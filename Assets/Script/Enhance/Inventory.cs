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
    public delegate void AddItemHandler(string enhanceCodeName);
    #endregion

    public event AddItemHandler OnAddItem;

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