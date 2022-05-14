public class AccumulateCount : PassiveConditionAction
{
    public override int AddCount(int userCount, int newAddedCount)
    {
        return userCount + 1;
    }

    public override bool SatisfyCondition(int passiveCount, int userCount)
    {
        return passiveCount <= userCount;
    }

    // 효과 발동했으면 초기화되고 아니면 누적됨
    public override int NextCount(int passiveCount, int userCount)
    {
        return userCount >= passiveCount ? 0 : userCount;
    }
}
