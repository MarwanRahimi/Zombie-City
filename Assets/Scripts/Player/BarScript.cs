using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarScript : MonoBehaviour
{
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image healthAmount;

    [SerializeField]
    private TMP_Text valueText;

    [SerializeField]
    private Color fullColor;

    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            valueText.text = value.ToString();
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    private void Start()
    {
        if (lerpColors)
        {
            healthAmount.color = fullColor;
        }
    }
    private void Update()
    {
        HandleBar();
    }
    //method to handle the bar stats
    private void HandleBar()
    {
        if (fillAmount != healthAmount.fillAmount)
        {
            healthAmount.fillAmount = Mathf.Lerp(healthAmount.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
        if (lerpColors)
        {
            healthAmount.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }

        healthAmount.color = Color.Lerp(lowColor, fullColor, fillAmount);
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
