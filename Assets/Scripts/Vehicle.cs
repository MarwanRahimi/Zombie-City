using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            return false;
        }

        if(inventory.hasWrench)
        {
        Debug.Log("Using wrench");
        inventory.hasWrench = false;
        return true;
        }

        Debug.Log("Find a repair tool!");
        return false;
    }

 
}
