using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private PlayerStats health;

    [SerializeField]
    public PlayerStats stamina;

    [SerializeField]
    private GameObject deathUI;

    [Tooltip("Time in seconds without taking damage to start regenerating health")]
    public float HealthRegenCooldown = 5f;

    [Tooltip("Amount of health regenerated per second")]
    public float HealthRegenRate = 1f;

    [Tooltip("Stamina drain rate per second when sliding")]
    public float slideStaminaDrain = 15f;

    [Tooltip("Time in seconds without consuming stamina to start regenerating stamina")]
    public float StaminaRegenCooldown = 2f;

    [Tooltip("Amount of stamina regenerated per second")]
    public float StaminaRegenRate = 10f;

    [Tooltip("Time in seconds to start regenerating stamina after it reaches 0")]
    public float StaminaExhaustedCooldown = 5f;

    private Coroutine healthRegenCooldownCoroutine;
    private Coroutine healthRegenCoroutine;
    private Coroutine staminaRegenCooldownCoroutine;
    private Coroutine staminaRegenCoroutine;

    private float lastDamageTime;
    private float lastStaminaUseTime;
    private bool isDead;
    private bool isTakingDamage;
    private bool isSprinting;

    private void Awake()
    {
        health.Initialize();
        stamina.Initialize();
        isDead = false;
        isTakingDamage = false;
        isSprinting = false;
    }

    private void Start()
    {
        deathUI.SetActive(false);
    }

    void Update()
    {
        // for testing values
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     health.CurrentVal -= 10;
        // }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     health.CurrentVal += 10;
        // }

        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     stamina.CurrentVal -= 10;
        // }
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     stamina.CurrentVal += 10;
        // }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health.CurrentVal -= amount;

        if (health.CurrentVal <= 0)
        {
            health.CurrentVal = 0;
            isDead = true;
            deathUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        isTakingDamage = true;
        lastDamageTime = Time.time;
        if (healthRegenCoroutine != null)
        {
            StopCoroutine(healthRegenCoroutine);
            healthRegenCoroutine = null;
        }

        if (healthRegenCooldownCoroutine != null)
        {
            StopCoroutine(healthRegenCooldownCoroutine);
        }
        healthRegenCooldownCoroutine = StartCoroutine(HealthRegenCooldownRoutine());
    }

    private IEnumerator HealthRegenCooldownRoutine()
    {
        yield return new WaitForSeconds(HealthRegenCooldown);

        if (health.CurrentVal < health.MaxVal && !isDead && !isTakingDamage)
        {
            healthRegenCoroutine = StartCoroutine(RegenerateHealth());
        }
        else
        {
            isTakingDamage = false;
            healthRegenCooldownCoroutine = StartCoroutine(HealthRegenCooldownRoutine());
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (health.CurrentVal < health.MaxVal && !isDead)
        {
            health.CurrentVal += HealthRegenRate;
            health.CurrentVal = Mathf.Clamp(health.CurrentVal, 0, health.MaxVal);

            yield return new WaitForSeconds(1f);
        }

        healthRegenCoroutine = null;
    }

    public void PlayerSprinting(float sprintDrain)
    {
        if (isDead) return;

        stamina.CurrentVal -= sprintDrain;
        lastStaminaUseTime = Time.time;

        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
            staminaRegenCoroutine = null;
        }

        if (staminaRegenCooldownCoroutine != null)
        {
            StopCoroutine(staminaRegenCooldownCoroutine);
        }

        if (stamina.CurrentVal <= 0)
        {
            staminaRegenCooldownCoroutine = StartCoroutine(StaminaRegenCooldownRoutine(StaminaExhaustedCooldown));
        }
        else
        {
            staminaRegenCooldownCoroutine = StartCoroutine(StaminaRegenCooldownRoutine(StaminaRegenCooldown));
        }
    }

    public void PlayerSliding(float slideDrain)
    {
        if (isDead) return;

        stamina.CurrentVal -= slideDrain;
        lastStaminaUseTime = Time.time;

        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
            staminaRegenCoroutine = null;
        }

        if (staminaRegenCooldownCoroutine != null)
        {
            StopCoroutine(staminaRegenCooldownCoroutine);
        }

        if (stamina.CurrentVal <= 0)
        {
            staminaRegenCooldownCoroutine = StartCoroutine(StaminaRegenCooldownRoutine(StaminaExhaustedCooldown));
        }
        else
        {
            staminaRegenCooldownCoroutine = StartCoroutine(StaminaRegenCooldownRoutine(StaminaRegenCooldown));
        }
    }

    private IEnumerator StaminaRegenCooldownRoutine(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        if (stamina.CurrentVal < stamina.MaxVal && !isDead)
        {
            staminaRegenCoroutine = StartCoroutine(RegenerateStamina());
        }
    }

    private IEnumerator RegenerateStamina()
    {
        while (stamina.CurrentVal < stamina.MaxVal && !isDead)
        {
            stamina.CurrentVal += StaminaRegenRate * Time.deltaTime;
            stamina.CurrentVal = Mathf.Clamp(stamina.CurrentVal, 0, stamina.MaxVal);

            yield return null;
        }

        staminaRegenCoroutine = null;
    }
}
