using System.Collections;
using System.Collections.Generic;
using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AddUILinePoint : MonoBehaviour
{

    [Header("Essential Settings")]
    [SerializeField] GameObject anchor = null;

    UILineRenderer uiLineRenderer = null;
    public RectTransform targetRectTransform = null;
    RectTransform BrushRectTransform = null;
    List<Vector2> points = new List<Vector2>();

    void Start()
    {
        if(null == anchor)
        {
            Debug.Log("Anchor is not setting!!");
            return;
        }

        uiLineRenderer = anchor.GetComponent<UILineRenderer>();

        if (null != uiLineRenderer)
        {
            BrushRectTransform = GetComponent<RectTransform>();

            if (targetRectTransform != null)
            {
                transform.position = targetRectTransform.position;
            }
            else
            {
                Debug.Log("Rect Transform is not found");
            }
        }
    }

    void Update()
    {
        if (UIManager.Instance.isGameEnd)
            return;

        if (UIManager.Instance.IsTimeOut)
            return;

        if (UIManager.Instance.shapeType == UIManager.ShapeType.Max || UIManager.Instance.shapeType == UIManager.ShapeType.None)
            return;

        transform.position = targetRectTransform.position;

        AddPoint(transform.localPosition);
    }

    public void AddPoint(Vector2 newPoint)
    {
        points.Add(newPoint);
        uiLineRenderer.Points = points.ToArray();
    }

    public void ResetPoints() 
    {
        Debug.Log("Reset");
        transform.position = anchor.transform.position;
        points.Clear();
        uiLineRenderer.Points = points.ToArray();
    }
}
