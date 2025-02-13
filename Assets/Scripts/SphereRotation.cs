using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float RotationSpeed;
    
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
            targetPosition.x = -50 * amount;
            StartCoroutine(Move(targetPosition));
            StartCoroutine(Rotate(-rotationAngle));
        }
        else
        {
            targetPosition.x = 50 * amount;
            StartCoroutine(Move(targetPosition));
            StartCoroutine(Rotate(rotationAngle));
        }
    }

    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }

    IEnumerator Rotate(float TargetRotate) 
    {
        while (true)
        {
            float RotateAngle = 0.0f;

            while (Mathf.Abs(TargetRotate - transform.rotation.z) > 1.0f)
            {
                RotateAngle = Mathf.LerpAngle(transform.rotation.z, TargetRotate, Time.deltaTime * RotationSpeed);
                transform.Rotate(0, 0, RotateAngle);

                yield return null;
            }

            transform.rotation = Quaternion.Euler(0, 0, TargetRotate);
        }
    }

    IEnumerator Move(Vector3 targetLocation) 
    {
        Vector3 MovePosition = new Vector3(0, 0, 0);

        while (Vector3.Distance(targetLocation, transform.position) > 1.0f)
        {
            float XPosition = Mathf.Lerp(transform.position.x, targetLocation.x, Time.deltaTime * MoveSpeed);
            MovePosition.x = XPosition;
            transform.Translate(MovePosition, Space.World);

            yield return null;
        }

        transform.position = targetLocation;
    }
}
