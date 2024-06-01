using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;
        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        public float CurrentHealth { get; set; }
        public bool Invincible;

        [System.Serializable]
        public class Item
        {
            public GameObject prefab;
        }

        public Item[] dropItems;
        private static List<GameObject> droppedItems = new List<GameObject>();
        private static int nextItemIndex = 0;
        private static float dropChance = 0.2f;

        bool m_IsDead;

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

        public void TakeDamage(float damage)
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
                OnDamaged?.Invoke(trueDamageAmount, null);
            }

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
                DropItems();
                Destroy(gameObject);
                IncreasePlayerAmmo();
                EnemySpawner.Instance.OnEnemyKilled(); //increment enemies killed in spawner class
            }
        }

        void DropItems()
        {
            // Ensure the next item index is within bounds
            if (nextItemIndex < dropItems.Length)
            {
                Item item = dropItems[nextItemIndex];

                // Check if the item has already been dropped
                if (!droppedItems.Contains(item.prefab))
                {
                    float roll = Random.value;
                    Debug.Log($"Drop chance for item {item.prefab.name}: {roll}");

                    // Drop item based on drop chance or if it's one of the last remaining items
                    if (roll <= dropChance || EnemySpawner.Instance.IsLastEnemy())
                    {
                        Debug.Log($"Dropping item: {item.prefab.name}");
                        GameObject droppedItem = Instantiate(item.prefab, transform.position, Quaternion.identity);
                        droppedItem.transform.position += new Vector3(0, 1, 0);
                        droppedItems.Add(item.prefab);
                        dropChance = 0.3f;
                        nextItemIndex++;
                    }
                }
                else
                {
                    // If the item was already dropped, increment the index and try the next item
                    nextItemIndex++;
                    dropChance += 0.1f;
                    DropItems();
                }
            }
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
            WeaponSwitcher weaponSwitcher = player.GetComponentInChildren<WeaponSwitcher>();

            if (ammoComponent == null || switcher == null)
            {
                if (ammoComponent == null)
                {
                    Debug.LogWarning("Ammo component not found on the player GameObject.");
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
            if (chance <= 0.25f) // 50% chance to increase ammo
            {
                switch (currentAmmoType)
                {
                    case AmmoType.PistolBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.PistolBullets, 7);
                        break;
                    case AmmoType.MPBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.MPBullets, 10);
                        break;
                    case AmmoType.AKMBullets:
                        ammoComponent.IncreaseCurrentAmmoAmount(AmmoType.AKMBullets, 6);
                        break;
                    default:
                        Debug.LogWarning("Current weapon ammo type is not recognized.");
                        break;
                }
            }
        }
    }
}
