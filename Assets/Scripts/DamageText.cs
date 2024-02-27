using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dmg;

    public void Display(float value, char hue)
    {
        if (hue == 's')
            dmg.color = new Color(0.3F, 0.1F, 1, 1);
        if ((hue == 'm') || (hue == 'r'))
            dmg.color = new Color(1, 0, 0, 1);
        if (hue == 'h')
            dmg.color = new Color(0, 0.7F, 0.1F, 1);
        if (hue == 'y')
            dmg.color = new Color(1, 1, 0, 1);

        if (value == 0)
            dmg.text = "blocked!";
        else
            dmg.text = value.ToString("0");
    }
}