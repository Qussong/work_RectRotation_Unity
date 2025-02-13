using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float RotationSpeed;

    Vector3 MovePosition = new Vector3(0, 0, 0);
    Vector3 targetPosition = new Vector3(0, 0, 0);
    private void Start()
    {
        MovePosition.y = transform.position.y;
        MovePosition.z = transform.position.z;
    }
    public void MoveAndRotate(int sensorDist) 
    {
        Debug.Log(sensorDist);

        float radius = 0.5f;
        float ratio = 33 / 2 * Mathf.PI * radius;
        float rotationAngle = 360 * ratio;
        int amount = Mathf.Abs(sensorDist);

        if (sensorDist < 0)
        {
            targetPosition.x = 60f * amount;
            rotationAngle *= -1;
        }
        else
        {
            targetPosition.x = -60f * amount;
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
            float currentRotation = Mathf.Lerp(transform.eulerAngles.z, targetRotation, step);
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
    }

    IEnumerator Move(Vector3 targetLocation)
    {
        targetLocation.x += transform.position.x;
        MovePosition.y = transform.position.y;

        Debug.Log(Mathf.Abs(transform.position.x - targetLocation.x));

        while (Mathf.Abs(transform.position.x - targetLocation.x) > 1.0f)
        {
            MovePosition.x = Mathf.Lerp(transform.position.x, targetLocation.x, Time.deltaTime * MoveSpeed);
            transform.position = MovePosition;

            yield return null;
        }

        transform.position = targetLocation;
    }*/

}
