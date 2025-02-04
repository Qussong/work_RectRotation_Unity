using DG.Tweening;
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
        //transform.RotateAround(CornerPosition[3], Vector3.back, ANGLE * Time.deltaTime * Speed);

        Vector2 CurrentLoction = transform.localPosition;
        CurrentLoction.x += ANGLE;

        transform.localPosition = CurrentLoction;

        if (OutLineRenderUI != null && InLineRenderUI != null)
        {
            DrawLineRenderer();
        }
    }

    void RotateAndMoveSphere(float ANGLE)
    {
        transform.Translate(new Vector3(ANGLE, 0, 0));
        transform.Rotate(0, 0, ANGLE);
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
        }
        else
        {
        }

    }
}
