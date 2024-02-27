using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostbattleHud : MonoBehaviour
{
    public Map map;
    public AllStats stats;
    public TMPro.TextMeshProUGUI silver_count, arcane_shards_count, experience_count, condition_count, army_lost;
    public GameObject Hud;

    private int roll, footmen_lost, marksmen_lost, cavalry_lost, mages_lost, silver, arcane_shards, experience, condition;
    private float count;

    public void Pop(int enemy_warband)
    {
        count = ((10 + Mathf.Pow(enemy_warband, 1.1f)) * Random.Range(0.5f, 0.8f));
        if (map.items.collected[39] == true)
            silver = Mathf.RoundToInt(count * 1.1f);
        else  silver = Mathf.RoundToInt(count);
        arcane_shards = 0;

        while (count > 0)
        {
            arcane_shards++;
            count *= 0.85f;
            count -= Random.Range(3f, 11f);
        }

        count = enemy_warband * 20 + Mathf.Pow(enemy_warband, 1.6f);
        if (map.items.collected[15] == true)
            count *= 1.07f;
        if (stats.map.items.collected[66] == true)
        {
            count *= 1.05f + 0.003f * enemy_warband;
        }
        experience = Mathf.RoundToInt(count);

        if (stats.hero_alive == false)
            condition = 4;
        else if (stats.hero.hitPoints < stats.hero.HP * 0.4F)
            condition = 1;
        else condition = 0;
        condition_count.text = condition.ToString("");

        if (map.footmen > stats.unit1.number)
            footmen_lost = map.footmen - stats.unit1.number;
        else footmen_lost = 0;
        if (map.marksmen > stats.unit2.number)
            marksmen_lost = map.marksmen - stats.unit2.number;
        else marksmen_lost = 0;
        if (map.cavalry > stats.unit3.number)
            cavalry_lost = map.cavalry - stats.unit3.number;
        else cavalry_lost = 0;
        if (map.mages > stats.unit4.number)
            mages_lost = map.mages - stats.unit4.number;
        else mages_lost = 0;

        army_lost.text = "Army Lost: " + footmen_lost.ToString("") + " | " + marksmen_lost.ToString("") + " | " + cavalry_lost.ToString("") + " | " + mages_lost.ToString("");
        silver_count.text = silver.ToString("");
        arcane_shards_count.text = arcane_shards.ToString("");
        experience_count.text = experience.ToString("");
    }

    public void Proceed()
    {
        map.footmen -= footmen_lost; map.marksmen -= marksmen_lost; map.cavalry -= cavalry_lost; map.mages -= mages_lost;
        map.silver += silver;
        map.arcane_shards += arcane_shards;
        map.experience += experience;
        map.hero_condition += condition;
        map.UpdateCounters();

        map.free = true;
        map.danger_level += (0.55f + 0.12f * map.floor) * (1 + 0.001f * map.cleared); ;

        map.ClearTile(map.current);
        Hud.SetActive(false);
    }
}