using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween : MonoBehaviour
{
    void Start()
    {
        // DOTween Init (optional)
        // If don't init, DOTween will be auto initialized with the default settings.
        //DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50);
        // DOTween.Init(autoKillMode, useSafeMode, logBehaviour);

        transform.DOLocalMove(new Vector3(0, 100, 0), 2f)
            .SetLoops(-1, LoopType.Yoyo)    // Infinite Loop Move
            .SetEase(Ease.InOutSine);       // Soft Move

    }

    void Update()
    {

    }
}
