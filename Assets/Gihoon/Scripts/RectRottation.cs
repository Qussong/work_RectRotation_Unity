using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RectRottation : MonoBehaviour
{
    public enum CornerName
    {
        LB, // LeftBottom (0)
        LT, // LeftTop (1)
        RT, // RightTop (2)
        RB  // RightBottom (3)
    }

    [Header("Property")]
    [SerializeField] Image rectImage = null;
    [SerializeField] Image leftBottomCorner = null;
    [SerializeField] Image leftTopCorner = null;
    [SerializeField] Image rightTopCorner = null;
    [SerializeField] Image rightBottomCorner = null;
    [SerializeField][Tooltip("사이클로이드 곡선을 긋는 선")] Image cycloidPoint = null;
    [SerializeField][Tooltip("회전 중심 점")] Image pivotPoint = null;
    [SerializeField][Tooltip("회전 판별 점")] Image rightPoint = null;

    [Header("Status")]
    [SerializeField][Tooltip("시작 지점")] Vector3 StartPoint = Vector3.zero;
    [SerializeField][Tooltip("현재 회전량")] float curRotation = 0f;
    [SerializeField][Tooltip("초당 회전 각도")] float rotationAngle = 0f;
    [SerializeField] int cycloidPos = -1;
    [SerializeField] int pivotPos = -1;
    [SerializeField] int rightPos = -1;

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

        cycloidPos = (int)CornerName.LB;
        pivotPos = (int)CornerName.RB;
        rightPos = (int)CornerName.RT;

        cycloidPoint.rectTransform.localPosition = corners[cycloidPos].rectTransform.localPosition;
        pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
        rightPoint.rectTransform.localPosition = corners[rightPos].rectTransform.localPosition;

        rotationAngle = 10;
    }

    void Update()
    {
        rectImage.rectTransform.RotateAround(pivotPoint.rectTransform.position, Vector3.back, rotationAngle * Time.deltaTime);
   
        curRotation = rectImage.rectTransform.rotation.eulerAngles.z;
        if (rightPoint.transform.position.y <= pivotPoint.transform.position.y)
        {
            rectImage.rectTransform.rotation = Quaternion.Euler(0, 0, InterpolationAngle(curRotation));

            pivotPos = ModuloOperator(pivotPos);
            rightPos = ModuloOperator(rightPos);
            pivotPoint.rectTransform.localPosition = corners[pivotPos].rectTransform.localPosition;
            rightPoint.rectTransform.localPosition = corners[rightPos].rectTransform.localPosition;

            //Time.timeScale = 0;
        }
    }

    float InterpolationAngle(float angle)
    {
        float result = 0f;

        if (angle <= 90) result = 90f;
        else if (angle <= 180) result = 180f;
        else if (angle <= 2700) result = 270f;
        else result = 360f;

        return result;
    }

    int ModuloOperator(int curValue)
    {
        int nextValue = (curValue - 1 + 4) % 4;
        return nextValue;
    }
}
