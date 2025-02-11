using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GridBrushBase;
using static UnityEngine.Rendering.ProbeTouchupVolume;

public class HexagonRotation : MonoBehaviour, MoveAndRotateInterface
{
    public enum CornerName
    {
        LB, // LeftBottom (0)
        LM, // LeftMiddle (1)
        LT, // LeftTop (2)
        RT, // RightTop (3)
        RM, // RightMiddle (4)
        RB  // RightBottom (5)
    }

    [Header("Property")]
    [SerializeField]
    [Tooltip("육각형 한변의 길이")]
    float length = 100f;
    [SerializeField]
    Image ownerImage = null;
    [SerializeField]
    Image leftBottomCorner = null;
    [SerializeField]
    Image rightBottomCorner = null;
    [SerializeField]
    Image rightMiddleCorner = null;
    [SerializeField]
    Image leftMiddleCorner = null;
    [SerializeField]
    Image leftTopCorner = null;
    [SerializeField]
    Image rightTopCorner = null;

    [SerializeField]
    [Tooltip("사이클로이드 곡선을 긋는 선")]
    Image cycloidPoint = null;
    [SerializeField]
    [Tooltip("회전 중심 점")]
    Image pivotPoint = null;
    [SerializeField]
    [Tooltip("바닥 충돌 점")]
    Image touchPoint = null;

    [SerializeField]
    [Tooltip("초당 회전 각도")]
    float speed = 10f;

    [Header("Status")]
    [SerializeField]
    [Tooltip("시작 위치")]
    Vector3 startPosition = Vector3.zero;
    [SerializeField]
    [Tooltip("현 위치")]
    Vector3 curPosition = Vector3.zero;

    [SerializeField]
    [Tooltip("총 회전각도")]
    float totalRotation = 0f;
    [SerializeField]
    float prevRotation = 0f;

    [Header("Properties")]
    [SerializeField]
    [Tooltip("초당 회전 각도 (speed)")]
    float rotationAnglePerSecond = 0f;
    [SerializeField]
    [Tooltip("회전 방향 ( true : right(→) , false : left(←) )")]
    bool rotationDirection = true;
    bool prevRotationDirection = true;

    [Header("Statuses")]
    [SerializeField]
    [Tooltip("현재 회전량")]
    float curRotation = 0f;
    [SerializeField]
    int cycloidPos = 0;
    [SerializeField]
    int pivotPos = 5;
    [SerializeField]
    int touchPos = 4;

    Image[] corners = new Image[6];
    Shape shape;

    void Start()
    {
        // 필요한 컴포넌트들이 설정되어 있는지 확인
        if (ownerImage == null ||
            leftBottomCorner == null ||
            rightBottomCorner == null ||
            rightMiddleCorner == null ||
            leftMiddleCorner == null ||
            rightTopCorner == null ||
            leftTopCorner == null)
        {
            throw new System.Exception("객체가 설정되지 않았습니다.");
        }

        // Status 초기화
        startPosition = ownerImage.rectTransform.localPosition;
        curPosition = startPosition;
        curRotation = ownerImage.rectTransform.eulerAngles.z;
        Debug.Log("A Cur : " + curRotation + ", Prev : " + prevRotation + ", Total : " + totalRotation);

        // 코너 배열의 순서를 LB, LM, LT, RT, RM, RB 로 설정
        corners[0] = leftBottomCorner;  // LB (240°)
        corners[1] = leftMiddleCorner;  // LM (180°)
        corners[2] = leftTopCorner;     // LT (120°)
        corners[3] = rightTopCorner;    // RT (60°)
        corners[4] = rightMiddleCorner; // RM (0°)
        corners[5] = rightBottomCorner; // RB (300°)

        rotationAnglePerSecond = 1f;

        shape = GetComponent<Shape>();
    }

    // 현재 회전각을 60도의 배수로 스냅하는 함수 (육각형 기준)
    float InterpolationAngle(float angle)
    {
        float snapInterval = 60f;
        // 현재 angle에서 가장 가까운 60° 배수로 목표 각도를 계산
        float targetAngle = Mathf.Round(angle / snapInterval) * snapInterval;
        // 보간 속도 (원하는 부드러움에 따라 조정 가능)
        float smoothingFactor = 10f;
        return Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * smoothingFactor);
    }

    int ModuloOperatorRight(int curValue)
    {
        int nextValue = (curValue - 1 + 6) % 6;
        return nextValue;
    }

    int ModuloOperatorLeft(int curValue)
    {
        int nextValue = (curValue + 1) % 6;
        return nextValue;
    }

    public void MoveAndRotate(int sensorDist)
    {
        // 센서 값에 따라 회전 방향 설정
        if (sensorDist < 0)
        {
            rotationDirection = false;
        }
        else if (sensorDist > 0)
        {
            rotationDirection = true;
        }

        // 회전 방향이 바뀌었을 때 터치 포인트의 위치를 갱신
        if (prevRotationDirection != rotationDirection)
        {
            if (rotationDirection == true)
            {
                // 오른쪽 회전일 경우: 터치 포인트 = pivot에서 한 칸 왼쪽 (모듈로 연산)
                touchPos = ModuloOperatorRight(pivotPos);
            }
            else
            {
                // 왼쪽 회전일 경우: 터치 포인트 = pivot에서 한 칸 오른쪽
                touchPos = ModuloOperatorLeft(pivotPos);
            }
            touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
        }

        // 회전 처리
        if (rotationDirection)
        {
            if (shape != null && shape.AutoMove == true)
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.back, Mathf.Abs(rotationAnglePerSecond) * Time.deltaTime * sensorDist * 50);
            }
            else
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.back, Mathf.Abs(rotationAnglePerSecond) * sensorDist * 50);
            }

            // 터치 포인트가 피벗 포인트의 수평 위치보다 아래로 내려가면 회전 보정 및 피벗/터치 위치 갱신
            if (touchPoint.transform.position.y <= pivotPoint.transform.position.y)
            {
                curRotation = ownerImage.rectTransform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(0, 0, InterpolationAngle(curRotation));

                pivotPos = ModuloOperatorRight(pivotPos);
                touchPos = ModuloOperatorRight(touchPos);

                pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
                touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;

                Vector3 ResetPosition = ownerImage.transform.localPosition;
                ResetPosition.y = 0;
                ResetPosition.z = 0;
                ownerImage.transform.localPosition = ResetPosition;
            }
        }
        else
        {
            if (shape != null && shape.AutoMove == true)
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.forward, Mathf.Abs(rotationAnglePerSecond) * Time.deltaTime * sensorDist * 50);
            }
            else
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.forward, Mathf.Abs(rotationAnglePerSecond) * sensorDist * 50);
            }

            if (touchPoint.transform.position.y <= pivotPoint.transform.position.y)
            {
                pivotPos = ModuloOperatorLeft(pivotPos);
                touchPos = ModuloOperatorLeft(touchPos);
                pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
                touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
                Vector3 ResetPosition = ownerImage.transform.localPosition;
                ResetPosition.y = 0;
                ResetPosition.z = 0;
                ownerImage.transform.localPosition = ResetPosition;
            }
        }

        curRotation = ownerImage.rectTransform.rotation.eulerAngles.z;
        prevRotationDirection = rotationDirection;
    }

    public void InitPivotPoint()
    {
        // 필요시 pivot, cycloid, touch 포인트의 초기 위치를 설정합니다.

        cycloidPos = (int)CornerName.LB;
        pivotPos = (int)CornerName.RB;
        touchPos = (int)CornerName.RM;

        cycloidPoint.rectTransform.localPosition = corners[cycloidPos].rectTransform.localPosition;
        pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
        touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
    }
}
