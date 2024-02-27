using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ability_Info : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AllStats stats;
    public GameObject[] tooltipsL, tooltipsW, tooltipsN, tooltipsB;
    public GameObject current;

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (stats.whichHero)
        {
            case 'l':
                tooltipsL[stats.whoseTurn - 1].SetActive(true);
                current = tooltipsL[stats.whoseTurn - 1];
                break;
            case 'w':
                tooltipsW[stats.whoseTurn - 1].SetActive(true);
                current = tooltipsW[stats.whoseTurn - 1];
                break;
            case 'n':
                tooltipsN[stats.whoseTurn - 1].SetActive(true);
                current = tooltipsN[stats.whoseTurn - 1];
                break;
            case 'b':
                tooltipsB[stats.whoseTurn - 1].SetActive(true);
                current = tooltipsB[stats.whoseTurn - 1];
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        current.SetActive(false);
    }

    public void Disactivate()
    {
        if (current == true)
            current.SetActive(false);
    }
}