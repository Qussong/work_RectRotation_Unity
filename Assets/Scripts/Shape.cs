using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.GridBrushBase;
using static UnityEngine.Rendering.ProbeTouchupVolume;

/// <summary>
/// 
/// Child 1 : Cycle Point
/// Child 2 : Pivot Point
/// 
/// </summary>

public class Shape : MonoBehaviour
{
    public enum CornerName
    {
        LB, // LeftBottom (0)
        LT, // LeftTop (1)
        RT, // RightTop (2)
        RB  // RightBottom (3)
    }

    [Header("Title Rect Property")]
    [SerializeField] public bool AutoMove = false;
    [SerializeField] Vector3 StartPos;
    [SerializeField] float EndPos;

    [Header("Line Render")]
    [SerializeField] public RectTransform OutLineRenderUI;
    [SerializeField] public RectTransform InLineRenderUI;

    [SerializeField] MoveAndRotateInterface moveAndRotateInterface;

    LineRenderer outlineRenderer;
    LineRenderer InlineRenderer;

    private void OnDisable()
    {
        if (AutoMove)
        {
            transform.localPosition = StartPos;
            transform.rotation = Quaternion.identity;
        }
    }

    void Start()
    {
        StartPos = transform.localPosition;

        if (OutLineRenderUI != null && InLineRenderUI)
        {

            outlineRenderer = OutLineRenderUI.GetComponent<LineRenderer>();
            InlineRenderer = InLineRenderUI.GetComponent<LineRenderer>();

            outlineRenderer.positionCount = 0;
            outlineRenderer.startWidth = 0.05f;
            outlineRenderer.endWidth = 0.05f;

            InlineRenderer.positionCount = 0;
            InlineRenderer.startWidth = 0.05f;
            InlineRenderer.endWidth = 0.05f;
        }
    }

    void Update()
    {
        if (AutoMove) 
        {
           if (moveAndRotateInterface == null)
           {
               moveAndRotateInterface = gameObject.GetComponent<MoveAndRotateInterface>();
           }

           moveAndRotateInterface.MoveAndRotate(20);
           
            if (transform.localPosition.x >= EndPos)
            {
                // Origin Settings
                transform.localPosition = StartPos;
                transform.rotation = Quaternion.identity;
                moveAndRotateInterface.InitPivotPoint();

            }
        }
    }

    public void UpdateRotateAndLocation(int sensorDist) 
    {
        if (UIManager.Instance.shapeType == UIManager.ShapeType.None)
            return;

        if (moveAndRotateInterface == null)
        {
            moveAndRotateInterface = gameObject.GetComponent<MoveAndRotateInterface>();
        }

        moveAndRotateInterface.MoveAndRotate(sensorDist);
    }
}
