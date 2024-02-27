using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MpText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI mp;


    public void Display(float value)
    {
        mp.text = value.ToString("0");
    }
}