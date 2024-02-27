using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI hp;

    public void Display(float value, float m_value)
    {
        hp.text = value.ToString("0") + "/" + m_value.ToString("0");
    }
}