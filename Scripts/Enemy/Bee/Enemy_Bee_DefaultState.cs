using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee_DefaultState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy._isAttack = false;
    }

    public override void LogicUpdate()
    {
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
        currentEnemy._isAttack = false;
    }
}
