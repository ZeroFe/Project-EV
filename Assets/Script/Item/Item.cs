using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite Image = null;
    public string Description = "";
    // ������ ȹ���� ���� ���� ����
    public Item[] prerequisites;
}
