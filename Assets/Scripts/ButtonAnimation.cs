using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    private RectTransform buttonRectTransform;
    private bool isAnimating = false;
    private Vector3 originalScale;

    private void Awake()
    {
        buttonRectTransform = GetComponent<RectTransform>();

        // Set the initial position of the button on the X-axis
        buttonRectTransform.anchoredPosition = new Vector2(-979.44f, buttonRectTransform.anchoredPosition.y);

        // Zoom the button to the target position over 1 second
        buttonRectTransform.DOAnchorPosX(-3.8147e-06f, 1f).SetEase(Ease.OutCubic);

        originalScale = buttonRectTransform.localScale;
    }

    public void OnPointerEnter()
    {
        if (!isAnimating)
        {
            isAnimating = true;

            // Shake the button's position for 0.5 seconds with reduced intensity
            buttonRectTransform.DOShakePosition(0.5f, new Vector3(3f, 0f, 0f), 10, 90, false).OnComplete(() =>
            {
                isAnimating = false;
            });

            // Scale up the button
            buttonRectTransform.DOScale(originalScale * 1.1f, 0.3f).SetEase(Ease.OutCubic);
        }
    }

    public void OnPointerExit()
    {
        if (isAnimating)
        {
            // Stop any ongoing animations
            buttonRectTransform.DOKill();
        }

        // Reset the button's scale to its original scale
        buttonRectTransform.localScale = originalScale;

        // Reset the isAnimating flag
        isAnimating = false;
    }
}