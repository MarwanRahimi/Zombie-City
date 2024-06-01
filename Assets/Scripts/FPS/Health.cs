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
        private HashSet<GameObject> droppedItems = new HashSet<GameObject>();
        private int nextItemIndex = 0;

        private int[] dropAtCounts = new int[] { 3, 7, 11, 14 }; 
        private int dropIndex = 0;

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


        public void DropItems()
        {
            int killedEnemies = EnemySpawner.Instance.maxSpawnNumber - EnemySpawner.Instance.remainingEnemies;
            Debug.Log($"killedEnemies: {killedEnemies}, dropIndex: {dropIndex}, nextItemIndex: {nextItemIndex}");
            Debug.Log(killedEnemies);
            Debug.Log(dropAtCounts[dropIndex]);
            // Check if the killed enemies count matches the next drop threshold
            if (killedEnemies == 3)
            {
                Item item = dropItems[0];
                Debug.Log($"Dropping item: {item.prefab.name}");
                GameObject droppedItem = Instantiate(item.prefab, transform.position, item.prefab.transform.rotation);
                droppedItem.transform.position += new Vector3(0, 1, 0);
                droppedItems.Add(item.prefab);
            }
            // Check if the killed enemies count matches the next drop threshold
            if (killedEnemies == 7)
            {
                Item item = dropItems[1];
                Debug.Log($"Dropping item: {item.prefab.name}");
                GameObject droppedItem = Instantiate(item.prefab, transform.position, item.prefab.transform.rotation);
                droppedItem.transform.position += new Vector3(0, 1, 0);
                droppedItems.Add(item.prefab);
            }
            // Check if the killed enemies count matches the next drop threshold
            if (killedEnemies == 11)
            {
                Item item = dropItems[2];
                Debug.Log($"Dropping item: {item.prefab.name}");
                GameObject droppedItem = Instantiate(item.prefab, transform.position, item.prefab.transform.rotation);
                droppedItem.transform.position += new Vector3(0, 1, 0);
                droppedItems.Add(item.prefab);
            }
            // Check if the killed enemies count matches the next drop threshold
            if (killedEnemies == 14)
            {
                Item item = dropItems[3];
                Debug.Log($"Dropping item: {item.prefab.name}");
                GameObject droppedItem = Instantiate(item.prefab, transform.position, item.prefab.transform.rotation);
                droppedItem.transform.position += new Vector3(0, 1, 0);
                droppedItems.Add(item.prefab);
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
