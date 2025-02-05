using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    public enum UIType
    {
        MissionSucceed,
        MissionFail,
        SphereEnd
    }

    [SerializeField] GameObject MissionSucceed;
    [SerializeField] GameObject MissionFail;
    [SerializeField] GameObject SphereEnd;
    

    public void ShowUI(UIType NewUIType) 
    {
        switch (NewUIType)
        {
            case UIType.MissionSucceed:
                MissionSucceed.SetActive(true);
                MissionFail.SetActive(false);
                SphereEnd.SetActive(false);
                break;
            case UIType.MissionFail:
                MissionSucceed.SetActive(false);
                MissionFail.SetActive(true);
                SphereEnd.SetActive(false);
                break;
            case UIType.SphereEnd:
                MissionSucceed.SetActive(false);
                MissionFail.SetActive(false);
                SphereEnd.SetActive(true);
                break;
            default:
                break;
        }
    }
}
