using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereRotation : MonoBehaviour, MoveAndRotateInterface
{
    [SerializeField] float MoveSpeed;      // �ʿ�� ���
    [SerializeField] float RotationSpeed;  // �ʿ�� ���

    public void MoveAndRotate(int sensorDist)
    {
        float radiusPx = 216.5f; // ��(��)�� ������
        float distancePerStep = 60f; // �� �ܰ�(sensorDist 1)�� �̵��� �Ÿ�

        // �̵��ؾ� �� �� �Ÿ�
        float distance = distancePerStep * sensorDist * -1;
        transform.Translate(distance, 0, 0, Space.World);

        float distancePx = distancePerStep * sensorDist;  // ���� �̵� �ȼ�
                                                            // transform.Translate(distancePx, 0, 0, Space.Self/World) 
                                                            // => UI������ �ᱹ RectTransform��anchoredPosition�� �ٲ� �ٵ�,
                                                            //    �� �κе� �ȼ� ��ǥ�� �ؼ���

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

        // ȸ�� ���⿡ ���� ��ȣ�� ����
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

