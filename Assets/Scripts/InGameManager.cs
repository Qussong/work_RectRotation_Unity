using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.ProbeTouchupVolume;

public class InGameManager : MonoBehaviour
{
    [SerializeField] Image goalimage;
    [SerializeField] Sprite[] goalSprites;
    [SerializeField] GoalManager goalManager;
    [SerializeField] GameObject[] shapes;
    [SerializeField] float Timer;
    [SerializeField] Transform StartPosition;
    [SerializeField] TimerManager timermanager;
    [SerializeField] AddUILinePoint AddLineRendererIn;
    [SerializeField] AddUILinePoint AddLineRendererOut;
    GameObject InstantiateGameObject;

    private void OnDisable()
    {
        if (InstantiateGameObject != null)
        {
            Destroy(InstantiateGameObject);
        }
    }

    public void DestoryShapeGameObject() 
    {

    }

    public void GameStart() 
    {
        Vector3 StartPos = StartPosition.position;

        switch (UIManager.Instance.shapeType) 
        {
            case UIManager.ShapeType.Sphere:
                goalimage.sprite = goalSprites[0];
                InstantiateGameObject = Instantiate(shapes[0], StartPos, Quaternion.identity, transform);
                break;
            case UIManager.ShapeType.Rect:
                goalimage.sprite = goalSprites[1];
                InstantiateGameObject = Instantiate(shapes[1], StartPos, Quaternion.identity, transform);
                break;
            case UIManager.ShapeType.Hexagon:
                goalimage.sprite = goalSprites[2];
                InstantiateGameObject = Instantiate(shapes[2], StartPos, Quaternion.identity, transform);
                break;
            case UIManager.ShapeType.Max:
                return;
        }

        InstantiateGameObject.transform.SetSiblingIndex(3);
        UIManager.Instance.shape = InstantiateGameObject.GetComponent<Shape>();
        goalManager.Shape = InstantiateGameObject.GetComponent<Shape>();
        AddLineRendererIn.targetRectTransform = InstantiateGameObject.GetComponent<Shape>().InLineRenderUI;
        AddLineRendererOut.targetRectTransform = InstantiateGameObject.GetComponent<Shape>().OutLineRenderUI;
        AddLineRendererIn.ResetPoints();
        AddLineRendererOut.ResetPoints();

        timermanager.StartTimer();
    }
}
