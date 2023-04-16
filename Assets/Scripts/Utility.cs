using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnimCurveValue
{
    [SerializeField] public AnimationCurve curve;
    [SerializeField] public float animation_duration;

    public float value = 0;
    [SerializeField] public float val_start, val_end;

    public void Reset()
    {
        value = 0;
    }

    public float Update(float time)
    {
        value += (1/(animation_duration/60)) * time; 
        value = Mathf.Clamp(value,0,1);

        return Mathf.Lerp(val_start,val_end,curve.Evaluate(value));
    }

    public float GetValue()
    {
        return Mathf.Lerp(val_start,val_end,curve.Evaluate(value));
    }

    public float GetRawValue()
    {
        return value;
    }
} 