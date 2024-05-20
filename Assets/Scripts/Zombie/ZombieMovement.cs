using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public float patrolRadius = 20f;
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float patrolWaitTime = 3f;
    public float rotationSpeed = 5f;

    private float patrolTimer;
    private enum State { Patrolling, Chasing, Attacking }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = State.Patrolling;
        patrolTimer = patrolWaitTime;
        SetRandomDestination();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attacking:
                Attack();
                break;
        }

        CheckForPlayer();
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                SetRandomDestination();
                patrolTimer = 0f;
            }
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
        FacePlayer();
    }

    private void Attack()
    {
        Debug.Log("Attacking player");
        FacePlayer();
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attacking;
        }
        else if (distanceToPlayer <= sightRange)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing || currentState == State.Attacking)
        {
            currentState = State.Patrolling;
            SetRandomDestination();
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

}
