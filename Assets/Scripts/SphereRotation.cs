using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    public void MoveAndRotate(int sensorDist) 
    {
        /*float angleRad = Angle * Mathf.Deg2Rad;
        float radius = 0.5f;

        Angle /= Angle;
        float moveDistance = Angle * 33;

        transform.Translate(new Vector3(33, 0, 0), Space.World);
        transform.Rotate(0, 0, Angle);*/

        MoveSphere(sensorDist);
    }
    private void MoveSphere(int sensorDist)
    {
        float radius = 0.5f;
        float ratio = 33 / 2 * Mathf.PI * radius;
        float rotationAngle = 360 * ratio;

        if (sensorDist > 0)
        {
            transform.Translate(new Vector3(33 * sensorDist, 0, 0), Space.World);
            transform.Rotate(0, 0, -rotationAngle);
        }
        else
        {
            transform.Translate(new Vector3(-33 * sensorDist, 0, 0), Space.World);
            transform.Rotate(0, 0, rotationAngle);
        }


    }

    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }
}
