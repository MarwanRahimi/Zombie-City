using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private bool hasUsedWrench = false;
    [SerializeField] private bool hasUsedWheel = false;
    [SerializeField] private bool isFixed = false;
    public GameObject wheelPrefab;
    public GameObject armorPrefab;
    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;
        if (inventory == null)
        {
            return false;
        }

        if (!hasUsedWrench && inventory.hasWrench)
        {
            inventory.hasWrench = false;
            hasUsedWrench = true;
            transform.rotation = Quaternion.identity;
            UpdatePrompt();
            return true;
        }
        else if (hasUsedWrench && inventory.hasWheel)
        {
            inventory.hasWheel = false;
            Destroy(gameObject);
            GameObject instantiatedVehicle = Instantiate(wheelPrefab, transform.position, Quaternion.identity);
            instantiatedVehicle.transform.rotation = Quaternion.identity;
            UpdatePrompt();
            return true;
        }
        else if (hasUsedWheel && inventory.hasArmor)
        {
            inventory.hasArmor = false;
            Destroy(gameObject);
            Instantiate(armorPrefab, transform.position, transform.rotation);
            UpdatePrompt();
            return true;
        }

        UpdatePrompt();
        return false;
    }

    public void UpdatePrompt()
    {
        if(isFixed)
        {
            _currPrompt = "Proceed to next area";

        }
        else if (!hasUsedWrench && PlayerInventory.Instance.hasWrench)
        {
            _currPrompt = "Press F to use Wrench";
        }
        else if (!hasUsedWheel && PlayerInventory.Instance.hasWheel)
        {
            _currPrompt = "Press F to use Wheel";
        }
        else if (!isFixed && PlayerInventory.Instance.hasArmor)
        {
            _currPrompt = "Press F to use Armor";
        }
        else if (!hasUsedWrench && !PlayerInventory.Instance.hasWrench)
        {
            _currPrompt = "Find wrench to fix vehicle";
        }
        else if (!hasUsedWheel && !PlayerInventory.Instance.hasWheel)
        {
            _currPrompt = "Find wheel to fix vehicle";
        }
        else if (!isFixed && !PlayerInventory.Instance.hasArmor)
        {
            _currPrompt = "Find armor to fix vehicle";
        }
        else 
        {
            _currPrompt = "Find a repair tool!";
        }
        
    }


}
