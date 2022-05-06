using UnityEngine;

public class Item : DescriptableObject
{
    public Sprite Image = null;

    public override string Explain()
    {
        return "item Explain";
    }
}
