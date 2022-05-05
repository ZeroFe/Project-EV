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
    // 각 Persist마다 구분되게 만들기?
    [Tooltip("효과가 개별적으로 ")]
    public bool stackable = true;
    [Tooltip("최대 몇 중첩까지 가능한지")]
    public int MaxStack = 1;
    [Tooltip("지속시간이 끝났을 때 스택이 감소하는데 걸리는 시간")]
    public float Respite = 0;
}
