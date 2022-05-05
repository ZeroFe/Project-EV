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
        //"화상 3스택 상태일 때 500 데미지, 1.5초 기절";
        //string ret = $"{Condition.name} 상태일 때 ";
        string ret = $"조건 A 상태일 때 ";
        return ret;
    }

    public override void TakeUseEffect()
    {
        throw new System.NotImplementedException();
    }
}
