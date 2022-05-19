using UnityEngine;

[CreateAssetMenu(fileName = "New Cc Effect", menuName = "UseEffect/CC Effect")]
// 대충 기절 등 거는 효과
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
