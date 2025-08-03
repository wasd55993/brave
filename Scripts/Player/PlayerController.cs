using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    [Header("物理材质")]
    private CapsuleCollider2D capsuleCollider;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    //获取键盘控制
    public PlayerInputControl inputcontrol;
    public Vector2 inputDiretion;

    //人物移动控制
    [Header("基本参数")]
    private Rigidbody2D rb;
    public float walkSpeed = 250f;
    public float runSpeed = 300f;
    public float currentSpeed;
    public float jumpForce = 14.5f;
    //奔跑、翻滚检测
    private PlayerAnimator playerAnimator;
    private Attribute playerAttribute;
    private bool _isRun = false;
    private bool _isRolling = false;
    //翻滚无敌帧
    private float invincibilityDuration = 0.75f;
    private float invincibilityCounter;
    //跳跃检测
    private PhysicsCheck physicscheck;
    

    [Header("受伤处理")]
    public bool isHurt = false;
    public float hurtForce = 8.0f;
    //死亡处理
    public bool isDead = false;
    //攻击状态
    public bool isAttack = false;


    private void Start()
    {
        currentSpeed = walkSpeed;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputcontrol = new PlayerInputControl();
        playerAnimator = GetComponent<PlayerAnimator>();
        physicscheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerAttribute = GetComponent<Attribute>();
        inputcontrol.Gameplay.Jump.started += jump;
        inputcontrol.Gameplay.Run.started += run;
        inputcontrol.Gameplay.Attack.started += attack;
        inputcontrol.Gameplay.Rolling.started += rolling;
    } 

    private void OnEnable()
    {
        inputcontrol.Enable();
    }
    private void OnDisable()
    {
        inputcontrol.Disable();
    }

    private void Update()
    {
        //获取键值
        inputDiretion = inputcontrol.Gameplay.Move.ReadValue<Vector2>();

        //奔跑速度设置
        if (_isRun)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
        //翻滚无敌帧数
        if (_isRolling)
        {
            invincibilityCounter -= Time.deltaTime;
            if (invincibilityCounter <= 0)
            {
                _isRolling = false;
                gameObject.layer = 7;
                playerAnimator.isRolling = _isRolling;
            }
        }

        CheckState();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        if (!isHurt && !isAttack )
        {
            Move();
        }
    }

    //移动
    private void Move()
    {
        rb.velocity = new Vector2(inputDiretion.x * currentSpeed * Time.deltaTime, rb.velocity.y);

        //反向移动
        int faceDir = (int)transform.localScale.x;

        if (inputDiretion.x < 0) { faceDir = -1; }
        else if (inputDiretion.x > 0) { faceDir = 1; }

        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    //跳跃控制
    private void jump(InputAction.CallbackContext context)
    {
        if (physicscheck.isGround || physicscheck.isLeftWall || physicscheck.isRightWall)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    //奔跑控制
    private void run(InputAction.CallbackContext context)
    {
        _isRun = !_isRun;
        playerAnimator.isRun = _isRun;
    }
    //翻滚控制
    private void rolling(InputAction.CallbackContext context)
    {
        _isRolling = true;
        playerAnimator.isRolling = _isRolling;
        //更换图层，不碰撞，实现无敌帧
        gameObject.layer = 2;
        invincibilityCounter = invincibilityDuration;
    }
    //攻击控制
    private void attack(InputAction.CallbackContext context)
    {
        if (!physicscheck.isGround) return;
        isAttack = true;
        playerAnimator.PlayerAttack();
    }

    //事件
        //受伤处理
    public void GetHurt(Transform attackTransfrom)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attackTransfrom.position.x,0).normalized;//归一化，转单位向量
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
        //死亡处理
    public void PlayerDie()
    {
        isDead = true;
        inputcontrol.Gameplay.Disable();
    }
    //材质选择
    private void CheckState()
    {
        capsuleCollider.sharedMaterial = physicscheck.isGround ? normal : wall;
    }
}