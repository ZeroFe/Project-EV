using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Condition", menuName = "Item/Passive Condition")]
public class PassiveCondition : ScriptableObject
{
    // EventType�� Scriptable Object
    public MagiMaker.EventObject conditionEvent;
    public PassiveConditionOption conditionOption;

    /// <returns>�нú� �ߵ� ���� ���� ����</returns>
    public bool SatisfyCondition(int passiveCount, int userCount)
    {
        return conditionOption.SatisfyCondition(passiveCount, userCount);
    }

    public int NextUserCount(int passiveCount, int userCount)
    {
        return conditionOption.ResetCount(passiveCount, userCount);
    }

    public string Explain()
    {
        return "";
    }
}
