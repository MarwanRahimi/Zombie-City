using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SliderControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;

    [SerializeField] private float maxSliderAmout = 100.0f;

    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmout;
        sliderText.text = localValue.ToString("0");
    }
}
