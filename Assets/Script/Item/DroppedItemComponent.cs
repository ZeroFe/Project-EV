using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(VisualEffect))]
// �÷��̾ �ݱ� ������ �������� ����� ������Ʈ
public class DroppedItemComponent : MonoBehaviour
{
    private static readonly int dropForce = 300;
    public Item ItemData
    {
        get { return itemData; }
        set 
        {
            // ������ ���� �ٲٱ�
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
