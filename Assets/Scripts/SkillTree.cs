using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Map map;
    public GameObject SkillTreeHud;
    public Button choose;
    public TMPro.TextMeshProUGUI aviable_points, first_tree, second_tree, third_tree, perk_info;
    public TMPro.TextMeshProUGUI[] perks;
    public string[] tooltip;

    public int tree1, tree2, tree3, current;
    public int[] perk, max, required;

    public void PerkClicked(int which)
    {
        current = which;
        perk_info.text = tooltip[which];

        if (CheckAviability(which) == true)
        {
            choose.interactable = true;
        }
        else choose.interactable = false;
    }

    private bool CheckAviability(int which)
    {
        if (map.stat_poionts > 0)
        {
            if (which < 7)
            {
                if (perk[which] < max[which] && tree1 >= required[which])
                    return true;
                else return false;
            }
            else if (which < 14)
            {
                if (perk[which] < max[which] && tree2 >= required[which])
                    return true;
                else return false;
            }
            else if (which < 21)
            {
                if (perk[which] < max[which] && tree3 >= required[which])
                    return true;
                else return false;
            }
            else return false;
        }
        else return false;
    }

    public void Choose()
    {
        perk[current]++;
        if (current < 7)
        {
            tree1++;
            perks[21].text = tree1.ToString("");
        }
        else if (current < 14)
        {
            tree2++;
            perks[22].text = tree2.ToString("");
        }
        else
        {
            tree3++;
            perks[23].text = tree3.ToString("");
        }
        map.stat_poionts--;
        aviable_points.text = map.stat_poionts.ToString("");
        perks[current].text = perk[current].ToString("") + "/" + max[current].ToString("");
        if (CheckAviability(current) == true)
        {
            choose.interactable = true;
        }
        else choose.interactable = false;
    }

    public void Proceed()
    {
        SkillTreeHud.SetActive(false);
        map.UpdateCounters();
    }

    public void Update()
    {
        aviable_points.text = map.stat_poionts.ToString("");
    }
}