using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineBrushMove : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float curAngle = 0f;
    [SerializeField] float curY = 0f;
    [SerializeField] float curX = 0f;
    [SerializeField] float degPerSec = 90f;
    [SerializeField] float moveDistY = 50f;
    [SerializeField] float moveSpeedX = 50f;

    RectTransform rectTrasnform = null;

    void Start()
    {
        rectTrasnform = GetComponent<RectTransform>();
        if (null == rectTrasnform)
        {
            Debug.Log("There is no RectTransform");
        }

        moveDistY = 50f;
        moveSpeedX = 50f;
    }

    void Update()
    {
        if (null == rectTrasnform)
            return;

        //Move();
    }

    private void Move()
    {
        // Current Angle Raise
        curAngle += (degPerSec * Time.deltaTime);
        float degToRad = curAngle * Mathf.Deg2Rad ;

        // Get PosY
        float posY = Mathf.Sin(degToRad);
        if(posY > 0)
        {
            posY = posY < 1e-8 ? 0f : posY;
        }
        else if(posY < 0)
        {
            posY = posY > -1e-8 ? 0f : posY;
        }
        curY = posY * moveDistY;

        // Get PosX
        curX += moveSpeedX * Time.deltaTime;

        // Set PosY
        rectTrasnform.localPosition = new Vector2(curX, curY);

    }
}
