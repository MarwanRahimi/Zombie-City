using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleCheck : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private TextMeshProUGUI _objectiveText;

    public AudioClip failedInteractionClip;
    private AudioSource audioSource;


    void Start()
    {
        if(_objectiveText != null)
        {
            _objectiveText.text = "Current Objective: Gather supplies from zombies!";
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void UpdatePrompt()
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasGas && inventory.hasSupplies)
        {
            _currPrompt = "Proceed to next area";
        }
        else if (inventory.hasGas && !inventory.hasSupplies)
        {
            _currPrompt = "I still need rations";
        }
        else if (!inventory.hasGas && !inventory.hasSupplies)
        {
            _currPrompt = "I need to gather supplies";
        }
        else if (!inventory.hasGas && inventory.hasSupplies)
        {
            _currPrompt = "I still need gas";
        }
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasGas && inventory.hasSupplies)
        {
            SceneManager.LoadScene("Level3");
            return true;
        }
        else
        {
            if (failedInteractionClip != null && audioSource != null)
            {
                audioSource.clip = failedInteractionClip;
                audioSource.Play();
            }
            return false;
        }
    }
}
