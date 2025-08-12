using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    //奔跑切换
    public bool isRun = false;
    public bool isRolling = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        //奔跑和行走
        animator.SetBool("isRun", isRun);
        if (isRun)
        {
            float runSpeed = Mathf.Abs(rb.velocity.x) * 2f;
            animator.SetFloat("velocityX", runSpeed);
        }
        else
        {
            animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        }
        //翻滚
        animator.SetBool("isRolling", isRolling);
        //跳跃
        animator.SetFloat("velocityY", rb.velocity.y);
        //落地
        animator.SetBool("isGround", physicsCheck.isGround);
        //死亡
        animator.SetBool("isDead", playerController.isDead);
        //攻击
        animator.SetBool("isAttack", playerController.isAttack);
    }
    //受伤
    public void PlayerHurt()
    {
        animator.SetTrigger("hurt");
    }
    //攻击
    public void PlayerAttack()
    {
        animator.SetTrigger("attack");
    }
}
