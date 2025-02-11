using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{ 
    [SerializeField] Image timerimage;
    public bool start = false;

    private void OnEnable()
    {
        timerimage.fillAmount = 1;
    }

    void Update()
    {
        if (start && !UIManager.Instance.isGameEnd) 
        {
            timerimage.fillAmount -= Time.deltaTime * 0.05f;

            if (timerimage.fillAmount <= 0) 
            {
                timerimage.fillAmount = 0;                
                UIManager.Instance.IsTimeOut = true;
                UIManager.Instance.LoadGameEndUI();
                start = false;
            }
        }
    }

    public void StartTimer() 
    {
        start = true;
    }
}
