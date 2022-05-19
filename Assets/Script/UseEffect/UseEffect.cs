using UnityEngine;

/// <summary>
/// ��ų, ������ �� ȿ���� ������ ��� ��쿡 ����ϴ� Ŭ����
/// �� ���� �������� ȿ���� ������ �� �ֵ��� UseEffect[] ���·� ����ϱ� ����
/// ���ο� ȿ���� �����ϰ� ���� ��� UseEffect ����� �޾Ƽ� ������ ��
/// </summary>
public abstract class UseEffect : ScriptableObject
{
    /// <summary>
    /// ȿ���� �����ϴ� �Լ�
    /// </summary>
    public abstract void TakeUseEffect(GameObject sender, GameObject target);
}

[System.Serializable]
public class UseEffectReference : ScriptableObjectReference<UseEffect>
{

}