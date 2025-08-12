using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("攻击")]
    public int damage;
    public float attackRange;//范围
    public float attackRate;//频率
    public bool isAttack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 尝试获取敌人组件（假设敌人脚本是 Enemy_Snail）
        Enemy_Snail enemy = collision.GetComponent<Enemy_Snail>();

        // 若碰撞对象是蜗牛敌人
        if (enemy != null)
        {
            // 若敌人处于隐藏状态，不造成伤害
            if (enemy.isHide)
            {
                isAttack = false; // 标记攻击无效
                return;
            }
            // 敌人未隐藏，正常造成伤害
            collision.GetComponent<Attribute>()?.TakeDamage(this);
            isAttack = true;
        }
        // 非蜗牛敌人的情况（如其他敌人）
        else
        {
            collision.GetComponent<Attribute>()?.TakeDamage(this);
            isAttack = true;
        }
    }
}