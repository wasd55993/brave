using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bee_PatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.DetectPlayer())
        {
            currentEnemy.SwitchStatus(NpcState.Trace);
        }
        if (currentEnemy.isHurt)
        {
            currentEnemy.SwitchStatus(NpcState.Default);
        }
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.isHurt && !currentEnemy.isDie)
            currentEnemy.Patrol();
    }

    public override void OnExit()
    {

    }
}
