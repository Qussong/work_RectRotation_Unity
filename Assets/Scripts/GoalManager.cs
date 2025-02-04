using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] bool InEnterToGoal = false;

    public Shape Shape;

    private void OnEnable()
    {
        InEnterToGoal = true;
    }
    private void Update()
    {
        if (InEnterToGoal) 
        {
            float ShapeXPosition = Shape.gameObject.transform.position.x; 
            float GoalXPosition = transform.position.x;

            if (ShapeXPosition > GoalXPosition) 
            {
                InEnterToGoal = false;
                UIManager.Instance.LoadGameEndUI();
            }
        }
    }
}
