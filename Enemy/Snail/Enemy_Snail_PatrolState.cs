using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail_PatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.animator.SetBool("isFound",false);
    }

    public override void LogicUpdate()
    {
        if(currentEnemy.DetectPlayer())
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
        currentEnemy.animator.SetBool("isFound", false);
    }
}
