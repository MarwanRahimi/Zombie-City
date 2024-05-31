using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] float zoomedOutFOV = 60f;
    [SerializeField] float zoomedInFOV = 35f;
    [SerializeField] float mouseSensitivityZoomedOut = 2f;

    // State variables
    bool isZoomedIn = false;
    float currentMouseSensitivity;

    // Cached references
    Camera fpsCamera = null;

    void Start()
    {
        fpsCamera = GetComponentInParent<Camera>();
        currentMouseSensitivity = mouseSensitivityZoomedOut;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isZoomedIn)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }
        }
    }

    private void ZoomOut()
    {
        isZoomedIn = false;
        fpsCamera.fieldOfView = zoomedOutFOV;
        currentMouseSensitivity = mouseSensitivityZoomedOut;
    }

    private void ZoomIn()
    {
        isZoomedIn = true;
        fpsCamera.fieldOfView = zoomedInFOV;
        currentMouseSensitivity = mouseSensitivityZoomedOut * (zoomedInFOV / zoomedOutFOV);
    }



    private void OnDisable()
    {
        if (isZoomedIn)
        {
            ZoomOut();
        }
    }
}
