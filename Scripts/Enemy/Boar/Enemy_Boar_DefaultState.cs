using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boar_DefaultState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        //重置
        currentEnemy.animator.SetBool("isWalk",false);
        currentEnemy.animator.SetBool("isRun", false);
        currentEnemy.currentAttack.isAttack = false;
    }

    public override void LogicUpdate()
    {
        currentEnemy.currentAttack.isAttack = false;
        if (currentEnemy.backShakeTimer <= 0)
        {
            currentEnemy.SwitchStatus(NpcState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        currentEnemy.currentAttack.isAttack = false;
    }
}
