using UnityEngine;

public abstract class PassiveConditionAction : EnumScriptableObject
{
    public abstract int AddCount(int userCount, int newAddedCount);

    public abstract bool SatisfyCondition(int passiveCount, int userCount);

    public abstract int NextCount(int conditionActionCount, int currentCount);
}
