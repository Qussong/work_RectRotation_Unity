using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    
    public void MoveAndRotate(int sensorDist) 
    {
        Debug.Log(sensorDist);

        float radius = 0.5f;
        float ratio = 33 / 2 * Mathf.PI * radius;
        float rotationAngle = 360 * ratio;
        int amount = Mathf.Abs(sensorDist);
        Vector3 targetPosition = new Vector3(0, 0, 0);

        if (sensorDist < 0)
        {
            targetPosition.x = 50 * amount;
            transform.Translate(targetPosition, Space.World);
            transform.Rotate(0, 0, -rotationAngle);
        }
        else
        {
            targetPosition.x = -50 * amount;
            transform.Translate(targetPosition, Space.World);
            transform.Rotate(0, 0, rotationAngle);
        }
    }

    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }
}
