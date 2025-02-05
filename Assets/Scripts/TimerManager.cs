using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{ 
    [SerializeField] Image timerimage;
    public bool start = false;


    void Update()
    {
        if (start) 
        {
            timerimage.fillAmount -= Time.deltaTime * 0.05f;

            if (timerimage.fillAmount <= 0) 
            {
                timerimage.fillAmount = 0;
                UIManager.Instance.LoadGameEndUI();
                UIManager.Instance.TestStartButton = false;
                start = false;
                timerimage.fillAmount = 1;
            }
        }
    }

    public void StartTimer() 
    {
        start = true;
    }
}
