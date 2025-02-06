using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class RectRottation : MonoBehaviour, MoveAndRotateInterface
{
    Shape shape = null;

    public enum CornerName
    {
        LB, // LeftBottom (0)
        LT, // LeftTop (1)
        RT, // RightTop (2)
        RB  // RightBottom (3)
    }

    [Header("Essential Settings")]
    [SerializeField] Image ownerImage = null;
    [SerializeField] Image leftBottomCorner = null;
    [SerializeField] Image leftTopCorner = null;
    [SerializeField] Image rightTopCorner = null;
    [SerializeField] Image rightBottomCorner = null;
    [SerializeField][Tooltip("사이클로이드 곡선을 긋는 선")] Image cycloidPoint = null;
    [SerializeField][Tooltip("회전 중심 점")] Image pivotPoint = null;
    [SerializeField][Tooltip("바닥 충돌 점")] Image touchPoint = null;
    private float sideLength = 0;

    [Header("Properties")]
    [SerializeField][Tooltip("초당 회전 각도 (speed)")] float rotationAnglePerSecond = 0f;
    [SerializeField][Tooltip("회전 방향 ( true : right(→) , false : left(←) )")] bool rotationDirection = true;
    bool prevRotationDirection = true;

    [Header("Statuses")]
    [SerializeField][Tooltip("시작 지점")] Vector3 StartPoint = Vector3.zero;
    [SerializeField][Tooltip("현재 회전량")] float curRotation = 0f;
    [SerializeField] int cycloidPos = -1;
    [SerializeField] int pivotPos = -1;
    [SerializeField] int touchPos = -1;

    Image[] corners = new Image[4];

    void Start()
    {
        if (null == leftTopCorner ||
            null == leftBottomCorner ||
            null == rightTopCorner ||
            null == rightBottomCorner)
        {
            throw new System.Exception("설정이 완료되지 않았습니다.");
        }

        corners[0] = leftBottomCorner;  // Corner.LB
        corners[1] = leftTopCorner;     // Corner.LT
        corners[2] = rightTopCorner;    // Corner.RT
        corners[3] = rightBottomCorner; // Corner.RB

        /*
        cycloidPos = (int)CornerName.LB;
        pivotPos = (int)CornerName.RB;
        touchPos = (int)CornerName.RT;

        cycloidPoint.rectTransform.localPosition = corners[cycloidPos].rectTransform.localPosition;
        pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
        touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
        */
        InitPivotPoint();

        sideLength = ownerImage.rectTransform.rect.width;
        rotationAnglePerSecond = (33 / (4 * sideLength)) * 360; // rotation angle per one sensor = (unit move dis / (4 * side length)) * 360 degree

        shape = GetComponent<Shape>();

        StartPoint = ownerImage.rectTransform.localPosition;
    }

    void Update()
    {

    }

    float InterpolationAngle(float angle)
    {
        float snapInterval = 90f;
        // 현재 angle에서 가장 가까운 90° 배수를 목표 각도로 계산
        float targetAngle = Mathf.Round(angle / snapInterval) * snapInterval;
        // 보간 속도 (필요에 따라 조정)
        float smoothingFactor = 10f;
        return Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * smoothingFactor);
    }

    int ModuloOperatorRight(int curValue)
    {
        int nextValue = (curValue - 1 + 4) % 4;
        return nextValue;
    }

    int ModuloOperatorLeft(int curValue)
    {
        int nextValue = (curValue + 1) % 4;
        return nextValue;
    }

    public void MoveAndRotate(int sensorDist)
    {

        if (sensorDist < 0)
        {
            rotationDirection = false;
        }
        else if (sensorDist > 0) 
        {
            rotationDirection = true;
        }

        //rotationAnglePerSecond = sensorDist;

        // When rotation direction change
        if (prevRotationDirection != rotationDirection)
        {
            // When new rotation direction is RIGHT
            if (rotationDirection == true)
            {
                // Touch Point Position = pivot - 1
                touchPos = ModuloOperatorRight(pivotPos);
            }
            // When new rotation direction is LEFT
            else
            {
                // Touch Point Position = pivot + 1
                touchPos = ModuloOperatorLeft(pivotPos);
            }

            touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
        }


        // Rotation Direction : right
        if (rotationDirection)
        {
            if (shape != null && shape.AutoMove == true)
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.back, Mathf.Abs(rotationAnglePerSecond) * Time.deltaTime * sensorDist);
            }
            else
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.back, Mathf.Abs(rotationAnglePerSecond)/* * Time.deltaTime*/ * sensorDist);
            }

            // When the TouchPoint touch ground
            if (touchPoint.transform.position.y < pivotPoint.transform.position.y)
            {
                //rectImage.rectTransform.rotation = Quaternion.Euler(0, 0, InterpolationAngle(curRotation));
                curRotation = ownerImage.rectTransform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(0, 0, InterpolationAngle(curRotation));

                pivotPos = ModuloOperatorRight(pivotPos);
                touchPos = ModuloOperatorRight(touchPos);

                pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
                touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;

                Vector3 ResetPosition = ownerImage.transform.localPosition;
                //ResetPosition.y = 0;
                ResetPosition.y = StartPoint.y;
                ResetPosition.z = 0;
                ownerImage.transform.localPosition = ResetPosition;
            }
        }
        // Rotation Direction : left
        else
        {
            if(shape != null && shape.AutoMove == true)
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.forward, Mathf.Abs(rotationAnglePerSecond) * Time.deltaTime * sensorDist);
            }
            else
            {
                ownerImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.forward, Mathf.Abs(rotationAnglePerSecond) /** Time.deltaTime*/ * sensorDist);
            }


            // When the TouchPoint touch ground
            if (touchPoint.transform.position.y <= pivotPoint.transform.position.y)
            {
                //rectImage.rectTransform.rotation = Quaternion.Euler(0, 0, InterpolationAngle(curRotation));

                pivotPos = ModuloOperatorLeft(pivotPos);
                touchPos = ModuloOperatorLeft(touchPos);
                pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
                touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;

                Vector3 ResetPosition = ownerImage.transform.localPosition;
                //ResetPosition.y = 0;
                ResetPosition.y = StartPoint.y;
                ResetPosition.z = 0;
                ownerImage.transform.localPosition = ResetPosition;
            }
        }

        curRotation = ownerImage.rectTransform.rotation.eulerAngles.z;
        prevRotationDirection = rotationDirection;
    }

    public void InitPivotPoint()
    {
        cycloidPos = (int)CornerName.LB;
        pivotPos = (int)CornerName.RB;
        touchPos = (int)CornerName.RT;

        cycloidPoint.rectTransform.localPosition = corners[cycloidPos].rectTransform.localPosition;
        pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
        touchPoint.rectTransform.localPosition = corners[touchPos].rectTransform.localPosition;
    }

}
