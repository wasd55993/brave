 using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class Enemy_Boar : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        defaultState = new Enemy_Boar_DefaultState();
        patrolState = new Enemy_Boar_PatrolState();
        traceState = new Enemy_Boar_TraceState();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
    }

    //野猪巡逻
    public override void Patrol()
    {
        if (isPatol)
        {
            Check();
        }
    }
    //是否检视
    private void Check()
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
    //检视控制
    private void Patrol_Check()
    {
        patolTimer = 0;//重置行走计时
        animator.SetBool("isWalk", false);//禁止行走动画
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
    private void Patrol_Walk()
    {
        checkTimer = 0;//重置检测计时
        animator.SetBool("isWalk", true);//启动行走动画
        Move();//开始行走

        //行走计时
        patolTimer += Time.deltaTime;
        if (patolTimer >= patrolTime)
        {
            isCheck = true;
        }
    }
    //转换方向
    private void ChangeDir()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    //野猪寻敌
    public override bool DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + (Vector3)detectOffset,
            detectSize,
            0,
            faceDir,
            detectDistance,
            detectLayer
        );

        if (hit.collider != null)
        {
            player = hit.collider.gameObject;
        }

        return hit;
    }

    //显示检测距离
    protected override void OnDrawGizmosSelected()
    { 
        Gizmos.DrawWireSphere(transform.position + (Vector3)detectOffset + new Vector3(detectDistance * faceDir.x, 0, 0), 0.2f); 
    }

    //移动到玩家位置攻击
    public override void RunToPlayerPosition()
    {
        if (player == null) return;

        if (faceDir.x < 0)
        {
            if (transform.position.x > player.transform.position.x)
            {
                Move();
            }
        }//左边移动
        if (faceDir.x > 0)
        {
            if (transform.position.x < player.transform.position.x)
            {
                Move();
            }
        }//右边移动
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
        if (AttackPlayer())
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
            animator.SetBool("isRun", false);
            animator.SetBool("isWalk", true);
            if (rb.velocity.x == 0)
            {
                animator.SetBool("isWalk", false);
            }

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
        animator.SetBool("isWalk", false);
        //受到攻击转向
        if ((attacker.position.x - transform.position.x) > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if ((attacker.position.x - transform.position.x) < 0)
            transform.localScale = new Vector3(1, 1, 1);
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
