using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;

    public static PlayerInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(PlayerInventory).Name);
                _instance = singletonObject.AddComponent<PlayerInventory>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private Transform weaponSocket;
    public bool hasWrench = false;
    public bool hasWheel = false;
    public bool hasArmor = false;
    public bool hasGas = false;
    public bool hasSupplies = false;
    public bool hasKey = false;
    public bool hasCure = false;
    public bool PickupWeapon(Transform weaponToPickup)
    {
        if (weaponSocket)
        {
            weaponToPickup.SetParent(weaponSocket);
            weaponToPickup.transform.localPosition = Vector3.zero;
            return true;
        }
        return false;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }
}
