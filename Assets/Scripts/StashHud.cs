using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StashHud : MonoBehaviour
{
    public Map map;
    public TMPro.TextMeshProUGUI silver_count, arcane_shards_count;
    public GameObject Stash;

    private int roll, silver, arcane_shards;

    public void Pop()
    {
        roll = Random.Range(1, 4);

        switch (roll)
        {
            case 1:
                silver = Random.Range(5, 16);
                arcane_shards = 1;
                break;
            case 2:
                silver = Random.Range(8, 24);
                arcane_shards = Random.Range(2, 6);
                break;
            case 3:
                silver = Random.Range(12, 34);
                arcane_shards = Random.Range(5, 10);
                break;
        }

        if (map.items.collected[39] == true)
            silver = Mathf.RoundToInt(silver * 1.1f);

        silver_count.text = silver.ToString("");
        arcane_shards_count.text = arcane_shards.ToString("");
    }

    public void PopT()
    {
        silver = Random.Range(14, 37);
        arcane_shards = Random.Range(6, 12);

        if (map.items.collected[39] == true)
            silver = Mathf.RoundToInt(silver * 1.1f);

        silver_count.text = silver.ToString("");
        arcane_shards_count.text = arcane_shards.ToString("");
    }

    public void Proceed()
    {
        map.silver += silver;
        map.arcane_shards += arcane_shards;
        map.UpdateCounters();

        map.free = true;

        map.ClearTile(map.current);
        Stash.SetActive(false);
    }
}