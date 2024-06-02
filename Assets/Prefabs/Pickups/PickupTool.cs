using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : MonoBehaviour
{
    [SerializeField] private string toolType;
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Z;

    public AudioClip pickup;
    private AudioSource audioSource;
    void Start()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var inventory = other.gameObject.GetComponent<PlayerInventory>();
            var audioSource = other.gameObject.GetComponent<AudioSource>();
            if (inventory != null)
            {
                if (pickup != null && audioSource != null)
                {
                    Debug.LogWarning("no audio");
                    audioSource.clip = pickup;
                    audioSource.Play();
                }
                HandlePickup(inventory);
            }
            else
            {
                Debug.Log("not found");
            }
            Destroy(gameObject);
        }
    }

    protected virtual void HandlePickup(PlayerInventory inventory)
    {
        switch (toolType.ToLower())
        {
            case "wrench":
                inventory.hasWrench = true;
                break;
            case "wheel":
                inventory.hasWheel = true;
                break;
            case "armor":
                inventory.hasArmor = true;
                break;
            case "gas":
                inventory.hasGas = true;
                break;
            case "food":
                inventory.hasSupplies = true;
                break;
            default:
                Debug.LogWarning($"Unknown tool type: {toolType}");
                break;
        }
    }

    public enum RotationAxis
    {
        None,
        X,
        Y,
        Z
    }
    void Update()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                transform.Rotate(100 * Time.deltaTime, 0f, 0f);
                break;
            case RotationAxis.Y:
                transform.Rotate(0f, 100 * Time.deltaTime, 0f);
                break;
            case RotationAxis.Z:
                transform.Rotate(0f, 0f, 100 * Time.deltaTime);
                break;
            case RotationAxis.None:
                // No rotation
                break;
        }
    }
}
