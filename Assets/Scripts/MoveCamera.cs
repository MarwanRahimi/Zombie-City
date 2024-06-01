using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraPosition == null)
        {
            Debug.LogWarning("cameraPosition is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraPosition != null)
        {
            transform.position = cameraPosition.position;
        }
        else
        {
            Debug.LogWarning("cameraPosition is null. Ensure it is assigned properly.");
        }
    }
}
