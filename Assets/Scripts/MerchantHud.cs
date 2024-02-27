using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantHud : MonoBehaviour
{
    public Map map;
    public GameObject Merchant;
    public GameObject[] artefacts, artefacts2, artefacts3, buttonz;
    public Button[] buttons;
    public TMPro.TextMeshProUGUI[] costs;
    public int[] roll, cost;
    bool viable = false, reseted = false;

    public void Pop()
    {
        RollArtefacts();

        cost[3] = Random.Range(8, 11);
        cost[4] = Random.Range(28, 41);
        cost[5] = 8 + map.lvl;
        cost[6] = 10;

        reseted = false;
        UpdateCosts();
    }

    void UpdateCosts()
    {
        for (int i = 0; i < 7; i++)
        {
            costs[i].text = cost[i].ToString("");
            if (cost[i] > map.silver)
                buttons[i].interactable = false;
            else buttons[i].interactable = true;
        }
        if (reseted == true)
            buttons[6].interactable = false;
    }

    public void RollArtefacts()
    {
        artefacts[roll[0]].SetActive(false);
        artefacts2[roll[1]].SetActive(false);
        artefacts3[roll[2]].SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            viable = false;
            roll[i] = Random.Range(1, 7);
            if (roll[i] == 6)
            {
                // Epic rarity
                do
                {
                    roll[i] = Random.Range(53, 61);
                    if (roll[i] > 58)
                    {
                        if (map.Class == 'w')
                            roll[i] += 2;
                        else if (map.Class == 'n')
                            roll[i] += 4;
                        else if (map.Class == 'b')
                            roll[i] += 6;
                    }
                    if (map.items.collected[roll[i]] != true)
                        viable = true;
                } while (viable == false);
                cost[i] = Random.Range(180, 231);
            }
            else if (roll[i] > 3)
            {
                // Rare rarity
                do
                {
                    roll[i] = Random.Range(32, 44);
                    if (roll[i] > 40)
                    {
                        if (map.Class == 'w')
                            roll[i] += 3;
                        else if (map.Class == 'n')
                            roll[i] += 6;
                        else if (map.Class == 'b')
                            roll[i] += 9;
                    }
                    if (map.items.collected[roll[i]] != true)
                        viable = true;
                } while (viable == false);
                cost[i] = Random.Range(120, 161);
            }
            else
            {
                // Common rarity
                do
                {
                    roll[i] = Random.Range(0, 20);
                    if (roll[i] > 15)
                    {
                        if (map.Class == 'w')
                            roll[i] += 4;
                        else if (map.Class == 'n')
                            roll[i] += 8;
                        else if (map.Class == 'b')
                            roll[i] += 12;
                    }
                    if (map.items.collected[roll[i]] != true)
                        viable = true;
                } while (viable == false);
                cost[i] = Random.Range(80, 111);
            }

            buttonz[i].SetActive(true);

            switch (i)
            {
                case 0:
                    artefacts[roll[i]].SetActive(true);
                    break;
                case 1:
                    artefacts2[roll[i]].SetActive(true);
                    break;
                case 2:
                    artefacts3[roll[i]].SetActive(true);
                    break;
            }
        }
    }

    public void BuyArtefact(int which)
    {
        map.items.Take(roll[which]);
        map.silver -= cost[which];
        map.UpdateCounters();

        buttonz[which].SetActive(false);
        switch (which)
        {
            case 0:
                artefacts[roll[which]].SetActive(false);
                break;
            case 1:
                artefacts2[roll[which]].SetActive(false);
                break;
            case 2:
                artefacts3[roll[which]].SetActive(false);
                break;
        }
        UpdateCosts();
    }

    public void BuyOther(int which)
    {
        switch (which)
        {
            case 0:
                map.silver -= cost[3];
                map.arcane_shards++;
                cost[3]++;
                break;
            case 1:
                map.silver -= cost[4];
                map.stat_poionts++;
                cost[4] += 3;
                break;
            case 2:
                map.silver -= cost[5];
                for (int i = 0; i < 4; i++)
                {
                    map.stat_poionts += map.stats.Tree[i].tree1 + map.stats.Tree[i].tree2 + map.stats.Tree[i].tree3;
                    map.stats.Tree[i].tree1 = 0; map.stats.Tree[i].tree2 = 0; map.stats.Tree[i].tree3 = 0;
                    for (int j = 0; j < 21; j++)
                    {
                        map.stats.Tree[i].perk[j] = 0;
                    }
                }
                reseted = true;
                break;
            case 3:
                map.silver -= cost[6];
                RollArtefacts();
                cost[6] += 2;
                break;
        }
        map.UpdateCounters();
        UpdateCosts();
    }

    public void Proceed()
    {
        map.free = true;
        map.ClearTile(map.current);
        Merchant.SetActive(false);
    }
}