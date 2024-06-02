using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieStateMachine : MonoBehaviour
{
    private ZombieState currentState;

    public ZombieDifficulty ZombieDifficulty;
    
    public NavMeshAgent ZombieNavMesh;
    
    public void Start()
    {
        currentState = new ChaseState(this);

        if(ZombieNavMesh == null)
            ZombieNavMesh = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState = currentState.Tick();

        }
    }
}
