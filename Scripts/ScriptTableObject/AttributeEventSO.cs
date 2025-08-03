using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/AttributeEventSO")]
public class AttributeEventSO : ScriptableObject
{
    //事件广播
    public UnityAction<Attribute> OnEventRaised;

    //通知
    public void RaiseEvent(Attribute attribute)
    {
        OnEventRaised?.Invoke(attribute);
    }
}
