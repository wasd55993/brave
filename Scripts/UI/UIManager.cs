using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("响应脚本")]
    public PlayerStateBar playerStateBar;
    [Header("事件监听")]
    public AttributeEventSO hpEvent;

    //启用
    void OnEnable()
    {
        hpEvent.OnEventRaised += OnHpEvent; 
    }
    //禁用
    void OnDisable()
    {
        hpEvent.OnEventRaised -= OnHpEvent;
    }

    //血量响应
    private void OnHpEvent(Attribute attribute)
    {
        var percentage = attribute.currentHP / attribute.maxHP;
        playerStateBar.OnHpChange(percentage);
    }

}
