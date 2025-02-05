using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    public void MoveAndRotate(float Angle) 
    {
        float angleRad = Angle * Mathf.Deg2Rad;
        float radius = 0.5f;
        float moveDistance = radius * angleRad;

        transform.Translate(new Vector3(moveDistance, 0, 0), Space.World);
        transform.Rotate(0, 0, Angle);
    }

    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }
}
