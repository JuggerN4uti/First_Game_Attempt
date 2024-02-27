using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefacts : MonoBehaviour
{
    public Map map;
    public bool[] collected;

    public void Take(int which)
    {
        collected[which] = true;
        if (which == 36)
        {
            for (int i = 5; i <= map.lvl; i += 5)
            {
                map.stat_poionts++;
            }
        }
        else if (which == 39)
        {
            map.silver += 60;
            map.UpdateCounters();
        }
    }
}