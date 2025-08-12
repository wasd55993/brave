using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Enemy_Boar_PatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        //切换状态
        if (currentEnemy.DetectPlayer())
        {
            currentEnemy.SwitchStatus(NpcState.Trace);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDie)
            currentEnemy.Patrol();
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isWalk",false);
    }
}
