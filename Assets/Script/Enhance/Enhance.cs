using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �÷��̾� ��ȭ�� ��� Ŭ����
/// �� Ŭ������ �нú� ����
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
                // �������� ���� ������ �����ϴ� ������ �Ǵ�
                // ��� sender�� target�� �� �� owner�̴�
                effect.GetReference().TakeUseEffect(owner, owner);
            }
        }
    }

    [SerializeField] private string enhanceName;
    [SerializeField] private string codeName;
    [SerializeField] private Sprite icon = null;
    [SerializeField, TextArea(3, 5)] 
    private string description = "";
    // ������ ȹ���� ���� ���� ����
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
            // ���� : �нú� �����ؾ��� ���� ������ ���ٽ��� �ƴ� ����ͷ� ������ ��
            passive.passiveCondition.GetReference().onSuccessCondition += () => passive.UsePassiveEffect(owner);
        }
    }

    public Enhance Clone()
    {
        var clone = Instantiate(this);
        // clone�� ���� ScriptableObject�� �ٸ� List�� ������ �Ѵ�
        // (������ ���� ������ �ǵ帮�� �ʱ� ����)
        // �׷��� ���� ���� �ȿ� ���� ScriptableObject�� ���� �Ͱ� �ٸ� �ʿ䰡 �����Ƿ� ���� ������ �ʴ´�
        clone.prerequisites = prerequisites.ConvertAll(o => o);
        return clone;
    }
}
