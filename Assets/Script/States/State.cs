using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public abstract void Enter(T Entity);
    public abstract void ProcessInput(T Entity);
    public abstract void LogicUpdate(T Entity);
    public abstract void PhysicsUpdate(T Entity);
    public abstract void Exit(T Entity);
}
