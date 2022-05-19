using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �÷��̾� ��ȭ�� ��� Ŭ����
/// �� Ŭ������ �нú� ����
/// </summary>
[CreateAssetMenu(fileName = "New Equip Enhance", menuName = "Enhance/Enhance")]
public class Enhance : ScriptableObject
{
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
    [SerializeField]
    private PassiveConditionReference passiveCondition;
    public UseEffectReference[] passiveEffect;

    private GameObject owner;

    public void Register(GameObject target)
    {
        if (passiveCondition == null)
        {
            return;
        }

        this.owner = target;
        passiveCondition.GetReference().Register(target);
        passiveCondition.GetReference().onSuccessCondition += UseEquipEffect;
    }

    private void UseEquipEffect()
    {
        foreach (var effect in passiveEffect)
        {
            // �������� ���� ������ �����ϴ� ������ �Ǵ�
            // ��� sender�� target�� �� �� owner�̴�
            effect.GetReference().TakeUseEffect(owner, owner);
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
