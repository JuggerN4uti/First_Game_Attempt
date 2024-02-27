using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class field_e : MonoBehaviour
{
    public Image image;
    public Sprite disab, enab;

    public void normal()
    {
        image.sprite = disab;
    }

    public void red()
    {
        image.sprite = enab;
    }
}