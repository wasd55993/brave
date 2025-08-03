using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boar_TraceState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        //Debug.Log("状态切换");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.runSpeed;
        currentEnemy.animator.SetBool("isRun", true);
    }

    public override void LogicUpdate()
    {
        //切换状态
        if (currentEnemy.losePlayerCount <= 0)
        {
            currentEnemy.SwitchStatus(NpcState.Patrol); 
        }
        if (currentEnemy.isHurt || currentEnemy.AttackPlayer())
        {
            currentEnemy.SwitchStatus(NpcState.Default);
        }
    }

    public override void PhysicsUpdate()
    {
        currentEnemy.RunToPlayerPosition();
    }

    public override void OnExit()
    {
        currentEnemy.animator.SetBool("isRun", false);
    }
}
