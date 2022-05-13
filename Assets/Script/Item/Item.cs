using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite Image = null;
    public string Description = "";
    // 아이템 획득을 위한 선행 조건
    public Item[] prerequisites;
}
