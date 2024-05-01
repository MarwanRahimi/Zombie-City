using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    private bool hasUsedWrench = false;
    private bool hasUsedWheel = false;


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
            hasUsedWheel = true;
            return true;
        }
        else if (hasUsedWheel && inventory.hasArmor)
        {
            Debug.Log("Using armor");
            inventory.hasArmor = false;
            return true;
        }


        //Debug.Log("Using wheel");
        //Debug.Log("Using armor");
        Debug.Log("Find a repair tool!");
        return false;


    }


}
