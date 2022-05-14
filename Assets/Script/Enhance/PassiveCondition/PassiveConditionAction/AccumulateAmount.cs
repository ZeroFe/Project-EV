public class AccumulateAmount : PassiveConditionAction
{
    public override int AddCount(int userCount, int newAddedCount)
    {
        return userCount + newAddedCount;
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
