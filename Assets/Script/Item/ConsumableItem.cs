using UnityEngine;

/// <summary>
/// 획득 시 바로 플레이어에게 적용되는 아이템
/// </summary>
[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public UseEffectReference[] consumeEffects;
}
