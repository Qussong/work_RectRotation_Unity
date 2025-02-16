using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    [SerializeField] float MoveSpeed;      // 필요시 사용
    [SerializeField] float RotationSpeed;  // 필요시 사용

    public void MoveAndRotate(int sensorDist)
    {
        float radiusPx = 216.5f; // 구(원)의 반지름
        float distancePerStep = 60f; // 한 단계(sensorDist 1)에 이동할 거리

        // 이동해야 할 총 거리
        float distance = distancePerStep * sensorDist * -1;
        transform.Translate(distance, 0, 0, Space.World);

        float distancePx = distancePerStep * sensorDist;  // 실제 이동 픽셀
                                                            // transform.Translate(distancePx, 0, 0, Space.Self/World) 
                                                            // => UI에서는 결국 RectTransform의anchoredPosition이 바뀔 텐데,
                                                            //    이 부분도 픽셀 좌표로 해석됨

        float rotationAngleDeg = (distancePx / radiusPx) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, rotationAngleDeg);
    }


    public void InitPivotPoint()
    {
        // pivot, cycloid, touch point init (option for title scene)
    }
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

