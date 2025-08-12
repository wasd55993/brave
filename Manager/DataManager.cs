using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //单例模式
    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSaveData(ISaveable saveable)
    { }
    public void UnRegisterSaveData(ISaveable saveable)
    { }
}
