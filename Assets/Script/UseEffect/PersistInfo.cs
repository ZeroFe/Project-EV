using UnityEngine;

[CreateAssetMenu(fileName = "New Persist Info", menuName = "UseEffect/Persist Info")]
public class PersistInfo : ScriptableObject
{
    [HideInInspector] public int ID; 
#if UNITY_EDITOR
    public string Description;
#endif
    public Sprite Thumbnail;
    //public ParticleSystem 
    // �� Persist���� ���еǰ� �����?
    [Tooltip("ȿ���� ���������� ")]
    public bool stackable = true;
    [Tooltip("�ִ� �� ��ø���� ��������")]
    public int MaxStack = 1;
    [Tooltip("���ӽð��� ������ �� ������ �����ϴµ� �ɸ��� �ð�")]
    public float Respite = 0;
}
