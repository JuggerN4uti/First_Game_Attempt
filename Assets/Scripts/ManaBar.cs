using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public MpText text;

    public void SetMaxMana(float value)
    {
        slider.maxValue = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMana(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        text.Display(value);
    }
}