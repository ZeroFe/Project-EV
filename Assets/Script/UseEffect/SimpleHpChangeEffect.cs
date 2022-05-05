using UnityEngine;

/// <summary>
/// �Ϲݸ�, ����Ʈ��, ������ ������ �ʰ� ���� �������� �ش�
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

    [Tooltip("�� / ������ ����")]
    public bool IsHeal = false;
    [Tooltip("�������� : �����ΰ�")]
    public bool IsTrueDamage = false;
    [Tooltip("�ɷ�ġ�� ������� �ʴ� ��ġ")]
    public int FixedAmount = 0;
    [Tooltip("�ɷ�ġ�� ����޴� ��ġ(�ۼ�Ʈ)")]
    public float EnhancePercentRate = 0;
    [Tooltip("ü�� ��� ���� ���")]
    public EHpRatioType RatioType;
    [Tooltip("ratioType�� ���� ���� ��ġ(�ۼ�Ʈ)")]
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
