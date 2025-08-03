using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Attribute : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHP;
    public float currentHP;

    [Header("无敌帧")]
    public bool invincibility = false;
    public float invincibilityDuration;
    public float invincibilityCounter;

    [Header("事件")]
    public UnityEvent<Attribute> OnHpChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    void Start()
    {
        currentHP = maxHP;
        OnHpChange?.Invoke(this);
    }

    private void Update()
    {
        //重置无敌帧时间
        if (invincibility)
        {
            invincibilityCounter -= Time.deltaTime;
            if (invincibilityCounter <= 0 ) 
            {
                invincibility = false;
            }
        }
    }

    //受伤
    public void TakeDamage(Attack attack)
    {
        if (invincibility) return;

        //判断该次伤害血量是否足够
        if ((currentHP - attack.damage) > 0)
        { 
            currentHP -= attack.damage;
            TriggerInvincibility();
            //受伤处理
            OnTakeDamage?.Invoke(attack.transform);
        }//超过直接清零死亡
        else
        {
            currentHP = 0;
            //触发死亡
            OnDie?.Invoke();
        }
        OnHpChange?.Invoke(this);
    }

    //溶于水
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            currentHP = 0;
            OnHpChange?.Invoke(this);
            OnDie?.Invoke();
        }
    }

    public void TriggerInvincibility()
    {
        //受到伤害无敌帧
        if(!invincibility)
        {
            invincibility = true;
            invincibilityCounter = invincibilityDuration;
        }
    }
}
