using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaygateHud : MonoBehaviour
{
    public Map map;
    public TMPro.TextMeshProUGUI footman_cost, marksman_cost, cavalier_cost, mage_cost, warband;
    public Button buy1, buy2, buy3, buy4;
    public GameObject Waygate;

    public int cost1, cost2, cost3, cost4, aviable_units;

    public void Pop()
    {
        cost1 = (PlayerPrefs.GetInt("unit1size") * Random.Range(5, 7) + Random.Range(-1, 2));
        cost2 = (PlayerPrefs.GetInt("unit2size") * Random.Range(5, 7) + Random.Range(-1, 2));
        cost3 = (PlayerPrefs.GetInt("unit3size") * Random.Range(5, 7) + Random.Range(-1, 2));
        cost4 = (PlayerPrefs.GetInt("unit4size") * Random.Range(5, 7) + Random.Range(-1, 2));

        aviable_units = map.max_warband - (map.footmen * PlayerPrefs.GetInt("unit1size") + map.marksmen * PlayerPrefs.GetInt("unit2size") + map.cavalry * PlayerPrefs.GetInt("unit3size") + map.mages * PlayerPrefs.GetInt("unit4size"));

        Check();
    }

    void Check()
    {
        footman_cost.text = cost1.ToString("");
        marksman_cost.text = cost2.ToString("");
        cavalier_cost.text = cost3.ToString("");
        mage_cost.text = cost4.ToString("");
        warband.text = "Warband: " + (map.max_warband - aviable_units).ToString("") + "/" + map.max_warband.ToString("");

        if (map.arcane_shards >= cost1 && aviable_units >= PlayerPrefs.GetInt("unit1size"))
            buy1.interactable = true;
        else buy1.interactable = false;

        if (map.arcane_shards >= cost2 && aviable_units >= PlayerPrefs.GetInt("unit2size"))
            buy2.interactable = true;
        else buy2.interactable = false;

        if (map.arcane_shards >= cost3 && aviable_units >= PlayerPrefs.GetInt("unit3size"))
            buy3.interactable = true;
        else buy3.interactable = false;

        if (map.arcane_shards >= cost4 && aviable_units >= PlayerPrefs.GetInt("unit4size"))
            buy4.interactable = true;
        else buy4.interactable = false;
    }

    public void Summon(int which)
    {
        switch (which)
        {
            case 1:
                map.arcane_shards -= cost1;
                cost1 += PlayerPrefs.GetInt("unit1size");
                aviable_units -= PlayerPrefs.GetInt("unit1size");
                map.footmen++;
                break;
            case 2:
                map.arcane_shards -= cost2;
                cost2 += PlayerPrefs.GetInt("unit2size");
                aviable_units -= PlayerPrefs.GetInt("unit2size");
                map.marksmen++;
                break;
            case 3:
                map.arcane_shards -= cost3;
                cost3 += PlayerPrefs.GetInt("unit3size");
                aviable_units -= PlayerPrefs.GetInt("unit3size");
                map.cavalry++;
                break;
            case 4:
                map.arcane_shards -= cost4;
                cost4 += PlayerPrefs.GetInt("unit4size");
                aviable_units -= PlayerPrefs.GetInt("unit4size");
                map.mages++;
                break;
        }
        map.UpdateCounters();
        Check();
    }

    public void Proceed()
    {
        map.free = true;

        map.ClearTile(map.current);
        Waygate.SetActive(false);
    }
}