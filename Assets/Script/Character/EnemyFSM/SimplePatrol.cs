using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 태어날 때 cube를 start에 놓고 싶다
// cube가 end로 갔다가 다시 start로 움직이고, 이를 반복한다
public class SimplePatrol : MonoBehaviour
{
    public GameObject cube;
    public Transform startTr;
    public Transform endTr;

    public AnimationCurve curve;

    private Vector3 startPos;
    private Vector3 endPos;

    private float currTime;

    // Start is called before the first frame update
    void Start()
    {
        startPos = startTr.transform.position;
        endPos = endTr.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        float currPercent = curve.Evaluate(currTime);
        cube.transform.position = Vector3.Lerp(startPos, endPos, currPercent);
    }
}
