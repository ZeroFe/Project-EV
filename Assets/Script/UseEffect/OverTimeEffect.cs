using UnityEngine;

[CreateAssetMenu(fileName = "New Over Time Effect", menuName = "UseEffect/Over Time Effect")]
public class OverTimeEffect : PersistEffect
{
    [Tooltip("Effect�� �ߵ��ϴ� �ֱ�")]
    public float Period = 1f;
    public UseEffect Effect = null;

    public override void TakeUseEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string Explain()
    {
        return $"Over Time : {Period}";
    }
}
