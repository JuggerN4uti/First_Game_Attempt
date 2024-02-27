using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TMPro.TextMeshProUGUI Text;

    private int max, current;

    public void SetMaxValue(int value)
    {
        max = value;
        slider.maxValue = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        Display();
    }

    public void SetValue(int value)
    {
        current = value;
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        Display();
    }

    private void Display()
    {
        Text.text = current.ToString("0") + "/" + max.ToString("0");
    }
}