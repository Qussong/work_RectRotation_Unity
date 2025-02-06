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
    RectTransform rectTransform = null;
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
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 startPos = rectTransform.localPosition;
            }
            else
            {
                Debug.Log("Rect Transform is not found");
            }
        }
    }

    void Update()
    {
        AddPoint(rectTransform.localPosition);
    }

    public void AddPoint(Vector2 newPoint)
    {
        points.Add(newPoint);
        uiLineRenderer.Points = points.ToArray();
    }
}
