using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactHud : MonoBehaviour
{
    public Map map;
    public GameObject Hud;
    public GameObject[] artefacts, artefacts2;
    int roll, roll2;
    bool viable = false;

    public void Pop()
    {
        viable = false;
        roll = Random.Range(1, 11);
        if (roll == 10)
        {
            // Epic rarity
            do
            {
                roll = Random.Range(53, 61);
                if (roll > 58)
                {
                    if (map.Class == 'w')
                        roll += 2;
                    else if (map.Class == 'n')
                        roll += 4;
                    else if (map.Class == 'b')
                        roll += 6;
                }
                if (map.items.collected[roll] != true)
                    viable = true;
            } while (viable == false);
            artefacts[roll].SetActive(true);

            viable = false;

            do
            {
                roll2 = Random.Range(53, 61);
                if (roll2 > 58)
                {
                    if (map.Class == 'w')
                        roll2 += 2;
                    else if (map.Class == 'n')
                        roll2 += 4;
                    else if (map.Class == 'b')
                        roll2 += 6;
                }
                if (map.items.collected[roll2] != true && roll2 != roll)
                    viable = true;
            } while (viable == false);
            artefacts2[roll2].SetActive(true);
        }
        else if (roll > 6)
        {
            // Rare rarity
            do
            {
                roll = Random.Range(32, 44);
                if (roll > 40)
                {
                    if (map.Class == 'w')
                        roll += 3;
                    else if (map.Class == 'n')
                        roll += 6;
                    else if (map.Class == 'b')
                        roll += 9;
                }
                if (map.items.collected[roll] != true)
                    viable = true;
            } while (viable == false);
            artefacts[roll].SetActive(true);

            viable = false;

            do
            {
                roll2 = Random.Range(32, 44);
                if (roll2 > 40)
                {
                    if (map.Class == 'w')
                        roll2 += 3;
                    else if (map.Class == 'n')
                        roll2 += 6;
                    else if (map.Class == 'b')
                        roll2 += 9;
                }
                if (map.items.collected[roll2] != true && roll2 != roll)
                    viable = true;
            } while (viable == false);
            artefacts2[roll2].SetActive(true);
        }
        else
        {
            // Common rarity
            do
            {
                roll = Random.Range(0, 20);
                if (roll > 15)
                {
                    if (map.Class == 'w')
                        roll += 4;
                    else if (map.Class == 'n')
                        roll += 8;
                    else if (map.Class == 'b')
                        roll += 12;
                }
                if (map.items.collected[roll] != true)
                    viable = true;
            } while (viable == false);
            artefacts[roll].SetActive(true);

            viable = false;

            do
            {
                roll2 = Random.Range(0, 20);
                if (roll2 > 15)
                {
                    if (map.Class == 'w')
                        roll2 += 4;
                    else if (map.Class == 'n')
                        roll2 += 8;
                    else if (map.Class == 'b')
                        roll2 += 12;
                }
                if (map.items.collected[roll2] != true && roll2 != roll)
                    viable = true;
            } while (viable == false);
            artefacts2[roll2].SetActive(true);
        }
    }

    public void Leave()
    {
        artefacts[roll].SetActive(false);
        artefacts2[roll2].SetActive(false);
        Hud.SetActive(false);
    }

    public void Take(int which)
    {
        if (which == 1)
            map.items.Take(roll);
        else map.items.Take(roll2);
        Leave();
    }
}