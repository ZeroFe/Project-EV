using UnityEngine;

public abstract class PassiveConditionOption : EnumScriptableObject
{
    public abstract int AddCount(int userCount, int newAddedCount);

    public abstract bool SatisfyCondition(int lhs, int rhs);

    public abstract int ResetCount(int passiveCount, int userCount);
}
