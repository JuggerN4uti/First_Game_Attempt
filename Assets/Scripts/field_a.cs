using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class field_a : MonoBehaviour
{
    public Image image;
    public Sprite impossible, possible, active;

    public void normal()
    {
        image.sprite = impossible;
    }

    public void green()
    {
        image.sprite = possible;
    }

    public void blue()
    {
        image.sprite = active;
    }
}