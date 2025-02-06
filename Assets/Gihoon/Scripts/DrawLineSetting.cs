using System.Collections;
using System.Collections.Generic;
using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DrawLineSetting : MonoBehaviour
{
    UILineRenderer uiLineRenderer = null;
    GameObject owner = null;

    void Start()
    {
        uiLineRenderer = GetComponent<UILineRenderer>();
        if(null != uiLineRenderer)
        {
            //Vector3 startPos = owner.rectTransform.localPosition;
                   
        }
    }

    void Update()
    {
        
    }

    public void AddPoint(Vector2 newPoint)
    {

        if (0 == uiLineRenderer.Points.Length)
        {
        }

    }
}
