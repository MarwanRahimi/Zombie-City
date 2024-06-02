using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private AudioClip[] weaponSwitchingSFX;
    [SerializeField][Range(0, 1)] private float[] weaponSwitchingSFXVolume;

    private int currentWeapon = 0;
    private bool canSwitch = true;

    private AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!canSwitch) { return; }

        ProcessKeyInput();
        ProcessScrollWheel();
    }

    private void ProcessKeyInput()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            KeyCode key = KeyCode.Alpha1 + i;
            if (Input.GetKeyDown(key))
            {
                SetCurrentWeapon(i);
                break;
            }
        }
    }

    private float scrollCooldown = 0.2f; 
    private float lastScrollTime;

    private void ProcessScrollWheel()
    {
        if (Time.time - lastScrollTime >= scrollCooldown)
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta != 0)
            {
                int scrollDirection = Mathf.RoundToInt(Mathf.Sign(scrollDelta));
                int newWeaponIndex = currentWeapon + scrollDirection;
                if (newWeaponIndex < 0)
                {
                    newWeaponIndex = transform.childCount - 1;
                }
                else if (newWeaponIndex >= transform.childCount)
                {
                    newWeaponIndex = 0;
                }
                SetCurrentWeapon(newWeaponIndex);
                lastScrollTime = Time.time;
            }
        }
    }



    public void SetCurrentWeapon(int weaponIndex)
    {
        if (weaponIndex == currentWeapon) { return; }

        Transform previousWeapon = transform.GetChild(currentWeapon);
        Transform newWeapon = transform.GetChild(weaponIndex);

        previousWeapon.gameObject.SetActive(false);
        newWeapon.gameObject.SetActive(true);

        currentWeapon = weaponIndex;

        HandleSwitchingWeaponsSFX();
    }

    private void HandleSwitchingWeaponsSFX()
    {
        if (weaponSwitchingSFX[currentWeapon] != null)
        {
            myAudioSource.Stop();
            myAudioSource.PlayOneShot(weaponSwitchingSFX[currentWeapon], weaponSwitchingSFXVolume[currentWeapon]);
        }
    }

    public void SetBoolCanSwitch(bool _canSwitch)
    {
        canSwitch = _canSwitch;
    }

    public int GetCurrentWeaponIndex()
    {
        return currentWeapon;
    }

    public AmmoType GetCurrentWeaponAmmoType()
    {
        Weapon currentWeaponComponent = transform.GetChild(currentWeapon).GetComponent<Weapon>();
        if (currentWeaponComponent != null)
        {
            Debug.LogWarning($"{currentWeapon}.");
            return currentWeaponComponent.GetAmmoType();

        }
        else
        {
            Debug.LogWarning("Current weapon does not have a Weapon component.");
            return AmmoType.None; 
        }
    }
}
