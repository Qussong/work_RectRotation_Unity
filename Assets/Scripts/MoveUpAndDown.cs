using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    [SerializeField] Vector3 destination;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        transform.DOLocalMove(destination, moveSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    }
}
