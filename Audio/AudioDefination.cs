using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefination : MonoBehaviour
{
    //广播事件
    public AudioEventSO audioEvent;
    //音频
    public AudioClip audioClip;
    //是否播放
    public bool playOnEnable;
    private void OnEnable()
    {
        if (playOnEnable) PlayAudio();
    }

    //通知
    public void PlayAudio()
    {
        audioEvent.RaiseEvent(audioClip);
    }
}
