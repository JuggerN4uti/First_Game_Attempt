using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UnitsSelect : MonoBehaviour
{
    public TMPro.TextMeshProUGUI AvialableUnits, Unit1, Unit2, Unit3, Unit4, Unit1size, Unit2size, Unit3size, Unit4size, Shards, Experience;
    public Button proceed;
    public Button[] plus, minus;

    private int aviableUnits, maxUnits, shards, expierience;
    public int[] unitCount, unitSize;

    void Start()
    {
        maxUnits = PlayerPrefs.GetInt("maxUnits");
        aviableUnits = PlayerPrefs.GetInt("maxUnits");
        unitSize[0] = PlayerPrefs.GetInt("unit1size");
        unitSize[1] = PlayerPrefs.GetInt("unit2size");
        unitSize[2] = PlayerPrefs.GetInt("unit3size");
        unitSize[3] = PlayerPrefs.GetInt("unit4size");

        Unit1size.text = unitSize[0].ToString("");
        Unit2size.text = unitSize[1].ToString("");
        Unit3size.text = unitSize[2].ToString("");
        Unit4size.text = unitSize[3].ToString("");
        Display();
    }

    public void Display()
    {
        AvialableUnits.text = (maxUnits - aviableUnits).ToString("") + "/" + maxUnits.ToString("");
        Unit1.text = unitCount[0].ToString("");
        Unit2.text = unitCount[1].ToString("");
        Unit3.text = unitCount[2].ToString("");
        Unit4.text = unitCount[3].ToString("");

        for (int i = 0; i < 4; i++)
        {
            if (aviableUnits < unitSize[i])
                plus[i].interactable = false;
            else plus[i].interactable = true;

            if (unitCount[i] == 0)
                minus[i].interactable = false;
            else minus[i].interactable = true;
        }

        if (aviableUnits > 5)
        {
            proceed.interactable = false;
            Shards.text = "";
            Experience.text = "";
        }
        else
        {
            proceed.interactable = true;
            shards = 3 * aviableUnits + (aviableUnits * (aviableUnits + 1) / 2);
            expierience = 60 * aviableUnits + (aviableUnits * (aviableUnits + 1) * 5);
            Shards.text = "+" + shards.ToString("0");
            Experience.text = "+" + expierience.ToString("0");
        }
    }

    public void Plus(int which)
    {
        unitCount[which]++;
        aviableUnits -= unitSize[which];
        Display();
    }

    public void Minus(int which)
    {
        unitCount[which]--;
        aviableUnits += unitSize[which];
        Display();
    }

    public void Proceed()
    {
        PlayerPrefs.SetInt("unit1count", unitCount[0]);
        PlayerPrefs.SetInt("unit2count", unitCount[1]);
        PlayerPrefs.SetInt("unit3count", unitCount[2]);
        PlayerPrefs.SetInt("unit4count", unitCount[3]);
        PlayerPrefs.SetInt("bonusShards", shards);
        PlayerPrefs.SetInt("bonusExperience", expierience);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}