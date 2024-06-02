using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Vehicle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _currPrompt;
    public string Prompt => _currPrompt;
    [SerializeField] private bool hasUsedWrench = false;
    [SerializeField] private bool hasUsedWheel = false;
    [SerializeField] private bool isFixed = false;
    private TextMeshProUGUI _objectiveText;
    public GameObject wheelPrefab;
    public GameObject armorPrefab;
    public AudioClip failedInteractionClip;
    public AudioClip successfulInteractionClip;
    private AudioSource audioSource;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            audioSource = player.GetComponent<AudioSource>();
        }
        GameObject objectiveObject = GameObject.FindGameObjectWithTag("Objective");
        if (objectiveObject != null)
        {
            _objectiveText = objectiveObject.GetComponent<TextMeshProUGUI>();
            if(_objectiveText.text == "") 
            { 
            _objectiveText.text = "Current Objective: Repair the vehicle";
            }
        }
    }

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
            Quaternion currentRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(0f, currentRotation.eulerAngles.y, 0f);
            transform.rotation = newRotation;
            success();
            UpdatePrompt();
            return true;
        }
        else if (hasUsedWrench && inventory.hasWheel)
        {
            inventory.hasWheel = false;
            Destroy(gameObject);
            GameObject instantiatedVehicle = Instantiate(wheelPrefab, transform.position, transform.rotation);
            success();
            UpdatePrompt();
            return true;
        }
        else if (hasUsedWheel && inventory.hasArmor)
        {
            inventory.hasArmor = false;
            Destroy(gameObject);
            Instantiate(armorPrefab, transform.position, transform.rotation);
            success();
            UpdatePrompt();
            return true;
        }
        else if (isFixed)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Destroy(player);
            }
            SceneManager.LoadScene("Level2");
            return true;
        }

        if (failedInteractionClip != null && audioSource != null)
        {
            audioSource.clip = failedInteractionClip;
            audioSource.Play();
        }
        UpdatePrompt();
        return false;
    }

    private void success()
    {
        if (successfulInteractionClip != null && audioSource != null)
        {
            audioSource.clip = successfulInteractionClip;
            audioSource.Play();
        }
    }

    public void UpdatePrompt()
    {
        if (isFixed)
        {
            _currPrompt = "Proceed to next area";
        }
        else if (!hasUsedWrench && PlayerInventory.Instance.hasWrench)
        {
            _currPrompt = "Press F to use Wrench";
        }
        else if (!hasUsedWheel && PlayerInventory.Instance.hasWheel && hasUsedWrench)
        {
            _currPrompt = "Press F to use Wheel";
        }
        else if (!isFixed && PlayerInventory.Instance.hasArmor && hasUsedWheel && hasUsedWrench)
        {
            _currPrompt = "Press F to use Armor";
        }
        else if (!hasUsedWrench && !PlayerInventory.Instance.hasWrench)
        {
            _currPrompt = "Find wrench to fix vehicle";
        }
        else if (!hasUsedWheel && !PlayerInventory.Instance.hasWheel && hasUsedWrench)
        {
            _currPrompt = "Find wheel to fix vehicle";
        }
        else if (!isFixed && !PlayerInventory.Instance.hasArmor && hasUsedWheel && hasUsedWrench)
        {
            _currPrompt = "Find armor to fix vehicle";
        }
        else
        {
            _currPrompt = "Find a repair tool!";
        }
    }
}
