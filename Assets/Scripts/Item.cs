using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory))
        {
            if (inventory.PickupWeapon(transform.GetChild(0)))
            {
                Destroy(gameObject);
            }
        }
    }
}
