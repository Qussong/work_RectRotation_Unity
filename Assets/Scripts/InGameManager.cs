using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.ProbeTouchupVolume;

public class InGameManager : MonoBehaviour
{
    [SerializeField] Shape shape;
    [SerializeField] GameObject Goal;
    [SerializeField] Sprite[] goalSprites;
    [SerializeField] GameObject[] shapes;
    [SerializeField] float Timer;
    [SerializeField] Transform StartPosition;

    Image goalimage;
    TimerManager timermanager;
    GameObject InstantiateGameObject;

    private void Start()
    {
        goalimage = Goal.GetComponent<Image>();
        timermanager = GetComponentInChildren<TimerManager>();
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
        
        timermanager.StartTimer();
    }
}
