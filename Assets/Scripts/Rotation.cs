using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Child 1 : Cycle Point
/// Child 2 : Pivot Point
/// 
/// </summary>

public class NewBehaviourScript : MonoBehaviour
{
    bool[] booArray = new bool[60]; // default false
    Vector2 StartPos = Vector2.zero;
    Vector2 ChildStartPos = Vector2.zero;

    float ANGLE = -10.0f;
    float LENGTH = 0.5f;
    Vector2 Pivot = Vector2.zero;

    Transform CycleTransform;
    Transform PivotTransform;
    Transform RightTransform;

    Vector3 PrevPosition;
    Vector3 NewPosition;

    int Speed = 3;

    void Start()
    {
        StartPos = transform.position;
        Debug.Log(StartPos);

        ChildStartPos = transform.GetChild(0).position;
        Debug.Log(ChildStartPos);

        CycleTransform = transform.GetChild(0);
        PivotTransform = transform.GetChild(1);
        RightTransform = transform.GetChild(2);
    }

    void Update()
    {
        // 중심점 기준 회전
        //transform.Rotate(0, 0, -50f * Time.deltaTime);

        // 임의점 기준 회전
        // Cycle.y == Pivot.y
        if(RightTransform.position.y <= PivotTransform.position.y)
        {
            RightTransform.position = new Vector3(transform.position.x + LENGTH, transform.position.y + LENGTH, -1);
            PivotTransform.position = new Vector3(transform.position.x + LENGTH, transform.position.y - LENGTH, -1);
        }
        // Pivot.y 중심 회전
        transform.RotateAround(PivotTransform.position, Vector3.forward, ANGLE * Time.deltaTime * Speed);
    }
}
