using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 플레이어 강화를 담는 클래스
/// 이 클래스는 패시브 위주
/// </summary>
[CreateAssetMenu(fileName = "New Equip Item", menuName = "Item/Equip Item")]
public class EquipItem : Item
{
    public enum ItemGrade
    {
        Normal,
        Rare,
        Unique,
        Epic,
    }

    /// <summary>
    /// 장비 강화 능력치
    /// </summary>
    public enum EStatusType
    {
        Attack,
        AttackSpeed,
        MaxHp,
        HpRegen,
        Defence,
        Speed,
        SkillCoolTime,
        DamageReduceRate,
        SkillRange,
        AttackPercent,
    }

    [System.Serializable]
    public class EquipStatus
    {
        public EStatusType equipEffect;
        public int value;
    }

    [SerializeField]
    private ItemGrade _grade;
    public ItemGrade Grade => _grade;

    public EquipStatus[] equipAbilities;

    [Header("Passive")]
    [SerializeField]
    private PassiveCondition passiveCondition;
    public UseEffectReference[] passiveEffect;


    public void Register(GameObject target)
    {
        passiveCondition.Register(target);
        passiveCondition.onSuccessCondition += UseEquipEffect;
    }

    private void UseEquipEffect()
    {
        foreach (var effect in passiveEffect)
        {
            effect.GetReference().TakeUseEffect();
        }
    }
}
