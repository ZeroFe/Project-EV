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
    [SerializeField] private string codeName;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private string description = "";
    // ������ ȹ���� ���� ���� ����
    public List<Enhance> prerequisites;

    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string Description => description;

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
