using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DrawLineRenderer : MonoBehaviour
{

    [Header("Target Information")]
    public RectTransform targetRectTransform;   // shape's renderer dot
    public RectTransform targetParentRectTransform;

    [Header("UI Line Renderer Setting Data")]
    [SerializeField] UILineRenderer uiLineRenderer = null;
    List<Vector2> points = new List<Vector2>();


    void Start()
    {
        if (targetRectTransform != null)
        {
            transform.position = targetRectTransform.position;
        }

        uiLineRenderer = GetComponent<UILineRenderer>();

        if (null != uiLineRenderer)
        {

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
        if (UIManager.Instance.isGameEnd || UIManager.Instance.IsTimeOut)
            return;

        Debug.Log(targetRectTransform.localPosition + targetParentRectTransform.localPosition);
        AddPoint(targetRectTransform.position);
    }

    public void AddPoint(Vector2 newPoint)
    {
        points.Add(newPoint);
        uiLineRenderer.Points = points.ToArray();
    }
}
