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

                    if (roll <= dropChance)
                    {
                        Debug.Log($"Dropping item: {item.prefab.name}");
                        GameObject droppedItem = Instantiate(item.prefab, transform.position, Quaternion.identity);
                        droppedItem.transform.position += new Vector3(0,1,0);
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
    }
}
