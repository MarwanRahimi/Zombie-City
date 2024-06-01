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
        // Find and assign the references directly
        GameObject playerCamObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (playerCamObject != null)
        {
            FPCamera = playerCamObject.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("PlayerCam not found in the scene.");
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            ammoSlot = playerObject.GetComponent<Ammo>();
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }

        GameObject ammoTextObject = GameObject.FindGameObjectWithTag("Ammo");
        if (ammoTextObject != null)
        {
            ammoText = ammoTextObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("AmmoText not found in the scene.");
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
                newWeapon.transform.localRotation = Quaternion.identity;

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
        // Set FPCamera, ammoSlot, and ammoText to newWeapon
        // You may need to adjust this according to how these references are used in the newWeapon's components
        if (newWeapon != null)
        {
            // Example: Setting references to newWeapon's components
            // Assuming the components are named appropriately and accessible via GetComponent
            Weapon weaponScript = newWeapon.GetComponent<Weapon>();
            if (weaponScript != null)
            {
                weaponScript.FPCamera = FPCamera;
                weaponScript.ammoSlot = ammoSlot;
                weaponScript.ammoText = ammoText;
            }
            else
            {
                Debug.LogWarning("YourWeaponScript component not found on new weapon.");
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
