using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.FPS.Game;

public class Weapon : MonoBehaviour {

    // Configuration parameters
    [SerializeField] public Camera FPCamera = null;
    [SerializeField] ParticleSystem muzzleFlash = null;
    [SerializeField] GameObject terrainHitEffect = null;
    [SerializeField] GameObject zombieHitEffect = null;
    [SerializeField] float delayDestroyZombieHitEffect = 0.5f;
    [SerializeField] float shootingRange = 100f;
    [SerializeField] float weaponDamage = 50f;
    [SerializeField] float delayDestroyTerrainHitEffect = 0.1f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] public Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] AudioClip gunshotSFX;
    [SerializeField] [Range(0, 1)] float gunshotSFXVolume = 1f;
    [SerializeField] public TextMeshProUGUI ammoText;

    // State variables
    bool canShoot = true;

    public AmmoType GetAmmoType()
    {
        Debug.LogWarning($"{ammoType}.");
        return ammoType;
    }

    // Cached references
    WeaponSwitcher myWeaponSwitcher = null;
    AudioSource myAudioSource;
    Animator myAnimator;

    void Start() {
        myWeaponSwitcher = GetComponentInParent<WeaponSwitcher>();
        myAudioSource = GetComponent<AudioSource>();
        myAnimator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0) && ammoSlot.GetCurrentAmmoAmount(ammoType) > 0 && canShoot) {
            if (myAnimator != null) {
                myAnimator.SetBool("isShooting", true);
            }
            StartCoroutine(Shoot());
        }

        else if (!Input.GetMouseButton(0) || ammoSlot.GetCurrentAmmoAmount(ammoType) <= 0){
            if (myAnimator != null) {
                myAnimator.SetBool("isShooting", false);
            }
        }

        DisplayAmmo();
    }

    void DisplayAmmo() {
        if (ammoType == AmmoType.PistolBullets) {
            int currentAmmo = ammoSlot.GetCurrentAmmoAmount(ammoType);
            ammoText.text = "1911 - " + currentAmmo.ToString();
        }
        else if (ammoType == AmmoType.MPBullets) {
            int currentAmmo = ammoSlot.GetCurrentAmmoAmount(ammoType);
            ammoText.text = "MP7 - " + currentAmmo.ToString();
        }
        else if (ammoType == AmmoType.AKMBullets) {
            int currentAmmo = ammoSlot.GetCurrentAmmoAmount(ammoType);
            ammoText.text = "AKM - " + currentAmmo.ToString();
        }
    }

    IEnumerator Shoot() {
        canShoot = false;
        myWeaponSwitcher.SetBoolCanSwitch(false);

        myAudioSource.Stop();
        myAudioSource.PlayOneShot(gunshotSFX, gunshotSFXVolume);

        ammoSlot.ReduceCurrentAmmoAmount(ammoType);
        PlayMuzzleFlash();
        ProcessRayCast();

        yield return new WaitForSeconds(timeBetweenShots);

        myWeaponSwitcher.SetBoolCanSwitch(true);
        canShoot = true;
    }

    void PlayMuzzleFlash() {
        muzzleFlash.Play();
    }

    void ProcessRayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, shootingRange, ~LayerMask.GetMask("Player")))
        {
            Health target = hit.transform.GetComponent<Health>();
            Debug.Log("Hit object: " + hit.transform.name);
            if (target == null)
            {
                CreateTerrainHitEffect(hit);
            }
            else
            {
                target.TakeDamage(weaponDamage);
                CreateZombieHitEffect(hit);
            }
        }
        else
        {
            return;
        }
    }

    void CreateTerrainHitEffect(RaycastHit hit) {
        GameObject terrainHitEffectObject = Instantiate(terrainHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(terrainHitEffectObject, delayDestroyTerrainHitEffect);
    }

    void CreateZombieHitEffect(RaycastHit hit) {
        GameObject zombieHitEffectObject = Instantiate(zombieHitEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
        Destroy(zombieHitEffectObject, delayDestroyZombieHitEffect);
    }

}
