using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Local Position : " + transform.localPosition);
    }
}
