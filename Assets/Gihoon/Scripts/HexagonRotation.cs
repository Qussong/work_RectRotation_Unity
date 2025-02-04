using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HexagonRotation : MonoBehaviour
{
    [Header("Property")]
    [SerializeField]
    [Tooltip("육각형 한변의 길이")]
    float length = 0f;
    [SerializeField]
    Image ownerImage = null;
    [SerializeField]
    Image leftBottomCorner = null;
    [SerializeField]
    Image rightBottomCorner = null;
    [SerializeField]
    Image rightMiddleCorner = null;
    [SerializeField]
    [Tooltip("초당 회전 각도")]
    float speed = 10f;

    [Header("Status")]
    [SerializeField]
    [Tooltip("시작 위치")]
    Vector3 startPosition = Vector3.zero;
    [SerializeField]
    [Tooltip("현 위치")]
    Vector3 curPosition = Vector3.zero;
    [SerializeField]
    [Tooltip("총 회전각도")]
    float totalRotation = 0f;
    [SerializeField]
    float curRotation = 0f;
    [SerializeField]
    float prevRotation = 0f;


    void Start()
    {
        // Property
        if (null == ownerImage
            || null == leftBottomCorner
            || null == rightBottomCorner
            || null == rightPoint)
        {
            throw new System.Exception("객체가 설정되지 않았습니다.");
        }
        else
        {
            length = ownerImage.rectTransform.rect.height / Mathf.Sqrt(3);
            // pivot
            float newX = startPosition.x + length/2;
            float newY = startPosition.y - Mathf.Sqrt(3) * length / 2;
            float newZ = startPosition.z;
            rightBottomCorner.rectTransform.localPosition = new Vector3(newX, newY, newZ);
            // right
            newX = startPosition.x + length;
            newY = startPosition.y;
            newZ = startPosition.z;
            rightPoint.rectTransform.localPosition = new Vector3(newX, newY, newZ);
            // cycle
            newX = startPosition.x - length / 2;
            newY = startPosition.y - Mathf.Sqrt(3) * length / 2;
            newZ = startPosition.z;
            leftBottomCorner.rectTransform.localPosition = new Vector3(newX, newY, newZ);
        }

        // Status
        startPosition = ownerImage.rectTransform.localPosition;
        curPosition = startPosition;
        curRotation = ownerImage.rectTransform.eulerAngles.z;
        Debug.Log("A Cur : " + curRotation + ", Prev : " + prevRotation + ", Total : " + totalRotation);
    }

    void Update()
    {
        // pivotPoint 를 축으로 회전
        ownerImage.transform.RotateAround(rightBottomCorner.transform.position, Vector3.back, speed * Time.deltaTime);

        // pivoitPoint.y >= rightPoint.y 이라면
/*        if(pivotPoint.transform.position.y >= rightPoint.transform.position.y)
        {
            // 회전값 정렬
            
            // rightPoint, pivotPoint 위치 설정

        }*/

        curPosition = ownerImage.rectTransform.localPosition;
        p 
        curRotation = ownerImage.rectTransform.eulerAngles.z;
        Debug.Log("B Cur : " + curRotation + ", Prev : " + prevRotation + ", Total : " + totalRotation);
        totalRotation += (prevRotation - curRotation);
        Debug.Log("C Cur : " + curRotation + ", Prev : " + prevRotation + ", Total : " + totalRotation);



    }
}
