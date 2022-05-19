using System;
using UnityEngine;

/// <summary>
/// 강화의 패시브 조건을 지정
/// 패시브 조건 세부 사항은 자식 클래스에서 지정
/// </summary>
public abstract class PassiveCondition : ScriptableObject
{
    [SerializeField, Tooltip("패시브 조건을 충족할 때 처리할 방식")]
    private PassiveConditionAction _conditionAction;

    // 
    [Header("Setting")] 
    [SerializeField, Tooltip("이벤트 성공 조건 카운트")]
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
    /// 장비 패시브를 추가 조건에 따라 실행
    /// ex) n회에 한 번 실행 / n 이상일 때 실행 / n 이하일 때 실행
    /// Count 사용 외에 추가적인 이벤트를 사용한다면 자식 클래스에서 함수를 만들어서 적용하는 걸 원칙으로 함
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
