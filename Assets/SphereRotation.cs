using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    void MoveAndRotateInterface.MoveAndRotate(float Angle) 
    {
        float angleRad = Angle * Mathf.Deg2Rad;
        float radius = 0.5f;
        float moveDistance = radius * angleRad;
        transform.Translate(new Vector3(moveDistance, 0, 0));
        transform.Rotate(0, 0, Angle);
    }
}
