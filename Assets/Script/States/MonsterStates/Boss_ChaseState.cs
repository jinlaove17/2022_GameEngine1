using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ChaseState : State<Boss>
{
    private static Boss_ChaseState instance;

    public static Boss_ChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Boss_ChaseState();
            }

            return instance;
        }
    }

    public override void Enter(Boss Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.recentTransition = 0.0f;
            Entity.navMeshAgent.isStopped = false;

            Entity.animator.SetTrigger("chase");
            Entity.IsAttack = false;
        }
    }

    public override void ProcessInput(Boss Entity)
    {

    }

    public override void LogicUpdate(Boss Entity)
    {
        Entity.recentTransition += Time.deltaTime;

        
        Entity.navMeshAgent.SetDestination(GameManager.Instance.player.transform.position);
        

        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {            
            if (0.0f < Entity.navMeshAgent.remainingDistance && Entity.navMeshAgent.remainingDistance < 10.0f )
            {
               Entity.stateMachine.ChangeState(Boss_AttackState.Instance);
            }
        }
    }

    public override void PhysicsUpdate(Boss Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.rigidBody.velocity = Vector3.zero;
            Entity.rigidBody.angularVelocity = Vector3.zero;
        }
    }

    public override void Exit(Boss Entity)
    {

    }
}
