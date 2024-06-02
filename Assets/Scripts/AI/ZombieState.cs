using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ZombieState
{
    protected ZombieState currentState;
    protected ZombieStateMachine stateMachine;
    public ZombieState(ZombieStateMachine stateMachineRef)
    {
        this.stateMachine = stateMachineRef;
    }

    protected virtual void OnEnter()
    {
        Debug.Log("Entered State");    
    }

    protected virtual void OnUpdate() 
    {
        Debug.Log("Update State");
    }

    protected virtual void OnExit()
    { 
        Debug.Log("Exit State");
    }

    protected void ChangeState(ZombieState nextState)
    {
        if(currentState != null)
            currentState.OnExit();

        currentState = nextState;
        currentState.OnEnter();
    }

    public ZombieState Tick()
    {
        OnUpdate();
        return currentState;
    }

    protected bool CanAttackPlayer()
    {
        return Vector3.Distance(stateMachine.gameObject.transform.position, Player.Instance.gameObject.transform.position) <= stateMachine.ZombieNavMesh.stoppingDistance;

    }
}

public class ChaseState : ZombieState
{
    public ChaseState(ZombieStateMachine stateMachineRef) : base(stateMachineRef)
    {


    }

    protected override void OnEnter()
    {
        Debug.Log("Entered Chase State");
        stateMachine.ZombieNavMesh.speed = stateMachine.ZombieDifficulty.ZombieSpeed;
    }

    protected override void OnUpdate()
    {
        Debug.Log("Entered update State");

        stateMachine.ZombieNavMesh.destination = Player.Instance.transform.position;
        
        if (CanAttackPlayer())
        {
            Debug.Log("Can attack player. Changing state");
            ChangeState(new AttackState(stateMachine));
        }
    }

    protected override void OnExit()
    {
        Debug.Log("exitted chase State");

    }

}

public class AttackState : ZombieState
{

    public AttackState(ZombieStateMachine stateMachineRef) : base(stateMachineRef)
    {


    }

    protected override void OnEnter()
    {
        Debug.Log("Attacking state");
    }

    protected override void OnUpdate()
    {
        Debug.Log("Attacking state running");

        if (!CanAttackPlayer())
        {
            ChangeState(new ChaseState(stateMachine));
        }
    }

    protected override void OnExit()
    {
        Debug.Log("Attacking state exit");
    }

}
