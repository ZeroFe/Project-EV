using UnityEngine;

public class Item : DescriptableObject
{
    public enum Grade
    {
        Normal,
        Rare,
        Unique,
        Epic,
    }

    public int ID = 0;
    public Grade grade;
    public Sprite Image = null;

    public override string Explain()
    {
        return "item Explain";
    }
}
