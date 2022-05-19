
/// <summary>
/// '1회 사용'처럼 이벤트 발생 시마다 작동하도록 만드는 조건
/// </summary>
public class Always : PassiveConditionAction
{
    public override int AddCount(int userCount, int newAddedCount)
    {
        return userCount;
    }

    public override bool SatisfyCondition(int passiveCount, int userCount)
    {
        return true;
    }

    public override int NextCount(int conditionActionCount, int currentCount)
    {
        return 0;
    }
}
