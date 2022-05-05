using UnityEngine;

/// <summary>
/// 일반몹, 엘리트몹, 보스몹 가리지 않고 같은 데미지를 준다
/// </summary>
[CreateAssetMenu(fileName = "New Simple HP Change Effect", menuName = "UseEffect/Simple Hp Change Effect")]
public class SimpleHpChangeEffect : UseEffect
{
    public enum EHpRatioType
    {
        Current,
        Max,
        Lost
    }

    [Tooltip("힐 / 데미지 구분")]
    public bool IsHeal = false;
    [Tooltip("데미지용 : 방어무시인가")]
    public bool IsTrueDamage = false;
    [Tooltip("능력치에 영향받지 않는 수치")]
    public int FixedAmount = 0;
    [Tooltip("능력치에 영향받는 수치(퍼센트)")]
    public float EnhancePercentRate = 0;
    [Tooltip("체력 비례 적용 방식")]
    public EHpRatioType RatioType;
    [Tooltip("ratioType에 따른 적용 수치(퍼센트)")]
    [Range(0, 100)] public float RatioPercent = 0;

    public override string Explain()
    {
        return $"SimpleHpChange : {FixedAmount} + {EnhancePercentRate} + {RatioPercent}";
    }

    public override void TakeUseEffect()
    {
        throw new System.NotImplementedException();
    }
}
