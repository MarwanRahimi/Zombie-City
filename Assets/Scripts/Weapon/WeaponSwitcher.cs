using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {

    // Configuration parameters
    [SerializeField] AudioClip[] weaponSwitchingSFX;
    [SerializeField] [Range (0, 1)] float[] weaponSwitchingSFXVolume;

    // State variables
    int currentWeapon = 0;
    bool canSwitch = true;

    // Cached references
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start() {
        myAudioSource = GetComponent<AudioSource>();
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update() {
        int previousWeapon = currentWeapon;

        if (!canSwitch) { return; }

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeapon) {
            SetWeaponActive();
            HandleSwitchingWeaponsSFX();
        }
    }

    void ProcessKeyInput() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentWeapon = 2;
        }
    }

    void ProcessScrollWheel() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (currentWeapon > 0) {
                currentWeapon--;
            }
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (currentWeapon < transform.childCount - 1) {
                currentWeapon++;
            }
        }
    }

    void SetWeaponActive() {
        int weaponIndex = 0;

        foreach (Transform weapon in transform) {
            if (weaponIndex == currentWeapon) {
                weapon.gameObject.SetActive(true);
            }

            else {
                weapon.gameObject.SetActive(false);
            }

            weaponIndex++;
        }
    }

    void HandleSwitchingWeaponsSFX() {
        if (weaponSwitchingSFX[currentWeapon] != null) {
            myAudioSource.Stop();
            myAudioSource.PlayOneShot(weaponSwitchingSFX[currentWeapon], weaponSwitchingSFXVolume[currentWeapon]);
        }
    }

    public void SetBoolCanSwitch (bool _canSwitch) {
        canSwitch = _canSwitch;
    }

}
