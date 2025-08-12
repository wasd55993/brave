using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    //玩家位置
    public Vector3 playerMenuPosition;
    public Vector3 playerfirstPosition;
    public Transform playerTransform;
    
    [Header("事件监听")]
    public SceneLoadEventSO loadEvent;
    public GameSceneSO Scene;//获取场景
    public VoidEventSO newGame;//新游戏
    
    [Header("事件广播")]
    public VoidEventSO cameraEvent;//相机
    public FadeCanvasEventSO FadeCanvasEvent;//淡入淡出
    public SceneLoadEventSO sceneUnLoadEvent;//控制血条在黑屏时不显示

    [Header("场景")]
    public GameSceneSO MenuScene;//菜单场景
    //临时存储下一个场景的信息
    private GameSceneSO currentScene;

    private GameSceneSO targetScene;
    private Vector3 targetPosition;
    private bool isFade;

    //淡入淡出计时
    private float fadeDuration = 1f;

    //是否加载
    private bool isLoading = false;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(currentScene.sceneReference, LoadSceneMode.Additive);
        //currentScene = Scene;
        //currentScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        //NewGame();
        loadEvent.RaiseLoadRequstEvent(MenuScene, playerMenuPosition,true);
    }

    private void OnEnable()
    {
        loadEvent.LoadRequstEvent += OnLoadRequstEvent;
        newGame.OnEventRaised += NewGame; 
    }
    private void OnDisable()
    {
        loadEvent.LoadRequstEvent -= OnLoadRequstEvent;
        newGame.OnEventRaised -= NewGame;
    }

    //加载新场景
    private void NewGame()
    {
        targetScene = Scene;
        //OnLoadRequstEvent(targetScene, firstPosition, true);
        loadEvent.RaiseLoadRequstEvent(targetScene, playerfirstPosition, true);
    }

    //事件响应，获取场景参数
    private void OnLoadRequstEvent(GameSceneSO targetScene, Vector3 targetPosition, bool isFade)
    {
        if (isLoading) return;

        this.targetScene = targetScene;
        this.targetPosition = targetPosition;
        this.isFade = isFade;

        isLoading = true;

        if (currentScene != null)
        {
            StartCoroutine(DeleteCurrentScene());
        }
        else
        {
            LoadTargetScene();
        }
    }
    //协程处理切换场景
    private IEnumerator DeleteCurrentScene()
    {
        
        if(isFade)
        {
            //TODO:淡入淡出
            Debug.Log("执行这里1");
            FadeCanvasEvent.FadeBlack(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        Debug.Log("执行这里2");
        sceneUnLoadEvent.RaiseLoadRequstEvent(targetScene, targetPosition,true);

        yield return currentScene.sceneReference.UnLoadScene();
        
        playerTransform.gameObject.SetActive(false);
        
        LoadTargetScene();
    }
        
    //加载新场景
    private void LoadTargetScene()
    {
        var loadingOption =  targetScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompeleted;//回调函数
    }

    private void OnLoadCompeleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentScene = targetScene;//换场景

        playerTransform.position = targetPosition;//移动角色
        playerTransform.gameObject.SetActive(true);

        if (isFade)
        {
            FadeCanvasEvent.FadeTransparent(fadeDuration);
        }

        isLoading = false;


        if(currentScene.SceneType == SceneType.Location) 
            cameraEvent?.RaiseEvent();
    }
    //加载完成后
}
