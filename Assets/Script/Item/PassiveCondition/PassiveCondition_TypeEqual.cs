using UnityEngine;

public class PassiveCondition_TypeEqual : PassiveConditionOption
{
    public override int AddCount(int userCount, int newAddedCount)
    {
        return newAddedCount;
    }

    public override bool SatisfyCondition(int passiveCount, int userCount)
    {
        return passiveCount == userCount;
    }

    public override int ResetCount(int passiveCount, int userCount)
    {
        return 0;
    }
}
