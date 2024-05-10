using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{

    private NavMeshAgent zombieNavMesh;

    [SerializeField]
    private Transform playerReferene;

    void Start()
    {
        zombieNavMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        zombieNavMesh.SetDestination(playerReferene.position);   
    }
}
