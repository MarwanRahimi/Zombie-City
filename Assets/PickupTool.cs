using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : MonoBehaviour
{
    [SerializeField] private string toolType;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var inventory = other.gameObject.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
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
                Debug.Log("Picked up a wrench!");
                break;
            case "wheel":
                inventory.hasWheel = true;
                Debug.Log("Picked up a wheel!");
                break;
            case "armor":
                inventory.hasArmor = true;
                Debug.Log("Picked up armor!");
                break;
            default:
                Debug.LogWarning($"Unknown tool type: {toolType}");
                break;
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, 100 * Time.deltaTime);
    }
}
