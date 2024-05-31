using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour {

    // Configuration parameters
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume;

    // Cached references
    WeaponSwitcher myWeaponSwitcher = null;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start() {
        gameOverCanvas.enabled = false;
        myWeaponSwitcher = GetComponentInChildren<WeaponSwitcher>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void HandleDeath() {
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(deathSFX, deathSFXVolume);

        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        myWeaponSwitcher.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
