using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour {

    // Configuration parameters
    [SerializeField] float maxHealth = 100f;
    [SerializeField] AudioClip[] damagedSFX;
    [SerializeField] [Range (0, 1)] float[] damagedSFXVolume;
    [SerializeField] TextMeshProUGUI healthText;

    // State variables
    float currentHealth;
    int damagedSFXIndex = 0;
    bool isDead = false;

    // Cached references
    DeathHandler myDeathHandler = null;
    AudioSource myAudioSource;
    DisplayDamage myDisplayDamage;

    // Start is called before the first frame update
    void Start() {
        myDeathHandler = GetComponent<DeathHandler>();
        myAudioSource = GetComponent<AudioSource>();
        myDisplayDamage = GetComponent<DisplayDamage>();

        currentHealth = maxHealth;

        healthText.text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void DamagePlayer(float damage) {
        currentHealth -= damage;

        healthText.text = currentHealth.ToString();

        if (damagedSFX.Length > 0) {
            myDisplayDamage.DisplayHurtOverlay();

            myAudioSource.Stop();

            damagedSFXIndex = Random.Range(0, damagedSFX.Length);
            myAudioSource.PlayOneShot(damagedSFX[damagedSFXIndex], damagedSFXVolume[damagedSFXIndex]);
        }

        if (currentHealth <= 0) {
            myDisplayDamage.DisableHurtOverlayOnDeath();

            healthText.text = "0";

            isDead = true;
            myDeathHandler.HandleDeath();
        }
    }

    public bool GetIsDead() {
        return isDead;
    }

}
