using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireHud : MonoBehaviour
{
    public Map map;
    public GameObject Campfire;

    public void Rest()
    {
        map.hero_condition -= 4 + map.hero_condition / 4;
        if (map.hero_condition < 0)
            map.hero_condition = 0;
        Proceed();
    }

    public void Train()
    {
        map.experience += 350 + map.lvl * 50;
        Proceed();
    }

    void Proceed()
    {
        map.UpdateCounters();
        Campfire.SetActive(false);
        map.free = true;
        map.ClearTile(map.current);
    }
}