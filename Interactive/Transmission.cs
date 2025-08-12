using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO sceneLoadEventSO;
    public GameSceneSO targetScene;
    public Vector3 startPoint;//目标位置

    public void TriggerAction()
    {
        sceneLoadEventSO.RaiseLoadRequstEvent(targetScene, startPoint,true);
    }
}
