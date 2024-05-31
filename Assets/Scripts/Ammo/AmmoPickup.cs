using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    // Configuration parameters
    [SerializeField] int ammoAmount = 5;
    [SerializeField] AmmoType ammoType;
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] [Range (0, 1)] float pickupSFXVolume = 1f;

    // State variables
    bool playerIsTouchingPickup = false;

    // Cached references
    Ammo playerAmmo = null;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        HandlePickupInput();
    }

    private void HandlePickupInput() {
        if (playerIsTouchingPickup && Input.GetKeyDown(KeyCode.E) && playerAmmo != null) {
            playerAmmo.IncreaseCurrentAmmoAmount(ammoType, ammoAmount);
            AudioSource.PlayClipAtPoint(pickupSFX, transform.position, pickupSFXVolume);
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerAmmo = other.gameObject.GetComponent<Ammo>();
            playerIsTouchingPickup = true;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerIsTouchingPickup = false;
        }
    }


}
