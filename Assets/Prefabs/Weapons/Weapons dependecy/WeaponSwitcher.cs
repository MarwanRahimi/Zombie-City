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

    private void ProcessScrollWheel()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            int newWeaponIndex = currentWeapon + (scrollDelta > 0 ? 1 : -1);
            newWeaponIndex = Mathf.Clamp(newWeaponIndex, 0, transform.childCount - 1);
            SetCurrentWeapon(newWeaponIndex);
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
            return currentWeaponComponent.GetAmmoType();
        }
        else
        {
            Debug.LogWarning("Current weapon does not have a Weapon component.");
            return AmmoType.None; // Return a default value or handle this case as needed
        }
    }
}
