using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
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
    GameObject InstantiateGameObject;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        if (InstantiateGameObject != null) 
        {
            Destroy(InstantiateGameObject);
        }
    }

    public void GameStart() 
    {
        switch (UIManager.Instance.shapeType) 
        {
            case UIManager.ShapeType.Sphere:
                goalimage.sprite = goalSprites[0];
                InstantiateGameObject = Instantiate(shapes[0], StartPosition);
                break;
            case UIManager.ShapeType.Rect:
                goalimage.sprite = goalSprites[1];
                InstantiateGameObject = Instantiate(shapes[1], StartPosition);
                break;
            case UIManager.ShapeType.Hexagon:
                goalimage.sprite = goalSprites[2];
                InstantiateGameObject = Instantiate(shapes[2], StartPosition);
                break;
        }

        UIManager.Instance.shape = InstantiateGameObject.GetComponent<Shape>();
        goalManager.Shape = InstantiateGameObject.GetComponent<Shape>();
        timermanager.StartTimer();
    }
}
