using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DrawLineSetting : MonoBehaviour
{
    UILineRenderer uiLineRenderer = null;
    Image owner = null;

    // Start is called before the first frame update
    void Start()
    {
        uiLineRenderer = GetComponent<UILineRenderer>();
        if(null != uiLineRenderer)
        {
            Vector3 startPos = owner.rectTransform.localPosition;
                   
        }
    }

    // Update is called once per frame
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
