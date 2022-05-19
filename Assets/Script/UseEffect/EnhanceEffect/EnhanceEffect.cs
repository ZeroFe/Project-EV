using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 능력치 강화를 시켜주는 클래스
/// 강화는 누가 줬는지 중요하지 않으므로 sender를 받지 않는다
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