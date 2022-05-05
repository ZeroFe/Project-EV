using UnityEngine;

/// <summary>
/// ��ų, ������ �� ȿ���� ������ ��� ��쿡 ����ϴ� Ŭ����
/// �� ���� �������� ȿ���� ������ �� �ֵ��� UseEffect[] ���·� ����ϱ� ����
/// ���ο� ȿ���� �����ϰ� ���� ��� UseEffect ����� �޾Ƽ� ������ ��
/// </summary>
public abstract class UseEffect : DescriptableObject
{
    /// <summary>
    /// ȿ���� �����ϴ� �Լ�
    /// </summary>
    public abstract void TakeUseEffect();

    /// <summary></summary>
    /// <returns>�ɷ��� �����ϴ� �ؽ�Ʈ('\n' ����)</returns>
    public abstract override string Explain();
}

[System.Serializable]
public class UseEffectReference : ScriptableObjectReference<UseEffect>
{

}