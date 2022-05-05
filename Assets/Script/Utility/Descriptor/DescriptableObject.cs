using UnityEngine;

/// <summary>
/// Descriptor를 가지고 있는 Object들에 사용하는 추상클래스
/// (아이템, 효과)
/// </summary>
public abstract class DescriptableObject : ScriptableObject
{
    [SerializeReference]
    public Descriptor descriptor = new Descriptor();

    protected virtual void OnEnable()
    {
        descriptor.InitDescriptor(this);
    }

    public abstract string Explain();
}