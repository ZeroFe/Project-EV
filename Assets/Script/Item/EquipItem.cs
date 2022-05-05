using UnityEngine;

[CreateAssetMenu(fileName = "New Equip Item", menuName = "Item/Equip Item")]
public class EquipItem : Item
{
    /// <summary>
    /// ��� ��ȭ �ɷ�ġ
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

    public EquipStatus[] equipAbilities;

    [Header("Passive")]
    public PassiveCondition passiveCondition;
    public int passiveCount;
    public UseEffectReference[] passiveEffect;

}
