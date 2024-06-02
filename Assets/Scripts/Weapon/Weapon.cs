using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField] public Image weaponImage; // Ensure this is assigned in the Inspector

    bool canShoot = true;
    bool isMeleeing = false;
    GameObject knifeObject;

    // Cached references
    WeaponSwitcher myWeaponSwitcher = null;
    AudioSource myAudioSource;

    // Sprites for different weapon types
    Dictionary<AmmoType, Sprite> weaponSprites;

    public AmmoType GetAmmoType()
    {
        return ammoType;
    }

    void Start()
    {
        myWeaponSwitcher = GetComponentInParent<WeaponSwitcher>();
        myAudioSource = GetComponent<AudioSource>();

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Knife"))
            {
                knifeObject = obj;
                knifeObject.SetActive(false);
                break;
            }
        }

        if (knifeObject == null)
        {
            Debug.LogError("Knife GameObject not found.");
        }

        GetWeaponImage();
        UpdateWeaponUI();
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
        GetWeaponImage();
        DisplayAmmo();
        UpdateWeaponUI();
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

        if (knifeObject != null)
        {
            knifeObject.SetActive(true);
            Animator knifeAnimator = knifeObject.GetComponent<Animator>();
            if (knifeAnimator != null)
            {
                knifeAnimator.Play("KnifeAnimation");
            }
        }
        else
        {
            Debug.LogError("Knife GameObject not found.");
        }

        gameObject.SetActive(false);
        ProcessMelee();
        Invoke(nameof(ResetMelee), 0.6f);
    }

    void ResetMelee()
    {
        isMeleeing = false;

        if (knifeObject != null)
        {
            knifeObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Knife GameObject not found.");
        }

        gameObject.SetActive(true);
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
                target.TakeDamage(meleeDamage, gameObject); // Pass gameObject as the damage source
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
                target.TakeDamage(weaponDamage, gameObject); // Pass gameObject as the damage source
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

    public void GetWeaponImage()
    {
        // Initialize weapon sprites dictionary
        weaponSprites = new Dictionary<AmmoType, Sprite>()
        {
            { AmmoType.PistolBullets, Resources.Load<Sprite>("Images/pistol") },
            { AmmoType.MPBullets, Resources.Load<Sprite>("Images/mp7") },
            { AmmoType.AKMBullets, Resources.Load<Sprite>("Images/akm") },
        };
    }

    public void UpdateWeaponUI()
    {
        if (weaponImage == null)
        {
            Debug.LogError("Weapon Image reference is not assigned.");
            return;
        }

        if (weaponSprites.ContainsKey(ammoType))
        {
            weaponImage.sprite = weaponSprites[ammoType];
        }
        else
        {
            Debug.LogError("No sprite found for the current weapon type.");
        }
    }
}
