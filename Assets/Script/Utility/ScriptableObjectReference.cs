using System;
using UnityEngine;

/// <summary>
/// ScriptableObject를 참조하는 클래스의 PropertyDrawer를 그릴 수 있도록 만드는 Wrapper 클래스
/// </summary>
public abstract class ScriptableObjectReference<T> where T : ScriptableObject
{
    [SerializeField] protected ScriptableObject reference = null;

    public static implicit operator ScriptableObject(ScriptableObjectReference<T> SOReference)
    {
        return SOReference.reference;
    }

    public T GetReference()
    {
        return (T)reference;
    }
}