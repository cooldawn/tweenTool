using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    void Start()
    {
        Transform myTransform = transform;
        Debug.Log(myTransform);

        // 移动Tween
        Tween moveTween = TweenTool.DoMove(myTransform, new Vector3(10, 0, 0), 2f, EaseType.EaseInOutSine, () => {
            Debug.Log("Move completed!");
        });

        // 缩放Tween
        Tween scaleTween = TweenTool.DoScale(myTransform, new Vector3(2, 2, 2), 2f, EaseType.EaseOutBack, () => {
            Debug.Log("Scale completed!");
        });

        // 在某个时刻终止Tween
        // moveTween.Kill();
        // scaleTween.Kill();
    }
}