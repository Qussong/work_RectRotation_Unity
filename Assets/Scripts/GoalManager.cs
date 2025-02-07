using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] bool EnterToGoal = false;
    [SerializeField] TimerManager timermanager;
    [SerializeField] float offset;

    public Shape Shape;

    private void OnEnable()
    {
        EnterToGoal = true;
    }
    private void Update()
    {
        if (EnterToGoal) 
        {
            if (Shape != null)
            {
                float ShapeXPosition = Shape.gameObject.transform.position.x;
                float GoalXPosition = transform.position.x - offset;

                if (ShapeXPosition > GoalXPosition)
                {
                    EnterToGoal = false;
                    UIManager.Instance.isGameEnd = true;

                    if (timermanager != null) 
                    {
                        timermanager.start = false;
                    }
                }
            }
        }
    }
}
