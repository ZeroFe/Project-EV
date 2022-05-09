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

    // ȿ�� �ߵ������� �ʱ�ȭ�ǰ� �ƴϸ� ������
    public override int NextCount(int passiveCount, int userCount)
    {
        return userCount >= passiveCount ? 0 : userCount;
    }
}
