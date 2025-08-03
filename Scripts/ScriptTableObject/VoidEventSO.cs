using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    //事件广播
    public UnityAction OnEventRaised;

    //响应
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
