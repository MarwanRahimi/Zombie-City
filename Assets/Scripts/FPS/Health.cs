using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Unity.FPS.Game
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

        [Tooltip("Damage dealt by this character")] public float Damage = 25f;

        [Tooltip("Cooldown period between consecutive attacks")] public float DamageCooldown = 2f;

        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        public float CurrentHealth { get; set; }
        public bool Invincible;
        public bool CanPickup() => CurrentHealth < MaxHealth;

        public float GetRatio() => CurrentHealth / MaxHealth;
        public bool IsCritical() => GetRatio() <= CriticalHealthRatio;

        bool m_IsDead;
        bool canDealDamage = true; // to track cooldown state

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void Heal(float healAmount)
        {
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnHeal action
            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (Invincible)
                return;

            float healthBefore = CurrentHealth;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnDamage action
            float trueDamageAmount = healthBefore - CurrentHealth;
            if (trueDamageAmount > 0f)
            {
                OnDamaged?.Invoke(trueDamageAmount, damageSource);
            }

            Debug.Log("Zombie took damage");

            HandleDeath();
        }

        public void Kill()
        {
            CurrentHealth = 0f;

            // call OnDamage action
            OnDamaged?.Invoke(MaxHealth, null);

            HandleDeath();
        }

        void HandleDeath()
        {
            if (m_IsDead)
                return;

            // call OnDie action
            if (CurrentHealth <= 0f)
            {
                m_IsDead = true;
                OnDie?.Invoke();

                Destroy(gameObject);
            }
        }

        public void DealDamage(GameObject target)
        {
            if (canDealDamage)
            {
                var characterStats = target.GetComponent<CharacterStats>();
                if (characterStats != null)
                {
                    characterStats.TakeDamage(Damage);
                    Debug.Log("Dealt " + Damage + " damage to " + target.name);
                    StartCoroutine(DamageCooldownRoutine());
                }
                else
                {
                    Debug.Log("Target does not have a CharacterStats component");
                }
            }
        }

        IEnumerator DamageCooldownRoutine()
        {
            canDealDamage = false;
            yield return new WaitForSeconds(DamageCooldown);
            canDealDamage = true;
        }

        // Collision detection
        void OnCollisionStay(Collision collision)
        {
            // Check if the other object has the CharacterStats component
            var characterStats = collision.gameObject.GetComponent<CharacterStats>();
            if (characterStats != null)
            {
                DealDamage(collision.gameObject);
            }
        }
    }
}
