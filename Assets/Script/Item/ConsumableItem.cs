using UnityEngine;

/// <summary>
/// ȹ�� �� �ٷ� �÷��̾�� ����Ǵ� ������
/// </summary>
[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public UseEffectReference[] consumeEffects;
}
