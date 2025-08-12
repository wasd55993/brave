using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee_TraceState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.runSpeed;
        currentEnemy.rb.velocity = Vector2.zero;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy._isAttack)
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

    }
}
