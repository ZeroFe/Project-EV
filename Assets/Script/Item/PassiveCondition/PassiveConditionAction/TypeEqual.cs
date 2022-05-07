using UnityEngine;

public class TypeEqual : PassiveConditionAction
{
    public override int AddCount(int userCount, int newAddedCount)
    {
        return newAddedCount;
    }

    public override bool SatisfyCondition(int passiveCount, int userCount)
    {
        return passiveCount == userCount;
    }

    public override int NextCount(int passiveCount, int userCount)
    {
        return 0;
    }
}
