using UnityEngine;

[CreateAssetMenu(fileName = "New Equip Item", menuName = "Item/Equip Item")]
public class EquipItem : Item
{
    public enum Grade
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
    private Grade _grade;
    public Grade grade => _grade;

    public EquipStatus[] equipAbilities;

    [Header("Passive")]
    public PassiveCondition passiveCondition;
    public int passiveCount;
    public UseEffectReference[] passiveEffect;

}
