using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public HpText text;

    public float max;

    public void SetMaxHealth(float value)
    {
        max = value;
        slider.maxValue = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.Display(value, max);
    }

    public void SetHealth(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.Display(value, max);
    }
}