using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            MoveUpAndDownUI();
        }
    }

    void MoveUpAndDownUI()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        float Height = Mathf.Lerp(minHeight, maxHeight, t);
        Vector3 Position = transform.localPosition;
        Position.y = Height;
        transform.localPosition = Position;
    }
}
