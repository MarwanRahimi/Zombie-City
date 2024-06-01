using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.FPS.Game;

public class Weapon : MonoBehaviour
{
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
    [SerializeField][Range(0, 1)] float gunshotSFXVolume = 1f;
    [SerializeField] public TextMeshProUGUI ammoText;
    [SerializeField] float meleeRange = 2f;
    [SerializeField] float meleeDamage = 25f;
    [SerializeField] AudioClip meleeSFX;
    [SerializeField][Range(0, 1)] float meleeSFXVolume = 1f;
    [SerializeField] Animator knifeAnimator = null;
    [SerializeField] GameObject knife;

    bool canShoot = true;
    bool isMeleeing = false;

    // Cached references
    WeaponSwitcher myWeaponSwitcher = null;
    AudioSource myAudioSource;

    public AmmoType GetAmmoType()
    {
        Debug.LogWarning($"{ammoType}.");
        return ammoType;
    }

    void Start()
    {
        knife.SetActive(false);
        myWeaponSwitcher = GetComponentInParent<WeaponSwitcher>();
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && ammoSlot.GetCurrentAmmoAmount(ammoType) > 0 && canShoot)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.V) && !isMeleeing)
        {
            MeleeAttack();
        }

        DisplayAmmo();
    }

    void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmoAmount(ammoType);
        ammoText.text = ammoType.ToString() + " - " + currentAmmo.ToString();
    }

    void Shoot()
    {
        canShoot = false;
        myWeaponSwitcher.SetBoolCanSwitch(false);

        myAudioSource.Stop();
        myAudioSource.PlayOneShot(gunshotSFX, gunshotSFXVolume);

        ammoSlot.ReduceCurrentAmmoAmount(ammoType);
        PlayMuzzleFlash();
        ProcessRayCast();

        Invoke(nameof(ResetShoot), timeBetweenShots);
    }

    void ResetShoot()
    {
        myWeaponSwitcher.SetBoolCanSwitch(true);
        canShoot = true;
    }

    void MeleeAttack()
    {
        isMeleeing = true;
        myWeaponSwitcher.SetBoolCanSwitch(false);

        // Activate the knife
        if (knife != null)
        {
            knife.SetActive(true);
            AudioSource knifeAudioSource = knife.GetComponent<AudioSource>();
            if (knifeAudioSource != null && knifeAudioSource.enabled)
            {
                knifeAudioSource.Stop();
                knifeAudioSource.PlayOneShot(meleeSFX, meleeSFXVolume);
            }
        }
        gameObject.SetActive(false);

        // Play the melee animation if the animator is assigned
        if (knifeAnimator != null)
        {
            knifeAnimator.Play("KnifeAnimation");
        }

        // Process melee attack logic
        ProcessMelee();

        // Reset melee state and deactivate the knife after animation duration
        Invoke(nameof(ResetMelee), 0.6f); 
    }


    void ResetMelee()
    {
        isMeleeing = false;

        // Deactivate the knife
        if (knife != null)
        {
            knife.SetActive(false);
        }

        // Re-enable the gun
        gameObject.SetActive(true);

        // Allow switching weapons again
        myWeaponSwitcher.SetBoolCanSwitch(true);
    }


    void ProcessMelee()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, meleeRange, ~LayerMask.GetMask("Player")))
        {
            Health target = hit.transform.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(meleeDamage);
                CreateZombieHitEffect(hit);
            }
            else
            {
                CreateTerrainHitEffect(hit);
            }
        }
    }

    void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    void ProcessRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, shootingRange, ~LayerMask.GetMask("Player")))
        {
            Health target = hit.transform.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
                CreateZombieHitEffect(hit);
            }
            else
            {
                CreateTerrainHitEffect(hit);
            }
        }
    }

    void CreateTerrainHitEffect(RaycastHit hit)
    {
        if (terrainHitEffect != null)
        {
            GameObject terrainHitEffectObject = Instantiate(terrainHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(terrainHitEffectObject, delayDestroyTerrainHitEffect);
        }
    }

    void CreateZombieHitEffect(RaycastHit hit)
    {
        if (zombieHitEffect != null)
        {
            GameObject zombieHitEffectObject = Instantiate(zombieHitEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            Destroy(zombieHitEffectObject, delayDestroyZombieHitEffect);
        }
    }
}