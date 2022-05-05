using UnityEngine;

[CreateAssetMenu(fileName = "New Conditional Effect", menuName = "UseEffect/Conditional Effect")]
public class ConditionalEffect : UseEffect
{
    [System.Serializable]
    public class Threshold
    {
        public int Value = 1;
        public int Increase = 0;
    }

    public PersistInfo Condition = null;
    public Threshold threshold = new Threshold();
    public bool RemoveConditionalEffect = false;
    public bool IsRelationStack = false;
    public float StackBonusRate = 0;
    public UseEffectReference[] Effect = null;

    public override string Explain()
    {
        //"ȭ�� 3���� ������ �� 500 ������, 1.5�� ����";
        //string ret = $"{Condition.name} ������ �� ";
        string ret = $"���� A ������ �� ";
        return ret;
    }

    public override void TakeUseEffect()
    {
        throw new System.NotImplementedException();
    }
}
