using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.GridBrushBase;

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
    [SerializeField] bool AutoMove = false;
    [SerializeField] Vector3 StartPos;
    [SerializeField] float EndPos;

    [Header("Line Render")]
    [SerializeField] RectTransform OutLineRenderUI;
    [SerializeField] RectTransform InLineRenderUI;

    [SerializeField] MoveAndRotateInterface moveAndRotateInterface;

    LineRenderer outlineRenderer;
    LineRenderer InlineRenderer;
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

           moveAndRotateInterface.MoveAndRotate(10);

           DrawLineRenderer();
           
            if (transform.localPosition.x >= EndPos)
            {
                transform.localPosition = StartPos;
                transform.rotation = Quaternion.identity;
            }
        }
    }

    void DrawLineRenderer() 
    {
        Vector3 worldOutlinePos = Camera.main.ScreenToWorldPoint(OutLineRenderUI.position);
        Vector3 worldInlinePos = Camera.main.ScreenToWorldPoint(InLineRenderUI.position);
        worldOutlinePos.z = 0;
        worldInlinePos.z = 0;

        outlineRenderer.positionCount++;
        outlineRenderer.SetPosition(outlineRenderer.positionCount - 1, worldOutlinePos);

        InlineRenderer.positionCount++;
        InlineRenderer.SetPosition(InlineRenderer.positionCount - 1, worldInlinePos);
    }

    public void UpdateRotateAndLocation(float ANGLE) 
    {
        if (UIManager.Instance.shapeType == UIManager.ShapeType.None)
            return;

  
        if (moveAndRotateInterface == null)
        {
            moveAndRotateInterface = gameObject.GetComponent<MoveAndRotateInterface>();
        }

        moveAndRotateInterface.MoveAndRotate(10);

        DrawLineRenderer();
       
    }
}
