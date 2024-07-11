using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EaseType
{
    Linear,
    EaseInSine,
    EaseOutSine,
    EaseInOutSine,
    EaseInBack,
    EaseOutBack,
    EaseInOutBack
}

public enum TweenType
{
    Move,
    Scale
}

public class Tween
{
    public Transform target;
    public Vector3 startValue;
    public Vector3 endValue;
    public float duration;
    public float elapsedTime;
    public EaseType easeType;
    public TweenType tweenType;
    public Action onComplete;
    public bool isKilled;

    public Tween(Transform target,Vector3 startValue, Vector3 endValue, float duration, EaseType easeType,TweenType tweenType, Action onComplete)
    {
        this.target = target;
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        this.elapsedTime = 0f;
        this.easeType = easeType;
        this.tweenType = tweenType;
        this.onComplete = onComplete;
        this.isKilled = false;
    }

    public void Kill()
    {
        isKilled = true;
    }
}

public class TweenTool : MonoBehaviour
{
    private static TweenTool Instance;
    private List<Tween> tweens = new List<Tween>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        for(int i = tweens.Count - 1; i >= 0; i--)
        {
            if (tweens[i].isKilled)
            {
                tweens.RemoveAt(i);
                continue;
            }

            Tween tween = tweens[i];
            tween.elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(tween.elapsedTime / tween.duration);
            float easeT = Ease(t, tween.easeType);

            switch (tween.tweenType)
            {
                case TweenType.Move:
                    tween.target.position = Vector3.Lerp(tween.startValue, tween.endValue, easeT);
                    break;
                case TweenType.Scale:
                    tween.target.localScale = Vector3.Lerp(tween.startValue, tween.endValue, easeT);
                    break;

            }

            

            if(tween.elapsedTime >= tween.duration)
            {
                tween.onComplete?.Invoke();
                tweens.RemoveAt(i);
            }
        }
    }

    public static Tween DoMove(Transform target, Vector3 endValue, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Tween tween = new Tween(target, target.position, endValue, duration, easeType,TweenType.Move, onComplete);
        Instance.tweens.Add(tween);
        return tween;
    }

    public static Tween DoScale(Transform target, Vector3 endValue, float duration, EaseType easeType = EaseType.Linear, Action onComplete = null)
    {
        Tween tween = new Tween(target, target.localScale, endValue, duration, easeType,TweenType.Scale, onComplete);
        Instance.tweens.Add(tween);
        return tween;
    }

    private float Ease(float t, EaseType easeType)
    {
        switch (easeType)
        {
            case EaseType.EaseInSine:
                return 1f - Mathf.Cos((t * Mathf.PI) / 2f);
            case EaseType.EaseOutSine:
                return Mathf.Sin((t * Mathf.PI) / 2f);
            case EaseType.EaseInOutSine:
                return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
            case EaseType.EaseInBack:
                float c1 = 1.70158f;
                float c3 = c1 + 1f;
                return c3 * t * t * t - c1 * t * t;
            case EaseType.EaseOutBack:
                c1 = 1.70158f;
                c3 = c1 + 1f;
                return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
            case EaseType.EaseInOutBack:
                c1 = 1.70158f;
                c3 = c1 * 1.525f;
                return t < 0.5f
                    ? (Mathf.Pow(2f * t, 2f) * ((c3 + 1f) * 2f * t - c3)) / 2f
                    : (Mathf.Pow(2f * t - 2f, 2f) * ((c3 + 1f) * (t * 2f - 2f) + c3) + 2f) / 2f;
            case EaseType.Linear:
                return t;
            default:
                return t;
        }
    }
}
