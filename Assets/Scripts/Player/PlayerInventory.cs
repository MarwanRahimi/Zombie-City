using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;
    private TextMeshProUGUI _objectiveText;

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

    void Start()
    {
        GameObject objectiveObject = GameObject.FindGameObjectWithTag("Objective");
        if (objectiveObject != null)
        {
            _objectiveText = objectiveObject.GetComponent<TextMeshProUGUI>();
            _objectiveText.text = "Current Objective: Repair the vehicle";
        }
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
