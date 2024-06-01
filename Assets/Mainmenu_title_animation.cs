using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu_title_animation : MonoBehaviour
{
    public float scaleDuration = 1f; // Duration for scaling animation
    public AnimationCurve scaleCurve; // Animation curve for scaling animation

    private Vector3 originalScale;
    private float timer = 0f;
    private bool isScaling = false;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero; // Start with scale zero

        // Start scaling animation automatically
        StartScalingAnimation();
    }

    private void Update()
    {
        if (isScaling)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / scaleDuration);
            float scaleValue = scaleCurve.Evaluate(t);
            transform.localScale = originalScale * scaleValue;

            if (t >= 1f)
            {
                isScaling = false;
            }
        }
    }

    public void StartScalingAnimation()
    {
        isScaling = true;
        timer = 0f;
    }
}