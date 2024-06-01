using UnityEngine;
using TMPro;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private WeaponSwitcher weaponSwitcher;

    private Camera FPCamera;
    private Ammo ammoSlot;
    private TextMeshProUGUI ammoText;

    private void Start()
    {
        InitializeReferences();
    }

    private void InitializeReferences()
    {
        // Initialize WeaponSwitcher
        GameObject switcherObject = GameObject.FindGameObjectWithTag("Switcher");
        if (switcherObject != null)
        {
            weaponSwitcher = switcherObject.GetComponent<WeaponSwitcher>();
            if (weaponSwitcher == null)
            {
                Debug.LogError("WeaponSwitcher component not found on switcherObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'Switcher' not found in the scene.");
        }

        // Initialize FPCamera
        GameObject playerCamObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (playerCamObject != null)
        {
            FPCamera = playerCamObject.GetComponent<Camera>();
            if (FPCamera == null)
            {
                Debug.LogError("Camera component not found on playerCamObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'MainCamera' not found in the scene.");
        }

        // Initialize AmmoSlot
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            ammoSlot = playerObject.GetComponent<Ammo>();
            if (ammoSlot == null)
            {
                Debug.LogError("Ammo component not found on playerObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'Player' not found in the scene.");
        }

        // Initialize AmmoText
        GameObject ammoTextObject = GameObject.FindGameObjectWithTag("Ammo");
        if (ammoTextObject != null)
        {
            ammoText = ammoTextObject.GetComponent<TextMeshProUGUI>();
            if (ammoText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found on ammoTextObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject with tag 'Ammo' not found in the scene.");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Find the Weapons game object
            GameObject weaponsParent = GameObject.Find("Weapons");

            if (weaponsParent != null)
            {
                GameObject newWeapon = Instantiate(weaponPrefab, weaponsParent.transform);
                newWeapon.transform.localPosition = Vector3.zero;

                SetReferencesToNewWeapon(newWeapon);

                // Find the index of the newly instantiated weapon
                int newIndex = weaponsParent.transform.childCount - 1;

                // Make the picked up weapon the active weapon through WeaponSwitcher
                if (weaponSwitcher != null)
                {
                    weaponSwitcher.SetCurrentWeapon(newIndex);
                }
                else
                {
                    Debug.LogWarning("WeaponSwitcher reference is not set.");
                }
            }
            else
            {
                Debug.LogWarning("No GameObject named 'Weapons' found in the scene.");
            }

            // Destroy the pickup object
            Destroy(gameObject);
        }
    }

    private void SetReferencesToNewWeapon(GameObject newWeapon)
    {
        if (newWeapon != null)
        {
            Weapon weaponScript = newWeapon.GetComponent<Weapon>();
            if (weaponScript != null)
            {
                weaponScript.FPCamera = FPCamera;
                weaponScript.ammoSlot = ammoSlot;
                weaponScript.ammoText = ammoText;
            }
            else
            {
                Debug.LogWarning("Weapon script component not found on new weapon.");
            }
        }
        else
        {
            Debug.LogWarning("New weapon is null.");
        }
    }

    void Update()
    {
        // Rotate the pickup object for a visual effect
        transform.Rotate(0f, 100 * Time.deltaTime, 0f);
    }
}
