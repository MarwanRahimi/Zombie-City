using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{

    private NavMeshAgent zombieNavMesh;

    [SerializeField]
    private Transform playerReference;

    CharacterStats playerStats;

    void Start()
    {
        zombieNavMesh = GetComponent<NavMeshAgent>();

        playerReference = GameObject.Find("Player").GetComponent<Transform>();  
    }

    void Update()
    {
        zombieNavMesh.SetDestination(playerReference.position);   
        


    }
}
