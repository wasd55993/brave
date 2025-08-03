using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee : Enemy
{
    //存储玩家位置
    private Vector2? _fixedPlayerPosition = null; // 使用可空类型，判断是否已记录玩家位置
    protected override void Awake()
    {
        base.Awake();
        defaultState = new Enemy_Bee_DefaultState();
        patrolState = new Enemy_Bee_PatrolState();
        traceState = new Enemy_Bee_TraceState();
    }
    protected override void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, currentSpeed * faceDir.x * Time.deltaTime);
    }

    public override void Patrol()
    {
        if (isPatol)
        {
            Check();
        }
    }
    //是否检视
    protected void Check()
    {
        if (isCheck)
        {
            Patrol_Check();
        }
        else
        {
            Patrol_Walk();
        }
    }
    //检视检视控制
    protected void Patrol_Check()
    {
        patolTimer = 0;//重置行走计时
        rb.velocity = Vector2.zero;//禁止行走

        //检视计时
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkTime)
        {
            ChangeDir();//转换方向
            isCheck = false;//转换不检视状态
        }
    }
    //移动控制
    protected void Patrol_Walk()
    {
        checkTimer = 0;//重置检测计时
        Move();//开始行走

        //行走计时
        patolTimer += Time.deltaTime;
        if (patolTimer >= patrolTime)
        {
            isCheck = true;
        }
    }
    //转换方向
    protected void ChangeDir()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    //蜜蜂寻敌
    public override bool DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position + (Vector3)detectOffset,
            detectRadius,
            faceDir,
            detectDistance,
            detectLayer
            );

        if (hit.collider != null)
        {
            player = hit.collider.gameObject;

            if (_fixedPlayerPosition == null)
            {
                _fixedPlayerPosition = player.transform.position;
            }
            //计算方向
            Vector2 direction = (_fixedPlayerPosition.Value - (Vector2)transform.position).normalized;
            //转向玩家位置
            Vector2 faceDir = transform.right; // 当前朝向
            float dotProduct = Vector2.Dot(faceDir, direction);
            float turnThreshold = -0.1f; // 只有当方向明显相反时才转向

            if (dotProduct < turnThreshold)
            {
                ChangeDir(); //转向
            }
        }

        return hit;
    }
    //移动到玩家位置攻击
    public override void RunToPlayerPosition()
    {
        // 若未记录位置，直接返回
        if (_fixedPlayerPosition == null) return;

        // 计算方向
        Vector2 direction = (_fixedPlayerPosition.Value - (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(transform.position, _fixedPlayerPosition.Value); // 计算距离
        // 向玩家位置移动
        rb.velocity = direction * currentSpeed;

        if (distance <= 0.3f)
        {
            rb.velocity = Vector2.zero;//停止运动攻击
            animator.SetBool("isAttack", true);//播放攻击
            _isAttack = true;//已经攻击，开始计算后摇
            _fixedPlayerPosition = null;//重置玩家位置
            StartCoroutine(attackReset());
        }
    }

    private IEnumerator attackReset()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttack", false);
    }

    //计时
    protected override void TimeCount()
    {
        //后摇计时
        if (backShakeTimer > 0)
        {
            backShakeTimer -= Time.deltaTime;
        }
        //攻击后摇
        if (_isAttack)
        {
            backShakeTimer = attackBackShake;
        }
        //受击后摇
        if (isHurt)
        {
            backShakeTimer = hitBackShake;
        }

        //丢失目标
        if (!DetectPlayer())
        {
            losePlayerCount -= Time.deltaTime;
        }
        else if (DetectPlayer())
        {
            losePlayerCount = losePlayerTime;
        }
    }

    #region 事件执行
    //受伤
    public override void OnTakeDamage(Transform attacker)
    {
        if (attacker.gameObject.tag != "Weapon") return;
        //转受伤状态
        isHurt = true;
        animator.SetTrigger("hurt");

        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        //携程,击退效果,结束后重置受伤状态
        StartCoroutine(OnHurt(dir));
    }

    public override IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        isHurt = false;
    }

    //死亡
    public override void OnDie()
    {
        isDie = true;
        rb.velocity = Vector2.zero;
        animator.SetBool("isDie", true);
    }
    //消失
    public override void DestroyEnemy()
    {
        gameObject.layer = 2;
        Destroy(this.gameObject);
    }
    #endregion
}
