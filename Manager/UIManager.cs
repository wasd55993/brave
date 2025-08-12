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
    public SceneLoadEventSO loadEvent;

    //启用
    void OnEnable()
    {
        hpEvent.OnEventRaised += OnHpEvent;
        loadEvent.LoadRequstEvent += OnLoadRequstEvent;
    }

    //禁用
    void OnDisable()
    {
        hpEvent.OnEventRaised -= OnHpEvent;
        loadEvent.LoadRequstEvent -= OnLoadRequstEvent;
    }

    private void OnLoadRequstEvent(GameSceneSO scene, Vector3 vector, bool arg3)
    {
        var isMenu = scene.SceneType == SceneType.Menu;
        playerStateBar.gameObject.SetActive(!isMenu);
    }

    //血量响应
    private void OnHpEvent(Attribute attribute)
    {
        var percentage = attribute.currentHP / attribute.maxHP;
        playerStateBar.OnHpChange(percentage);
    }

}
