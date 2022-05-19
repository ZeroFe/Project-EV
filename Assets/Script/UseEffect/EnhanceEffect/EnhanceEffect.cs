using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ɷ�ġ ��ȭ�� �����ִ� Ŭ����
/// ��ȭ�� ���� ����� �߿����� �����Ƿ� sender�� ���� �ʴ´�
/// </summary>
public abstract class EnhanceEffect : UseEffect
{
    public override void TakeUseEffect(GameObject sender, GameObject target)
    {
        ApplyEnhance(target);
    }

    public abstract void ApplyEnhance(GameObject target);
}

[System.Serializable]
public class EnhanceEffectReference : ScriptableObjectReference<EnhanceEffect>
{

}