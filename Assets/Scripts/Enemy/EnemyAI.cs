using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    // Configuration parameters
    [SerializeField] Transform target;
    [SerializeField] float provokeRange = 8f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float stoppingDistanceBuffer = 0.5f;

    // State variables
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool isProvokedByDamage = false;
    string currenAnimationState = "idle";

    // Cached references
    NavMeshAgent myNavMeshAgent;
    Animator myAnimator;
    Health myEnemyHealth;
    EnemySFX myEnemySFX;
    BossAudio myBossAudio;
    CapsuleCollider myCapsuleCollider;

    // Start is called before the first frame update
    void Start() {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
        myEnemyHealth = GetComponent<Health>();
        if (GetComponent<EnemySFX>() != null) {
            myEnemySFX = GetComponent<EnemySFX>();
        }
        else if (GetComponent<BossAudio>() != null) {
            myBossAudio = GetComponent<BossAudio>();
        }
        myCapsuleCollider = GetComponent<CapsuleCollider>();
        myAnimator.SetTrigger("startIdle");
    }

    // Update is called once per frame
    void Update() {

        if (myEnemyHealth.IsDead()) {
            if (GetComponent<EnemySFX>() != null) {
                myEnemySFX.SetCurrentAnimationState("death");
            }
            else if (GetComponent<BossAudio>() != null) {
                myBossAudio.SetCurrentAnimationStateBoss("death");
            }
            currenAnimationState = "death";
            myNavMeshAgent.enabled = false;
            myCapsuleCollider.enabled = false;
            enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);
        HandleProvoked();

        if (isProvoked || isProvokedByDamage) {
            EngageTarget();
        }
    }

    public void OnDamageTaken() {
        isProvokedByDamage = true;
    }

    void HandleProvoked() {
        if (distanceToTarget <= provokeRange && !isProvoked) {
            isProvoked = true;
        }
    }

    void EngageTarget() {
        FaceTarget();

        if (distanceToTarget > myNavMeshAgent.stoppingDistance + stoppingDistanceBuffer) {
            ChaseTarget();
        }

        else {
            AttackTarget();
        }
    }

    void ChaseTarget() {
        currenAnimationState = "chasing";
        myAnimator.SetBool("isAttacking", false);
        myAnimator.SetTrigger("startRunning");
        myNavMeshAgent.SetDestination(target.position);
    }

    void AttackTarget() {
        currenAnimationState = "attacking";
        myAnimator.SetBool("isAttacking", true);
    }

    void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }


    public string GetAnimationState() {
        return currenAnimationState;
    }

    public float GetDistanceToTarget() {
        return distanceToTarget;
    }

    public Transform GetTargetTransform() {
        return target;
    }



    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, provokeRange);
    }

}
