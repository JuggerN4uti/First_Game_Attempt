using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI no;
    public int max_number;

    public void SetMax(int value)
    {
        max_number = value;
        no.color = new Color(0.4f, 0.4f, 0.4f, 1);
    }


    public void Display(int value)
    {
        if (value < max_number)
            no.color = new Color(1, 0.2f, 0.2f, 1);
        else if (value > max_number)
            no.color = new Color(0.2f, 0.2f, 1f, 1);
        else no.color = new Color(0.4f, 0.4f, 0.4f, 1);

        no.text = value.ToString("0");
    }
}