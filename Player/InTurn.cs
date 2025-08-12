using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InTurn : MonoBehaviour
{
    //输入系统
    public PlayerInputControl inputcontrol;

    public GameObject inTurn;
    public Transform playerTransform;//控制方向
    private Animator animator;//动画控制
    private IInteractable interactable;//获取可互动对象
    private bool canPress = false;

    void Awake()
    {
        animator = inTurn.GetComponent<Animator>();
        inputcontrol = new PlayerInputControl();
        inputcontrol.Gameplay.Interacted.started += OnConfirm;
        inputcontrol.Enable();//启用输入系统
    }    

    void Update()
    {
        inTurn.GetComponent<SpriteRenderer>().enabled = canPress;
        inTurn.transform.localScale = playerTransform.localScale;
    }

    //按下互动按钮
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            interactable.TriggerAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interacted"))
        {
            canPress = true;
            interactable = collision.GetComponent<IInteractable>();
            animator.Play("InTurn");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
        animator.Play("Default");
    }
}
