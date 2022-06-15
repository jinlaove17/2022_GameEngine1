using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AttackState : State<Boss>
{
    private static Boss_AttackState instance;

    public static Boss_AttackState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Boss_AttackState();
            }

            return instance;
        }
    }

    public override void Enter(Boss Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            Entity.transform.LookAt(GameManager.Instance.player.transform.position);
            Entity.animator.SetBool("IS_CHASE", false);
            Entity.isAttack = true;
            Entity.navMeshAgent.isStopped = true;
                        
        }
    }

    public override void ProcessInput(Boss Entity)
    {

    }

    public override void LogicUpdate(Boss Entity)
    {
        if (Entity.IsAlive && Entity.navMeshAgent.enabled)
        {
            int ranAction = UnityEngine.Random.Range(0, 3);
            switch (ranAction)
            {
                case 0:
                    Entity.ThrowPoison();
                    break;
                case 1:
                    Entity.SpellMeteor();
                    break;
                case 2:
                    Entity.Explosion();
                    break;
            }

            Entity.stateMachine.ChangeState(Boss_ChaseState.Instance);
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


