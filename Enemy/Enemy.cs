using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //状态
    protected BaseState currentState;//当前状态
    protected BaseState defaultState;//默认状态
    protected BaseState patrolState;//巡逻状态
    protected BaseState traceState;//追踪状态

    //组件获取
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Attack currentAttack;

    [Header("基本参数")]
    public float normalSpeed;//基本速度
    public float runSpeed;//奔跑速度
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public Vector3 faceDir;//朝向

    //巡逻计时
    public float checkTime = 1f;//检视计时
    protected float checkTimer = 0;
    public float patrolTime = 2f;//巡逻计时
    protected float patolTimer = 0;
    protected bool isCheck = true;
    protected bool isPatol = true;//是否巡逻状态

    //寻找玩家、攻击
    public Vector2 detectOffset;//检测偏移量
    public Vector2 detectSize;//检测盒大小
    public float detectRadius;//检测半径
    public float detectDistance;//检测距离
    public LayerMask detectLayer;//检测图层
    public bool _isAttack = false;//是否已经攻击
    
    [HideInInspector] public GameObject player;//玩家
    protected float losePlayerTime = 3f;//丢失玩家
    [HideInInspector] public float losePlayerCount = 0;//计时
    protected float attackTiming = 1.5f;//攻击间隔
    [HideInInspector] public float attackTimingCount = 0;//计时


    //受伤
    [HideInInspector] public bool isHurt = false;
    protected float hurtForce = 4f;
    
    [Header("后摇")]
    public float attackBackShake;//攻击后摇
    public float hitBackShake;//受击后摇
    public float backShakeTimer = 0;//后摇计时

    //死亡
    [HideInInspector] public bool isDie = false;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentAttack = GetComponent<Attack>();
        currentSpeed = normalSpeed;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    protected virtual void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);//获取朝向
        TimeCount();//计时
        currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    //移动
    protected virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    //巡逻
    public virtual void Patrol(){}

    //寻敌检测
    public virtual bool DetectPlayer(){ return false; }

    //奔跑状态移动到玩家位置
    public virtual void RunToPlayerPosition(){}

    //攻击玩家
    public bool AttackPlayer()
    {
        _isAttack = currentAttack.isAttack;
        return _isAttack;
    }
    //计时
    protected virtual void TimeCount(){}

    //状态切换
    public void SwitchStatus(NpcState npcState)
    {
        var newState = npcState switch
        {
            NpcState.Patrol => patrolState,
            NpcState.Trace => traceState,
            NpcState.Default => defaultState,
            _ => null //弃元模式
        };

        currentState.OnExit();//退出上一状态
        currentState = newState;//新状态
        currentState.OnEnter(this);//执行新状态
    }
    protected virtual void OnDrawGizmosSelected(){}

    #region 事件执行
    //受伤
    public virtual void OnTakeDamage(Transform attacker){}

    public virtual IEnumerator OnHurt(Vector2 dir)
    {
        yield return null;
    }

    //死亡
    public virtual void OnDie(){}
    //消失
    public virtual void DestroyEnemy(){}
    #endregion
}
