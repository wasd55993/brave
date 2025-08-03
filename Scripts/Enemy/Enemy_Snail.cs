using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail : Enemy
{
    [Header("蜗牛参数")]
    //是否寻找到玩家
    public bool isFound = false;
    //发现玩家躲藏后摇
    public float foundPlayerTime = 4f;
    //是否隐藏状态
    public bool isHide = false;

    protected override void Awake()
    {
        base.Awake();
        defaultState = new Enemy_Snail_DefaultState();
        patrolState = new Enemy_Snail_PatrolState();
        traceState = new Enemy_Snail_TraceState();
    }
    protected override void Update()
    {
        base.Update();
        if (backShakeTimer < 0)
        {
            isHide = false;
        }
    }
    //蜗牛 巡逻
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

    //蜗牛寻敌
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
            isFound = true;
            player = hit.collider.gameObject;
        }
        else
        {
            isFound = false;
        }

        return hit;
    }

    //显示检测距离
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)detectOffset + new Vector3(detectDistance * faceDir.x, 0, 0), 0.2f);
    }

    //发现玩家
    public override void RunToPlayerPosition()
    {
        if (player == null) return;

        isHide = true;
        animator.SetBool("isFound", true);
    }

    //计时
    protected override void TimeCount()
    {
        //后摇计时
        if (backShakeTimer > 0)
        {
            backShakeTimer -= Time.deltaTime;
        }
        //发现玩家
        if (isFound)
        {
            backShakeTimer = foundPlayerTime;
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
        Debug.Log(isHide);
        if (attacker.gameObject.tag != "Weapon" || isHide) return;
        Debug.Log("执行");
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
