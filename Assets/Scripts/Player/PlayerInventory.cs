using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]

    private Transform weaponSocket;
    public bool hasWrench = false;
    public bool hasWheel = false;
    public bool hasArmor = false;
    //public bool hasInflator = false;

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
}
