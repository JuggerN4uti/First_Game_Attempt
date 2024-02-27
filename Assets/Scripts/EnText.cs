using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI en;


    public void Display(float value)
    {
        en.text = value.ToString("0");
    }
}