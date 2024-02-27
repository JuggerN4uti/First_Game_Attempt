using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_Image : MonoBehaviour
{
    public AllStats stats;
    public Button thisbutton;
    public Image image;
    public Sprite melee, ranged, spell, buff;
    public TMPro.TextMeshProUGUI mana, fatigue;

    public char Type;

    void Update()
    {
        switch (Type)
        {
            case 'm':
                image.sprite = melee;
                break;
            case 'r':
                image.sprite = ranged;
                break;
            case 's':
                image.sprite = spell;
                break;
            case 'b':
                image.sprite = buff;
                break;
            case 'i':
                gameObject.SetActive(false);
                break;
        }
    }

    public void Set_text(float mana_cost, float current_fatigue)
    {
        mana.text = mana_cost.ToString("0");
        fatigue.text = current_fatigue.ToString("0");
    }

    public void Disable()
    {
        thisbutton.interactable = false;
    }

    public void Enable()
    {
        thisbutton.interactable = true;
    }
}