using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraControl : MonoBehaviour
{
    //组件获取
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;

    //监听
    public VoidEventSO cameraShakeEvent;
    public VoidEventSO cameraEvent;

    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        cameraEvent.OnEventRaised += OnCameraEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        cameraEvent.OnEventRaised -= OnCameraEvent;
    }

    //事件响应
    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    private void OnCameraEvent()
    {
        GetCameraBounds();
    }

    //获取相机约束空间
    private void GetCameraBounds()
    {
        var bound = GameObject.FindGameObjectWithTag("Bounds");
        if (bound == null) return;

        confiner2D.m_BoundingShape2D = bound.GetComponent<Collider2D>();

        //清除缓存
        confiner2D.InvalidateCache();
    }
}
