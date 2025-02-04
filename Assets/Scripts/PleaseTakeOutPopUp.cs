using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PleaseTakeOutPopUp : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float MaxScale;
    [SerializeField] float MinScale;

    private void Update()
    {
        if (gameObject.activeSelf) 
        {
            ResizeUI();
        }
    }

    void ResizeUI() 
    {
        float t = Mathf.PingPong(Time.time * Speed, 1f);
        float scale = Mathf.Lerp(MinScale, MaxScale, t);
        transform.localScale = new Vector3(scale, scale, scale);
    }

}
