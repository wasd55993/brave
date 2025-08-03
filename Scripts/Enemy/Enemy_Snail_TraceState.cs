using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Snail_TraceState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.backShakeTimer < 0)
        {
            currentEnemy.SwitchStatus(NpcState.Patrol);
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
