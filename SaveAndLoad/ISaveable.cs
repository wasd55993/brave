using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    //注册和卸载,加载物品和卸载物品
    void RegisterSaveData() => DataManager.Instance.RegisterSaveData(this);
    void UnRegisterSaveData() => DataManager.Instance.UnRegisterSaveData(this);




}
