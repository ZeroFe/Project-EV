using UnityEngine;

[CreateAssetMenu(fileName = "New Cc Effect", menuName = "UseEffect/CC Effect")]
// ���� ���� �� �Ŵ� ȿ��
public class CcEffect : UseEffect
{
    public override void TakeUseEffect(GameObject sender, GameObject target)
    {
        throw new System.NotImplementedException();
    }
    public string Explain()
    {
        return "CC Effect";
    }
}
