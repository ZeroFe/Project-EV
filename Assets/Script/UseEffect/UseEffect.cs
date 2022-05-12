using UnityEngine;

/// <summary>
/// 스킬, 아이템 등 효과를 가지는 모든 경우에 사용하는 클래스
/// 한 번에 여러가지 효과를 실행할 수 있도록 UseEffect[] 형태로 사용하길 권장
/// 새로운 효과를 제작하고 싶은 경우 UseEffect 상속을 받아서 구현할 것
/// </summary>
public abstract class UseEffect : ScriptableObject
{
    /// <summary>
    /// 효과를 적용하는 함수
    /// </summary>
    public abstract void TakeUseEffect();

    /// <summary></summary>
    /// <returns>능력을 설명하는 텍스트('\n' 없음)</returns>
    public abstract string Explain();
}

[System.Serializable]
public class UseEffectReference : ScriptableObjectReference<UseEffect>
{

}