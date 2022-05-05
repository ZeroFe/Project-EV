using UnityEngine;

public abstract class PersistEffect : UseEffect
{
    public PersistInfo persistInfo;
    public float Time;
    [Tooltip("스택 증가량")]
    public int increaseStack = 1;
}
