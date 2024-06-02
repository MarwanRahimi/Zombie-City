using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleCheck2 : MonoBehaviour, IInteractable
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
            _objectiveText.text = "Current Objective: Find the lab key!";
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }

    public void UpdatePrompt()
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasKey)
        {
            _currPrompt = "Proceed to next area";
        }
        else
        {
            _currPrompt = "I need to find the lab keys!";

        }

    }

    public bool Interact(Interactor interactor)
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.hasKey)
        {
            SceneManager.LoadScene("Level4");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Destroy(player);
            }
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