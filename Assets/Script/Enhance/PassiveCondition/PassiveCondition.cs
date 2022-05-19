using System;
using UnityEngine;

/// <summary>
/// ��ȭ�� �нú� ������ ����
/// �нú� ���� ���� ������ �ڽ� Ŭ�������� ����
/// </summary>
public abstract class PassiveCondition : ScriptableObject
{
    [SerializeField, Tooltip("�нú� ������ ������ �� ó���� ���")]
    private PassiveConditionAction _conditionAction;

    // 
    [Header("Setting")] 
    [SerializeField, Tooltip("�̺�Ʈ ���� ���� ī��Ʈ")]
    private int _conditionActionCount = 0;
    private int _currentCount = 0;

    #region Property

    public int CurrentCount
    {
        get => _currentCount;
        set
        {
            int prevSuccess = _currentCount;
            _currentCount = Mathf.Clamp(value, 0, _conditionActionCount);
            if (_currentCount != prevSuccess)
            {
                //state = _currentCount == _conditionActionCount ? TaskState.Complete : TaskState.Running;
                //onSuccessChanged?.Invoke(this, _currentCount, prevSuccess);
            }
        }
    }

    public int ConditionActionCount => _conditionActionCount;

    #endregion

    public event Action onSuccessCondition;

    public abstract void Register(GameObject target);

    /// <summary>
    /// ��� �нú긦 �߰� ���ǿ� ���� ����
    /// ex) nȸ�� �� �� ���� / n �̻��� �� ���� / n ������ �� ����
    /// Count ��� �ܿ� �߰����� �̺�Ʈ�� ����Ѵٸ� �ڽ� Ŭ�������� �Լ��� ���� �����ϴ� �� ��Ģ���� ��
    /// </summary>
    /// <param name="addedCount"></param>
    protected void ExecuteEquipPassive(int addedCount)
    {
        _currentCount = _conditionAction.AddCount(_currentCount, addedCount);
        Debug.Log($"current Event Count - {_currentCount}");
        if (_conditionAction.SatisfyCondition(_conditionActionCount, _currentCount))
        {
            onSuccessCondition?.Invoke();
        }
        _currentCount = _conditionAction.NextCount(_conditionActionCount, _currentCount);
    }

    public string Explain()
    {
        return "";
    }
}


[System.Serializable]
public class PassiveConditionReference : ScriptableObjectReference<PassiveCondition>
{

}
