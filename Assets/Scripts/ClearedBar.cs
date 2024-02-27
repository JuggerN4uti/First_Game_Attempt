using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearedBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMPro.TextMeshProUGUI Text;

    private int max = 100;

    public void SetValue(int value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (value > max)
            Text.text = "100%";
        else Text.text = value.ToString("0") + "/" + max.ToString("0");
    }

    public void SetMaxValue(int value)
    {
        max = value;
        slider.maxValue = value;
        SetValue(0);
    }
}