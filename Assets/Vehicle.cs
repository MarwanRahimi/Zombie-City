using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    private bool hasUsedWrench = false;
    [SerializeField] private bool hasUsedWheel = false;
    [SerializeField] private bool isFixed = false;
    public GameObject wheelPrefab;
    public GameObject armorPrefab;
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            return false;
        }

        if (!hasUsedWrench && inventory.hasWrench)
        {
            Debug.Log("Using wrench");
            inventory.hasWrench = false;
            hasUsedWrench = true;
            return true;
        }
        else if (hasUsedWrench && inventory.hasWheel)
        {
            Debug.Log("Using wheel");
            inventory.hasWheel = false;
            Destroy(gameObject);
            GameObject instantiatedVehicle = Instantiate(wheelPrefab, transform.position, Quaternion.identity);
            instantiatedVehicle.transform.rotation = Quaternion.identity;
            return true;
        }
        else if (hasUsedWheel && inventory.hasArmor)
        {
            Debug.Log("Using armor");
            inventory.hasArmor = false;
            Destroy(gameObject);
            Instantiate(armorPrefab, transform.position, transform.rotation);
            return true;
        }
        else if (!isFixed)
        { 
        Debug.Log("Find a repair tool!");
        return false;
        }

        Debug.Log("Back to home!");
        return false;
    }


}
