using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    [SerializeField] float MoveSpeed = 10;
    [SerializeField] float RotationSpeed = 10;

    Vector3 MovePosition = new Vector3(0, 0, 0);
    
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
            targetPosition.x = 60 * amount;
            rotationAngle *= -1;
        }
        else
        {
            targetPosition.x = -60 * amount;
            //transform.Translate(targetPosition, Space.World);
            //transform.Rotate(0, 0, rotationAngle);
        }

        transform.Translate(targetPosition, Space.World);
        transform.Rotate(0, 0, rotationAngle);
        //StartCoroutine(Move(targetPosition));
        //StartCoroutine(Rotate(rotationAngle));
    }

    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }

/*    IEnumerator Rotate(float TargetRotate)
    {
        float startRotation = transform.eulerAngles.z;
        float targetRotation = TargetRotate;
        float rotationAngle = Mathf.Abs(targetRotation - startRotation);

        // 회전 방향에 따라 부호를 결정
        if (rotationAngle > 180)
        {
            targetRotation -= 360;
        }

        while (Mathf.Abs(targetRotation - transform.eulerAngles.z) > 1.0f)
        {
            float step = RotationSpeed * Time.deltaTime;
            float currentRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotation, step);
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
    }

    IEnumerator Move(Vector3 targetLocation)
    {
        while (Vector3.Distance(transform.position, targetLocation) > 1.0f)
        {
            MovePosition.x = Mathf.Lerp(transform.position.x, targetLocation.x, Time.deltaTime * MoveSpeed);
            transform.Translate(MovePosition, Space.World);

            yield return null;
        }

        transform.position = targetLocation;
    }*/

}
