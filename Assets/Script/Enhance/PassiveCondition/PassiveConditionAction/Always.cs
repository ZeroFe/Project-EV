
/// <summary>
/// '1ȸ ���'ó�� �̺�Ʈ �߻� �ø��� �۵��ϵ��� ����� ����
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
