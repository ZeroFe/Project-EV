using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 플레이어 강화를 담는 클래스
/// 이 클래스는 패시브 위주
/// </summary>
[CreateAssetMenu(fileName = "New Equip Enhance", menuName = "Enhance/Enhance")]
public class Enhance : ScriptableObject
{
    [System.Serializable]
    public class Passive
    {
        public PassiveConditionReference passiveCondition;
        public UseEffectReference[] passiveEffects;
        
        public void UsePassiveEffect(GameObject owner)
        {
            foreach (var effect in passiveEffects)
            {
                // 아이템은 내가 나에게 적용하는 것으로 판단
                // 고로 sender와 target이 둘 다 owner이다
                effect.GetReference().TakeUseEffect(owner, owner);
            }
        }
    }

    [SerializeField] private string enhanceName;
    [SerializeField] private string codeName;
    [SerializeField] private Sprite icon = null;
    [SerializeField, TextArea(3, 5)] 
    private string description = "";
    // 아이템 획득을 위한 선행 조건
    public List<Enhance> prerequisites;

    public string EnhanceName => enhanceName;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string Description => description;

    [Header("Ability")]
    public EnhanceEffectReference[] abilities;

    [Header("Passive")] 
    [SerializeField] private Passive[] passives;

    private GameObject owner;

    public void Register(GameObject target)
    {
        owner = target;
        foreach (var passive in passives)
        {
            if (passive.passiveCondition == null)
            {
                return;
            }

            passive.passiveCondition.GetReference().Register(target);
            // 주의 : 패시브 해제해야할 일이 있으면 람다식이 아닌 어댑터로 변경할 것
            passive.passiveCondition.GetReference().onSuccessCondition += () => passive.UsePassiveEffect(owner);
        }
    }

    public Enhance Clone()
    {
        var clone = Instantiate(this);
        // clone은 기존 ScriptableObject와 다른 List를 가져야 한다
        // (원본을 선행 조건을 건드리지 않기 위함)
        // 그러나 선행 조건 안에 들어가는 ScriptableObject는 기존 것과 다를 필요가 없으므로 새로 만들지 않는다
        clone.prerequisites = prerequisites.ConvertAll(o => o);
        return clone;
    }
}
