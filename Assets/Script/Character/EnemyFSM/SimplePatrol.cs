using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �¾ �� cube�� start�� ���� �ʹ�
// cube�� end�� ���ٰ� �ٽ� start�� �����̰�, �̸� �ݺ��Ѵ�
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
