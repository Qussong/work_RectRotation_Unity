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
    public enum ShapeType
    {
        None,
        Sphere,
        Rect,
        Hexagon
    }

    public ShapeType Type;

    Vector2 StartPos = Vector2.zero;
    [SerializeField] float EndPos;
    
    Vector2 ChildStartPos = Vector2.zero;

    float ANGLE = -10.0f;
    Vector2 Pivot = Vector2.zero;

    [Header("Rect Information")]
    Transform CycleTransform;
    Transform PivotTransform;
    Transform RightTransform;
    [SerializeField] float LENGTH;
    [SerializeField] float Speed;

    [SerializeField] RectTransform OutLineRenderUI;
    [SerializeField] RectTransform InLineRenderUI;

    LineRenderer outlineRenderer;
    LineRenderer InlineRenderer;

    Vector2 PrevPosition;
    Vector2 NewPosition;
    void Start()
    {
        StartPos = transform.localPosition;
        Debug.Log(StartPos);

        ChildStartPos = transform.GetChild(0).position;
        Debug.Log(ChildStartPos);

        CycleTransform = transform.GetChild(0);
        PivotTransform = transform.GetChild(1);
        RightTransform = transform.GetChild(2);

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
        if (Type == ShapeType.None)
            return;

        switch (Type)
        {
            case ShapeType.Sphere:
                RotateAndMoveSphere();
                break;
            case ShapeType.Rect:
                RotateAndMoveRect();
                break;
            case ShapeType.Hexagon:
                RotateAndMoveHexagon();
                break;
        }
        Debug.Log(transform.position.x + " , " + EndPos);

        if (transform.localPosition.x >= EndPos) 
        {
            transform.localPosition = StartPos;
            transform.rotation = Quaternion.identity;
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

    void RotateAndMoveRect() 
    {
        if (RightTransform.position.y <= PivotTransform.position.y)
        {
            RightTransform.position = new Vector3(transform.position.x + LENGTH, transform.position.y + LENGTH, -1);
            PivotTransform.position = new Vector3(transform.position.x + LENGTH, transform.position.y - LENGTH, -1);
        }
        // Pivot.y 중심 회전
        transform.RotateAround(PivotTransform.position, Vector3.forward, ANGLE * Time.deltaTime * Speed);
        if (OutLineRenderUI != null && InLineRenderUI != null)
        {
            DrawLineRenderer();
        }
    }

    void RotateAndMoveSphere()
    {
        DrawLineRenderer();
    }

    void RotateAndMoveHexagon()
    {
        DrawLineRenderer();
    }
}
