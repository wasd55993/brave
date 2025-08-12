using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequstEvent;

    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="nextScene">目标场景</param>
    /// <param name="targetScene">目标位置</param>
    /// <param name="fadeScene">是否淡入淡出</param>
    public void RaiseLoadRequstEvent(GameSceneSO nextScene,Vector3 targetScene,bool fadeScene)
    {
        LoadRequstEvent?.Invoke(nextScene, targetScene, fadeScene);
    }
}
