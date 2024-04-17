using UnityEngine;

public class BackgroundMouseMovement : MonoBehaviour
{
    public float sensitivity = 5f;
    public float zoomLevel = 1f;
    public float trackingDelay = 0.1f;

    private RectTransform rectTransform;
    private Vector2 targetPosition;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        targetPosition += new Vector2(mouseX, mouseY) * trackingDelay;

        float scale = Mathf.Clamp(zoomLevel, 0.1f, 10f); // Limit the zoom level within a specific range
        rectTransform.localScale = new Vector3(scale, scale, 1f);

        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, trackingDelay);
    }
}