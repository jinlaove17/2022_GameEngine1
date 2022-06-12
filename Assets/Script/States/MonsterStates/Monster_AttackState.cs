using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_AttackState : State<Monster>
{
    private static Monster_AttackState instance;

    public static Monster_AttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Monster_AttackState();
            }

            return instance;
        }
    }

    public override void Enter(Monster Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.animator.SetTrigger("Attack");
        }
    }

    public override void ProcessInput(Monster Entity)
    {

    }

    public override void LogicUpdate(Monster Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.navMeshAgent.SetDestination(GameManager.Instance.player.transform.position);

            if (Entity.navMeshAgent.remainingDistance >= 1.0f)
            {
                Entity.stateMachine.ChangeState(Monster_ChaseState.Instance);
            }
        }
    }

    public override void PhysicsUpdate(Monster Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.rigidBody.velocity = Vector3.zero;
            Entity.rigidBody.angularVelocity = Vector3.zero;
        }
    }

    public override void Exit(Monster Entity)
    {

    }
}
