using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 
/// Child 1 : Cycle Point
/// Child 2 : Pivot Point
/// 
/// </summary>

public class Shape : MonoBehaviour
{
    Vector2 StartPos = Vector2.zero;
    [SerializeField] float EndPos;
    
    [Header("Rect Information")]
    [SerializeField] Vector3[] CornerPosition = new Vector3[4];
    [SerializeField] GameObject[] Corners;
    [SerializeField] float LENGTH;
    [SerializeField] float Speed;
    [SerializeField] bool AutoMove = false;

    [SerializeField] RectTransform OutLineRenderUI;
    [SerializeField] RectTransform InLineRenderUI;

    LineRenderer outlineRenderer;
    LineRenderer InlineRenderer;
    void Start()
    {
        StartPos = transform.localPosition;

        CornerPosition[0] = Corners[0].transform.localPosition;
        CornerPosition[1] = Corners[1].transform.localPosition;
        CornerPosition[2] = Corners[2].transform.localPosition;
        CornerPosition[3] = Corners[3].transform.localPosition;

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
            RotateAndMoveRect(-10);

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

    void RotateAndMoveRect(float ANGLE) 
    {
        ChangeTransform(ANGLE);

        // Pivot.y 중심 회전
        transform.RotateAround(CornerPosition[4], Vector3.back, ANGLE * Time.deltaTime * Speed);
  

        if (OutLineRenderUI != null && InLineRenderUI != null)
        {
            DrawLineRenderer();
        }
    }

    void RotateAndMoveSphere(float ANGLE)
    {
        DrawLineRenderer();
    }

    void RotateAndMoveHexagon(float ANGLE)
    {
        DrawLineRenderer();
    }

    public void UpdateRotateAndLocation(float ANGLE) 
    {
        if (UIManager.Instance.shapeType == UIManager.ShapeType.None)
            return;

        switch (UIManager.Instance.shapeType)
        {
            case UIManager.ShapeType.Sphere:
                RotateAndMoveSphere(ANGLE);
                break;
            case UIManager.ShapeType.Rect:
                RotateAndMoveRect(ANGLE);
                break;
            case UIManager.ShapeType.Hexagon:
                RotateAndMoveHexagon(ANGLE);
                break;
        }
    }

    void ChangeTransform(float Angle) 
    {
        if (Angle > 0)
        {
            if (CornerPosition[3].x > CornerPosition[3].x) 
            {
                CornerPosition[2] = Corners[2].transform.position;
            }

            if (CornerPosition[3].y >= CornerPosition[2].y) 
            {
                CornerPosition[3] = Corners[2].transform.position;
                CornerPosition[2] = Corners[1].transform.position;
            }
        }
        else 
        {
            if (CornerPosition[3].x < CornerPosition[2].x)
            {
                CornerPosition[2] = Corners[0].transform.position;
            }

            if (CornerPosition[3].y <= CornerPosition[2].y)
            {
                CornerPosition[3] = Corners[0].transform.position;
                CornerPosition[0] = Corners[1].transform.position;
            }
        }
    }
}
