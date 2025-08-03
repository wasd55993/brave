using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("事件监听")]
    public AudioEventSO bgmEvent;
    public AudioEventSO effectEvent;
    [Header("音频源")]
    public AudioSource bgmSource;//背景音乐
    public AudioSource effectSource;//音效

    private void OnEnable()
    {
        bgmEvent.OnEventRaised += PlayBgmEvent;
        effectEvent.OnEventRaised += PlayEffectAudioEvent;
    }
    private void OnDisable()
    {
        bgmEvent.OnEventRaised -= PlayBgmEvent;
        effectEvent.OnEventRaised -= PlayEffectAudioEvent;
    }
    //播放背景音乐
    private void PlayBgmEvent(AudioClip audioClip)
    {
        bgmSource.clip = audioClip;
        bgmSource.Play();
    }

    //播放音效
    private void PlayEffectAudioEvent(AudioClip audioClip)
    {
        effectSource.clip = audioClip;
        effectSource.Play();
    }
}
