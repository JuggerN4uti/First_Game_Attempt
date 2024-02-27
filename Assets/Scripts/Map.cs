using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Map : MonoBehaviour
{
    public AllStats stats;
    public GameObject map_canvas, battle_canvas;
    public Tile[] tiles;
    public Sprite fight, elite, campfire, merchant, treasure, waygate, mystery, stash, GPlus, Plus;
    public Button threat;
    // done: Stash, Waygate, Fight, Elite, treasure, campfire
    // to add: merchant, mystery

    public GameObject Stash, Waygate, PreBattle, Artefact, Campfire, Merchant;
    public GameObject[] SkillTrees;
    public StashHud stash_hud;
    public WaygateHud waygate_hud;
    public ArtefactHud artefact_hud;
    public PrebattleHud prebattle_hud;
    public MerchantHud merchant_hud;

    public TMPro.TextMeshProUGUI silver_count, arcane_shards_count, condition_count, army_count, current_level;

    public ClearedBar clearedBar;
    public Bar experienceBar;
    public int cleared = 0, to_clear = 100;

    public bool free = true;
    private int roll;
    public float danger_level = 14f, warband_increase = 0f;
    public int silver, arcane_shards, hero_condition, current, max_warband, current_warband = 0, footmen, marksmen, cavalry, mages, lvl = 1, experience = 0, stat_poionts = 0, floor = 0, enemies_defeated = 0;
    public Image SkillTree;
    public Artefacts items;
    public float blood;
    public char Class;
    public bool[] threat_eliminated;

    void Start()
    {
        {
            lvl = 1;

            max_warband = PlayerPrefs.GetInt("maxUnits");
            footmen = PlayerPrefs.GetInt("unit1count");
            marksmen = PlayerPrefs.GetInt("unit2count");
            cavalry = PlayerPrefs.GetInt("unit3count");
            mages = PlayerPrefs.GetInt("unit4count");

            arcane_shards = PlayerPrefs.GetInt("bonusShards");
            experience = PlayerPrefs.GetInt("bonusExperience");

            PlayerPrefs.SetInt("FirstBoss", 0);
            PlayerPrefs.SetInt("SecondBoss", 0);
            PlayerPrefs.SetInt("ThirdBoss", 0);

            experienceBar.SetMaxValue(255 + lvl * 145 + lvl * lvl);
            to_clear = 100;
            clearedBar.SetMaxValue(to_clear);
            Generate();
            switch (PlayerPrefs.GetString("HeroClass"))
            {
                case "Light":
                    Class = 'l';
                    SkillTrees[4] = SkillTrees[0];
                    break;
                case "Water":
                    Class = 'w';
                    SkillTrees[4] = SkillTrees[1];
                    break;
                case "Nature":
                    Class = 'n';
                    SkillTrees[4] = SkillTrees[2];
                    break;
                case "Blood":
                    Class = 'b';
                    SkillTrees[4] = SkillTrees[3];
                    blood = 0;
                    break;
            }
        }
        UpdateCounters();
    }

    public void UpdateCounters()
    {
        silver_count.text = silver.ToString("");
        arcane_shards_count.text = arcane_shards.ToString("");
        condition_count.text = hero_condition.ToString("");
        current_warband = footmen * PlayerPrefs.GetInt("unit1size") + marksmen * PlayerPrefs.GetInt("unit2size") + cavalry * PlayerPrefs.GetInt("unit3size") + mages * PlayerPrefs.GetInt("unit4size");
        army_count.text = footmen.ToString("") + "/" + marksmen.ToString("") + "/" + cavalry.ToString("") + "/" + mages.ToString("") + ", " + current_warband.ToString("") + "/" + max_warband.ToString("");
        while (experience >= 255 + lvl * 145 + lvl * lvl)
            LevelUp();
        experienceBar.SetValue(experience);
        current_level.text = lvl.ToString("0");

        if (stat_poionts > 0)
            SkillTree.sprite = Plus;
        else SkillTree.sprite = GPlus;
    }

    private void LevelUp()
    {
        experience -= 255 + lvl * 145 + lvl * lvl;
        lvl++;
        experienceBar.SetMaxValue(255 + lvl * 145 + lvl * lvl);
        stat_poionts++;

        warband_increase += (0.1f + 0.006F * PlayerPrefs.GetInt("maxUnits")) * (1f + 0.02F * lvl);
        if (warband_increase >= 1f)
        {
            warband_increase -= 1f;
            max_warband++;
        }

        if (items.collected[36] == true)
        {
            if (lvl % 5 == 0)
                stat_poionts++;
        }
        UpdateCounters();
    }

    public void OpenSkillTree()
    {
        SkillTrees[4].SetActive(true);
    }

    void Generate()
    {
        for (int i = 0; i < 2; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = treasure;
            tiles[roll].type = "treasure";
        }

        for (int i = 0; i < 3; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = waygate;
            tiles[roll].type = "waygate";
        }
        
        for (int i = 0; i < 3; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = campfire;
            tiles[roll].type = "campfire";
        }

        for (int i = 0; i < 3; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = merchant;
            tiles[roll].type = "merchant";
        }

        for (int i = 0; i < 5; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = elite;
            tiles[roll].type = "elite";
        }

        for (int i = 0; i < 13 - floor; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = stash;
            tiles[roll].type = "stash";
        }

        /*for (int i = 0; i < 23; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = mystery;
            tiles[roll].type = "mystery";
        }*/

        for (int i = 0; i < 27 + floor * 2; i++)
        {
            do
            {
                roll = Random.Range(0, 75);
            } while (tiles[roll].type != "none");
            tiles[roll].icon.sprite = fight;
            tiles[roll].type = "fight";
        }

        for (int i = 0; i < 75; i++)
        {
            tiles[i].number = i;
            if (tiles[i].type == "none")
            {
                if (Random.Range(1, 6) == 5)
                {
                    ClearFog(i);
                    ClearTile(i);
                }
            }
        }
    }

    public void ClearFog(int which)
    {
        tiles[which].fog.enabled = false;
        Clear();
    }

    public void ClearTile(int which)
    {
        tiles[which].button.SetActive(false);
        Clear();
    }

    void Clear()
    {
        cleared++;
        clearedBar.SetValue(cleared);
        danger_level += (0.05f + 0.01f * floor) * (1 + 0.002f * cleared);
        if (cleared >= to_clear)
            threat.interactable = true;
        else
            threat.interactable = false;
    }

    public void StashClicked()
    {
        Stash.SetActive(true);
        stash_hud.Pop();
    }

    public void WaygateClicked()
    {
        Waygate.SetActive(true);
        waygate_hud.Pop();
    }

    public void FightClicked()
    {
        PreBattle.SetActive(true);
        prebattle_hud.Pop('n');
    }

    public void EliteClicked()
    {
        PreBattle.SetActive(true);
        prebattle_hud.Pop('e');
    }

    public void TreasureClicked()
    {
        Artefact.SetActive(true);
        artefact_hud.Pop();
        Stash.SetActive(true);
        stash_hud.PopT();
    }

    public void CampfireClicked()
    {
        Campfire.SetActive(true);
    }

    public void MerchantClicked()
    {
        Merchant.SetActive(true);
        merchant_hud.Pop();
    }

    public void ThreatClicked()
    {
        free = false;
        PreBattle.SetActive(true);
        prebattle_hud.PopT();
    }

    public void ThreatEliminated()
    {
        floor++;
        switch (floor)
        {
            case 1:
                PlayerPrefs.SetInt("FirstBoss", prebattle_hud.roll + 1);
                break;
            case 2:
                PlayerPrefs.SetInt("SecondBoss", prebattle_hud.roll + 1);
                break;
            case 3:
                PlayerPrefs.SetInt("ThirdBoss", prebattle_hud.roll + 1);
                break;
        }
        if (floor == 3)
        {
            PlayerPrefs.SetString("EndScreenTitle", "Congratulations! You have Won.");
            EndScreen();
        }
        for (int i = 0; i < 75; i++)
        {
            tiles[i].button.SetActive(true);
            tiles[roll].type = "none";
        }
        threat_eliminated[prebattle_hud.roll] = true;
        cleared = 0;
        danger_level = 14 + 3.3f * floor + floor * (floor + 1) / 1.8f;
        silver += 50 + 50 * floor;
        arcane_shards += 5 + 7 * floor;
        hero_condition = 0;
        experience += 500 + 400 * floor + 300 * floor + floor * (floor + 1) / 2;
        footmen = stats.unit1.number;
        marksmen = stats.unit2.number;
        cavalry = stats.unit3.number;
        mages = stats.unit4.number;
        UpdateCounters();
        to_clear = 100 + 10 * floor;
        clearedBar.SetMaxValue(to_clear);
        Generate();
        free = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            cleared += 100;
            clearedBar.SetValue(cleared);
            if (cleared >= to_clear)
                threat.interactable = true;
            else
                threat.interactable = false;
        }
    }

    public void EndScreen()
    {
        PlayerPrefs.SetInt("PlayerLevel", lvl);
        PlayerPrefs.SetInt("EnemiesSlain", enemies_defeated);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
}