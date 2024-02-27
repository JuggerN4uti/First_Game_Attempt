using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public EnText text;

    public bool change = true;

    void Start()
    {
        slider.maxValue = 100;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetColor(char color)
    {
        change = false;

        switch (color)
        {
            case 'l':
                fill.color = new Color(0.9F, 1, 0.6f, 1);
                break;
            case 'w':
                fill.color = new Color(0.6F, 1, 1, 1);
                break;
            case 'n':
                fill.color = new Color(0.5F, 0.9f, 0.5f, 1);
                break;
            case 'b':
                fill.color = new Color(0.8F, 0.5f, 0.2f, 1);
                break;
        }
    }

    public void SetEnergy(float value)
    {
        slider.value = value;
        if (change == true)
            fill.color = gradient.Evaluate(slider.normalizedValue);
        text.Display(value);
    }
}