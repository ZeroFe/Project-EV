using UnityEngine;

[CreateAssetMenu(fileName = "New Over Time Effect", menuName = "UseEffect/Over Time Effect")]
public class OverTimeEffect : PersistEffect
{
    [Tooltip("Effect가 발동하는 주기")]
    public float Period = 1f;
    public UseEffect Effect = null;

    public override void TakeUseEffect(GameObject sender, GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public string Explain()
    {
        return $"Over Time : {Period}";
    }
}
