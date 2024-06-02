using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Unity.FPS.Game
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

        [Tooltip("Damage dealt by this character")] public float Damage = 25f;

        [Tooltip("Cooldown period between consecutive attacks")] public float DamageCooldown = 2f;

        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;
        AudioSource audioSource;
        public AudioClip zombieSound1;
        public AudioClip zombieSound2;
        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        private Animator zombieAnimator;
        public float CurrentHealth { get; set; }
        public bool Invincible;

        [System.Serializable]
        public class Item
        {
            public GameObject prefab;
        }

        public Item[] dropItems; 
        private HashSet<GameObject> droppedItems = new HashSet<GameObject>();

        bool m_IsDead;
        bool canDealDamage = true; // to track cooldown state

        void Start()
        {
            CurrentHealth = MaxHealth;
            audioSource = GetComponent<AudioSource>();
            zombieAnimator = GetComponent<Animator>();
            if (audioSource != null )
            {
                PlayRandomZombieSound();
                /*InvokeRepeating("PlayRandomZombieSound", 0f, 5f);*/
            }
        }

        void PlayRandomZombieSound()
        {
            // Select a random zombie sound
            int randomIndex = Random.Range(0, 2);
            if (randomIndex == 0)
            {
                audioSource.clip = zombieSound1;
            }
            else
            {
                audioSource.clip = zombieSound2;
            }
            audioSource.Play();
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

            HandleDeath();
        }

        public void Kill()
        {
            CurrentHealth = 0f;
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
                DropItems();
                IncreasePlayerAmmo();
                EnemySpawner.Instance.OnEnemyKilled(); //increment enemies killed in spawner class
                zombieAnimator.Play("Death");
                Destroy(gameObject, 5.0f);
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

        public void DropItems() //Static item drops
        {
            int killedEnemies = EnemySpawner.Instance.maxSpawnNumber - EnemySpawner.Instance.remainingEnemies;

            // Check if the killed enemies count matches the next drop threshold
            if (killedEnemies == 3)
            {
                TryDropItem(0);
            }
            if (killedEnemies == 7)
            {
                TryDropItem(1);
            }
            if (killedEnemies == 11)
            {
                TryDropItem(2);
            }
            if (killedEnemies == 14)
            {
                TryDropItem(3);
            }
            if (killedEnemies == 20)
            {
                TryDropItem(4);
            }
        }

        private void TryDropItem(int index)
        {
            if (index < 0 || index >= dropItems.Count())
            {
                return;
            }

            Item item = dropItems[index];
            if (item == null || item.prefab == null)
            {
                return;
            }

            Debug.Log($"Dropping item: {item.prefab.name}");
            GameObject droppedItem = Instantiate(item.prefab, transform.position, item.prefab.transform.rotation);
            droppedItem.transform.position += new Vector3(0, 1, 0);
            droppedItems.Add(droppedItem);
        }
    

    public bool IsDead()
        {
            return m_IsDead;
        }

        void IncreasePlayerAmmo()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject switcher = GameObject.FindGameObjectWithTag("Switcher");
            
            if (player == null)
            {
                Debug.LogWarning("Player GameObject not found.");
                return;
            }

            Ammo ammoComponent = player.GetComponent<Ammo>();
            WeaponSwitcher weaponSwitcher = switcher.GetComponent<WeaponSwitcher>();

            if (ammoComponent == null || switcher == null || weaponSwitcher == null)
            {
                if (ammoComponent == null)
                {
                    Debug.LogWarning("Ammo component not found on the player GameObject.");
                }

                if (switcher == null)
                {
                    Debug.LogWarning("WeaponSwitcher component not found on the player.");
                }

                if (weaponSwitcher == null)
                {
                    Debug.LogWarning("WeaponSwitcher component not found on the player.");
                }

                return;
            }

            AmmoType currentAmmoType = weaponSwitcher.GetCurrentWeaponAmmoType();

            float chance = Random.value;
            Debug.Log($"Drop chance for ammo: {chance}");
            if (chance <= 0.25f) // 25% chance to increase ammo
            {
                switch (currentAmmoType)
                {
                    case AmmoType.PistolBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.PistolBullets, 15);
                        break;
                    case AmmoType.MPBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.MPBullets, 20);
                        break;
                    case AmmoType.AKMBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.AKMBullets, 15);
                        break;
                    default:
                        Debug.LogWarning("Current weapon ammo type is not recognized.");
                        break;
                }
            }
        }

        IEnumerator DamageCooldownRoutine()
        {
            canDealDamage = false;
            yield return new WaitForSeconds(DamageCooldown);
            canDealDamage = true;
        }


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
