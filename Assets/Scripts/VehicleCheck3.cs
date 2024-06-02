using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VehicleCheck3 : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private TextMeshProUGUI _objectiveText;

    public AudioClip failedInteractionClip;
    private AudioSource audioSource;
    [SerializeField] private VideoPlayer player;

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
        else if (EnemySpawner.Instance.remainingEnemies == 0)
        {
            _currPrompt = "Proceed to next area";
        }
        else if (EnemySpawner.Instance.remainingEnemies <= 25)
        {
            _currPrompt = "I need to kill all remaining zombies!";
        }
    }

    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;
        if (inventory.hasCure && EnemySpawner.Instance.remainingEnemies == 0)
        {

            player.transform.parent.gameObject.SetActive(true);

            player.Play();
            Invoke("creditScene", 80f);
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

    public void creditScene()
    {
        SceneManager.LoadScene("Credit");
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
