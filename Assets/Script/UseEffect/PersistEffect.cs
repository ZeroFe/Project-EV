using UnityEngine;

public abstract class PersistEffect : UseEffect
{
    public PersistInfo persistInfo;
    public float Time;
    [Tooltip("���� ������")]
    public int increaseStack = 1;
}
