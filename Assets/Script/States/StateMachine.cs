using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T owner;

    private State<T> currentState;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void InitState(State<T> initState)
    {
        currentState = initState;
        currentState.Enter(owner);
    }

    public void ChangeState(State<T> newState)
    {
        currentState.Exit(owner);
        currentState = newState;
        currentState.Enter(owner);
    }

    public void LogicUpdate()
    {
        if (currentState != null)
        {
            currentState.LogicUpdate(owner);
        }
    }

    public void PhysicsUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsUpdate(owner);
        }
    }
}
