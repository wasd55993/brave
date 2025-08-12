using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeCanvasEventSO")]
public class FadeCanvasEventSO : ScriptableObject
{
    public UnityAction<Color,float,bool> OnEventRaised;

    /// <summary>
    /// 场景变黑
    /// </summary>
    /// <param name="durtaion"></param>
    public void FadeBlack(float duration)
    {
        RaiseEvent(Color.black,duration,true);
    }
    /// <summary>
    /// 场景显示
    /// </summary>
    /// <param name="duration"></param>
    public void FadeTransparent(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
    }

    public void RaiseEvent(Color color,float duration,bool isFade) 
    {
        OnEventRaised?.Invoke(color,duration,isFade);
    }
}
