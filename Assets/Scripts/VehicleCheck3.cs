using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleCheck3 : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private TextMeshProUGUI _objectiveText;

    public AudioClip failedInteractionClip;
    private AudioSource audioSource;

    void Start()
    {
        if (_objectiveText != null)
        {
            _objectiveText.text = "Current Objective: Find the cure!";
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void UpdatePrompt()
    {
        var inventory = PlayerInventory.Instance;

        if (!inventory.hasCure)
        {
            _currPrompt = "I need to find the cure.";
        }
        else if (EnemySpawner.Instance.remainingEnemies <= 25)
        {
            _currPrompt = "I need to kill all remaining zombies!";
        }
        else
        {
            _currPrompt = "Proceed to next area";
        }
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;
        if (inventory.hasCure && EnemySpawner.Instance.remainingEnemies == 0)
        {
            
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

    private void UpdateObjectiveText()
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasCure)
        {
            if (EnemySpawner.Instance.remainingEnemies > 0)
            {
                _objectiveText.text = $"Current Objective: Kill all enemies! \nRemaining enemies:{EnemySpawner.Instance.remainingEnemies}";
            }
            else
            {
                _objectiveText.text = "Current Objective: Return to the vehicle!";
            }
        }
        else
        {
            _objectiveText.text = "Current Objective: Find the cure!";
        }
    }

    public void UpdateEnemyCount()
    {
        UpdateObjectiveText();
    }

    public void CurePickup()
    {
        UpdateObjectiveText();
    }
}
