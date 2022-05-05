using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(VisualEffect))]
// 플레이어가 줍기 가능한 돌려쓰기 방식의 컴포넌트
public class DroppedItemComponent : MonoBehaviour
{
    private static readonly int dropForce = 300;
    public Item ItemData
    {
        get { return itemData; }
        set 
        {
            // 아이템 색상 바꾸기
            //GetComponent<VisualEffect>().GetVector4(Shader.PropertyToID("Color"));
            itemData = value;
        }
    }

    public Item itemData = null;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(Vector3.up * dropForce);
    }
}
