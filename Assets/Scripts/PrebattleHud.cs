using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrebattleHud : MonoBehaviour
{
    public AllStats stats;

    public Map map;
    public PostbattleHud postbattle_hud;
    public ArtefactHud artefact_hud;
    public GameObject PreBattle, PostBattle, Artefact;

    public string[] enemy_type;
    public int[] enemy_count, enemy_size, enemy_part;
    public int roll;
    public int warband_size;
    private float max_warband_size;

    public Image[] enemy_im;
    public Sprite[] sprites;
    public TMPro.TextMeshProUGUI army_count;
    public bool relic = false;

    public void Pop(char type)
    {
        warband_size = 0;
        SetEnemyWarband(type);
        army_count.text = "~" + (warband_size + Random.Range(-3, 4)).ToString("0");
        if (type == 'e')
            relic = true;
        else relic = false;
    }

    public void PopT()
    {
        do
        {
            roll = Random.Range(0, 6);
        } while (map.threat_eliminated[roll] == true);

        for (int i = 1; i < 5; i++)
        {
            enemy_count[i] = 0;
            enemy_size[i] = 0;
            enemy_im[i].enabled = false;
        }

        switch (roll)
        {
            case 0:
                enemy_type[0] = "lord_of_torment";
                enemy_im[0].sprite = sprites[14];
                break;
            case 1:
                enemy_type[0] = "ratking";
                enemy_im[0].sprite = sprites[15];
                break;
            case 2:
                enemy_type[0] = "glorious_creation";
                enemy_im[0].sprite = sprites[16];
                for (int i = 3; i < 5; i++)
                {
                    enemy_type[i] = "energy_core";
                    enemy_count[i] = 1;
                    enemy_size[i] = 5 + map.floor;
                }
                break;
            case 3:
                enemy_type[0] = "corrupted_treant";
                enemy_im[0].sprite = sprites[17];
                break;
            case 4:
                enemy_type[0] = "tar_lord";
                enemy_im[0].sprite = sprites[18];
                break;
            case 5:
                enemy_type[0] = "bog-thing";
                enemy_im[0].sprite = sprites[19];
                for (int i = 1; i < 5; i++)
                {
                    enemy_type[i] = "mud_pile";
                    enemy_count[i] = 1;
                    enemy_size[i] = 2;
                }
                break;
        }

        enemy_count[0] = 1;
        enemy_size[0] = 20 + 2 * map.floor + map.floor * (map.floor + 1) / 2;
        army_count.text = "only one...";
        warband_size = enemy_size[0];
    }

    public void SetEnemyWarband(char type)
    {
        if (type == 'n')
        {
            for (int i = 0; i < 5; i++)
            {
                SetEnemy(i);
                enemy_count[i] = 1;
                warband_size += enemy_size[i];
                enemy_part[i] = 0;
            }

            roll = Random.Range(0, 3);
            max_warband_size = roll + map.danger_level * Random.Range(1f, 1.13f);
        }

        else if (type == 'e')
        {
            SetElite();

            for (int i = 1; i < 5; i++)
            {
                SetEnemy(i);
                enemy_count[i] = 1;
                warband_size += enemy_size[i];
                enemy_part[i] = 0;
            }

            roll = Random.Range(2, 6);
            max_warband_size = roll + map.danger_level * Random.Range(1.04f, 1.19f);
        }

        while (max_warband_size > warband_size)
        {
            roll = Random.Range(0, 5);
            enemy_part[roll]++;
            if (enemy_part[roll] >= enemy_size[roll])
            {
                enemy_part[roll] -= enemy_size[roll];
                enemy_count[roll]++;
                warband_size += enemy_size[roll];
            }
        }
    }

    void SetEnemy(int which)
    {
        enemy_im[which].enabled = true;
        if (which < 3)
        {
            roll = Random.Range(0, 5);
            switch (roll)
            {
                case 0:
                    enemy_type[which] = "reanimated_soldier";
                    enemy_im[which].sprite = sprites[0];
                    enemy_size[which] = 1;
                    break;
                case 1:
                    enemy_type[which] = "rotfiend";
                    enemy_im[which].sprite = sprites[5];
                    enemy_size[which] = 1;
                    break;
                case 2:
                    enemy_type[which] = "ghoul";
                    enemy_im[which].sprite = sprites[7];
                    enemy_size[which] = 1;
                    break;
                case 3:
                    enemy_type[which] = "living_corpse";
                    enemy_im[which].sprite = sprites[9];
                    enemy_size[which] = 1;
                    break;
                case 4:
                    enemy_type[which] = "dreadnought";
                    enemy_im[which].sprite = sprites[12];
                    enemy_size[which] = 4;
                    break;
            }
        }
        else
        {
            roll = Random.Range(0, 4);
            switch (roll)
            {
                case 0:
                    enemy_type[which] = "reanimated_jailer";
                    enemy_im[which].sprite = sprites[1];
                    enemy_size[which] = 1;
                    break;
                case 1:
                    enemy_type[which] = "soul_collector";
                    enemy_im[which].sprite = sprites[3];
                    enemy_size[which] = 2;
                    break;
                case 2:
                    enemy_type[which] = "husk";
                    enemy_im[which].sprite = sprites[6];
                    enemy_size[which] = 1;
                    break;
                case 3:
                    enemy_type[which] = "mad_scientist";
                    enemy_im[which].sprite = sprites[10];
                    enemy_size[which] = 1;
                    break;
            }
        }
    }

    void SetElite()
    {
        roll = Random.Range(0, 3);
        switch (roll)
        {
            case 0:
                enemy_type[0] = "revenant_legionary";
                enemy_im[0].sprite = sprites[4];
                enemy_size[0] = 6;
                break;
            case 1:
                enemy_type[0] = "shambler";
                enemy_im[0].sprite = sprites[8];
                enemy_size[0] = 5;
                break;
            case 2:
                enemy_type[0] = "abomination";
                enemy_im[0].sprite = sprites[13];
                enemy_size[0] = 7;
                break;
        }

        enemy_count[0] = 1;
        warband_size += enemy_size[0];
        enemy_part[0] = Mathf.RoundToInt(enemy_size[0] / 2.8f);
    }

    public void Fight()
    {
        map.map_canvas.SetActive(false);
        map.battle_canvas.SetActive(true);
        stats.Set();
    }

    public void BattleWon()
    {
        map.map_canvas.SetActive(true);
        map.battle_canvas.SetActive(false);

        map.enemies_defeated += warband_size;

        if (relic == true)
        {
            Artefact.SetActive(true);
            artefact_hud.Pop();
        }
           
        PostBattle.SetActive(true);
        postbattle_hud.Pop(warband_size);
        PreBattle.SetActive(false);
    }

    public void ThreatDefeated()
    {
        map.battle_canvas.SetActive(false);
        map.map_canvas.SetActive(true);

        map.ThreatEliminated();
    }
}