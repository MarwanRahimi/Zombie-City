using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    // Configuration parameters
    [SerializeField] float attackDamage = 40f;
    [SerializeField] float attackRange = 5f;

    // State variables
    float distanceToTargetPlayer;

    // Cached references
    PlayerHealth targetPlayerHealth = null;
    Transform targetPlayerTransform = null;

    // Start is called before the first frame update
    void Start() {
        targetPlayerHealth = FindObjectOfType<PlayerHealth>();
        if (targetPlayerHealth != null) {
            targetPlayerTransform = targetPlayerHealth.transform;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void AttackHitEvent() {
        if (targetPlayerHealth == null) { return; }

        distanceToTargetPlayer = Vector3.Distance(targetPlayerTransform.position, transform.position);

        if (distanceToTargetPlayer < attackRange) {
            targetPlayerHealth.DamagePlayer(attackDamage);
        }

    }

}
