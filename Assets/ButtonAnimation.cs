using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform buttonRectTransform;
    private Vector3 originalScale;
    private Sequence animationSequence;

    private void Awake()
    {
        buttonRectTransform = GetComponent<RectTransform>();

        buttonRectTransform.anchoredPosition = new Vector2(-979.44f, buttonRectTransform.anchoredPosition.y);

        buttonRectTransform.DOAnchorPosX(0f, 1f).SetEase(Ease.OutCubic);

        originalScale = buttonRectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationSequence != null && animationSequence.IsPlaying())
        {
            animationSequence.Kill();
        }

        animationSequence = DOTween.Sequence();

        animationSequence.Append(buttonRectTransform.DOShakePosition(0.5f, new Vector3(3f, 0f, 0f), 10, 90, false))
                         .Join(buttonRectTransform.DOScale(originalScale * 1.1f, 0.3f).SetEase(Ease.OutCubic))
                         .OnComplete(() =>
                         {
                             animationSequence = null; // Clear sequence
                         });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationSequence != null && animationSequence.IsPlaying())
        {
            animationSequence.Kill();
        }

        buttonRectTransform.DOScale(originalScale, 0.3f).SetEase(Ease.OutCubic);
    }
}
