using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllStats : MonoBehaviour
{
    public Map map;
    public GameObject map_canvas, battle_canvas;
    public PrebattleHud hud;
    public SkillTree[] Tree;

    public HeroStats hero;
    public Unit1Stats unit1;
    public Unit2Stats unit2;
    public Unit3Stats unit3;
    public Unit4Stats unit4;
    public Enemy1Stats[] enemy;

    public GameObject[] enemy_obj;
    public Image pass1, pass2;
    public Button_Image butt0, butt1, butt2, butt3;
    public Ability_Info abi1, abi2, abi3;
    public Slider turn_fill;
    public TMPro.TextMeshProUGUI turn_count;

    public char whichHero = 'l';
    public int whoseTurn = 0;
    public int whichAbility;
    public bool hero_alive = true, unit1_alive = true, unit2_alive = true, unit3_alive = true, unit4_alive = true;
    public bool[] enemy_alive;
    public bool frontLineA = true, frontLineE = true, backLineA = true, backLineE = true;
    public int allyArmy, enemyArmy;
    public int turnTime = 1;
    private float updateTick = 0;

    public int amount;
    public float amountf;
    public int chance;
    public string buff;
    public char ability_range, color;
    public bool viable;
    public int ally_attacks, enemy_attacks;

    public void Set()
    {
        switch (PlayerPrefs.GetString("HeroClass"))
        {
            case "Light":
                whichHero = 'l';
                break;
            case "Water":
                whichHero = 'w';
                break;
            case "Nature":
                whichHero = 'n';
                break;
            case "Blood":
                whichHero = 'b';
                break;
        }

        frontLineA = true; frontLineE = true; backLineA = true; backLineE = true;

        enemyArmy = hud.warband_size;

        for (int i = 0; i < 5; i++)
        {
            enemy_obj[i].SetActive(true);
            enemy[i].character = hud.enemy_type[i];
            enemy[i].number = hud.enemy_count[i];
            enemy_alive[i] = true;
            enemy[i].Set();
        }

        unit1.number = hud.map.footmen;
        unit2.number = hud.map.marksmen;
        unit3.number = hud.map.cavalry;
        unit4.number = hud.map.mages;

        allyArmy = unit1.number * PlayerPrefs.GetInt("unit1size") + unit2.number * PlayerPrefs.GetInt("unit2size") + unit3.number * PlayerPrefs.GetInt("unit3size") + unit4.number * PlayerPrefs.GetInt("unit4size");

        hero_alive = true; unit1_alive = true; unit2_alive = true; unit3_alive = true; unit4_alive = true;
        hero.Set();
        unit1.Set();
        unit2.Set();
        unit3.Set();
        unit4.Set();

        if (Tree[1].perk[18] > 0)
        {
            amount = Tree[1].perk[18];
            while (amount > 0)
            {
                chance = Random.Range(1, 6);
                switch (chance)
                {
                    case 1:
                        if (hero_alive == true)
                        {
                            hero.GainAgility(1);
                            amount--;
                        }
                        break;
                    case 2:
                        if (unit1_alive == true)
                        {
                            unit1.GainAgility(1);
                            amount--;
                        }
                        break;
                    case 3:
                        if (unit2_alive == true)
                        {
                            unit2.GainAgility(1);
                            amount--;
                        }
                        break;
                    case 4:
                        if (unit3_alive == true)
                        {
                            unit3.GainAgility(1);
                            amount--;
                        }
                        break;
                    case 5:
                        if (unit4_alive == true)
                        {
                            unit4.GainAgility(1);
                            amount--;
                        }
                        break;
                }
            }
        }
        if (map.items.collected[55] == true)
        {
            amount = 2;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    enemy[chance].GainStun();
                }
                amount--;
            }
        }

        updateTick = 0;
        turnTime = 0;
        turn_count.text = turnTime.ToString("0");

        play();
    }

    public void enable_ally()
    {
        if (hero_alive == true)
            hero.enable();
        if (unit1_alive == true)
            unit1.enable();
        if (unit2_alive == true)
            unit2.enable();
        if (unit3_alive == true)
            unit3.enable();
        if (unit4_alive == true)
            unit4.enable();
    }

    public void enable_front()
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemy_alive[i] == true)
                enemy[i].enable();
        }
    }

    public void enable_back()
    {
        for (int i = 3; i < 5; i++)
        {
            if (enemy_alive[i] == true)
                enemy[i].enable();
        }
    }

    public void disable_all()
    {
        if (hero_alive == true)
            hero.disable();
        if (unit1_alive == true)
            unit1.disable();
        if (unit2_alive == true)
            unit2.disable();
        if (unit3_alive == true)
            unit3.disable();
        if (unit4_alive == true)
            unit4.disable();

        for (int i = 0; i < 5; i++)
        {
            if (enemy_alive[i] == true)
                enemy[i].disable();
        }

        switch (whoseTurn)
        {
            case 1:
                if (hero_alive == true)
                    hero.field.blue();
                break;
            case 2:
                if (unit1_alive == true)
                    unit1.field.blue();
                break;
            case 3:
                if (unit2_alive == true)
                    unit2.field.blue();
                break;
            case 4:
                if (unit3_alive == true)
                    unit3.field.blue();
                break;
            case 5:
                if (unit4_alive == true)
                    unit4.field.blue();
                break;
        }

    }

    public void play()
    {
        whoseTurn = 0;
        pass1.enabled = false;
        pass2.enabled = false;
        abi1.Disactivate();
        abi2.Disactivate();
        abi3.Disactivate();
        butt0.Type = 'i';
        butt1.Type = 'i';
        butt2.Type = 'i';
        butt3.Type = 'i';
        whichAbility = 10;
        disable_all();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            for (int i = 0; i < 5; i++)
            {
                if (enemy_alive[i] == true)
                    enemy[i].TakeDamage(500, 'r');
            }
        }

        if (whoseTurn == 0)
        {
            updateTick += 300 * Time.deltaTime;
            if (updateTick >= 2000)
            {
                updateTick -= 2000;
                turnTime++;
                turn_count.text = turnTime.ToString("0");
                if (map.items.collected[61] == true)
                {
                    if (turnTime % 3 == 0)
                        Lighting();
                }

                if (hero_alive == true)
                    hero.NewTurn();
                if (unit1_alive == true)
                    unit1.NewTurn();
                if (unit2_alive == true)
                    unit2.NewTurn();
                if (unit3_alive == true)
                    unit3.NewTurn();
                if (unit4_alive == true)
                    unit4.NewTurn();

                for (int i = 0; i < 5; i++)
                {
                    if (enemy_alive[i] == true)
                        enemy[i].NewTurn();
                }

                if (Tree[0].perk[13] > 0)
                {
                    switch(EnemyTarget('s'))
                    {
                        case 1:
                            hero.block++;
                            break;
                        case 2:
                            unit1.block++;
                            break;
                        case 3:
                            unit2.block++;
                            break;
                        case 4:
                            unit3.block++;
                            break;
                        case 5:
                            unit4.block++;
                            break;
                    }
                }
            
            }
            turn_fill.value = updateTick;
        }
        if (whoseTurn != 0)
        {
            pass1.enabled = true;
            pass2.enabled = true;
            butt0.gameObject.SetActive(true);
            butt1.gameObject.SetActive(true);
            butt1.Enable();
            butt2.gameObject.SetActive(true);
            butt2.Enable();
            butt3.gameObject.SetActive(true);
            butt3.Enable();
        }
        if (whoseTurn == 1)
        {
            if (whichHero == 'l')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 's';
                butt3.Type = 'b';
            }
            else if (whichHero == 'w')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'm';
                butt3.Type = 's';
            }
            else if (whichHero == 'n')
            {
                butt0.Type = 'm';
                butt1.Type = 's';
                butt2.Type = 'm';
                butt3.Type = 'b';
            }
            else if (whichHero == 'b')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'm';
                butt3.Type = 'b';
            }
            butt1.Set_text(hero.Cost_1 * (1 + (0.01F * hero.fatigue_1)), hero.fatigue_1);
            butt2.Set_text(hero.Cost_2 * (1 + (0.01F * hero.fatigue_2)), hero.fatigue_2);
            butt3.Set_text(hero.Cost_3 * (1 + (0.01F * hero.fatigue_3)), hero.fatigue_3);
            if (hero.Cost_1 * (1 + (0.01F * hero.fatigue_1)) > hero.mana)
                butt1.Disable();
            if (hero.Cost_2 * (1 + (0.01F * hero.fatigue_2)) > hero.mana)
                butt2.Disable();
            if (hero.Cost_3 * (1 + (0.01F * hero.fatigue_3)) > hero.mana)
                butt3.Disable();
        }
        if (whoseTurn == 2)
        {
            if (whichHero == 'l')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'b';
                butt3.Type = 'm';
            }
            if (whichHero == 'w')
            {
                butt0.Type = 'm';
                butt1.Type = 'r';
                butt2.Type = 'r';
                butt3.Type = 'm';
            }
            if (whichHero == 'n')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'b';
                butt3.Type = 'm';
            }
            if (whichHero == 'b')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'm';
                butt3.Type = 's';
            }
            butt1.Set_text(unit1.Cost_1 * (1 + (0.01F * unit1.fatigue_1)), unit1.fatigue_1);
            butt2.Set_text(unit1.Cost_2 * (1 + (0.01F * unit1.fatigue_2)), unit1.fatigue_2);
            butt3.Set_text(unit1.Cost_3 * (1 + (0.01F * unit1.fatigue_3)), unit1.fatigue_3);
            if (unit1.Cost_1 * (1 + (0.01F * unit1.fatigue_1)) > unit1.mana)
                butt1.Disable();
            if (unit1.Cost_2 * (1 + (0.01F * unit1.fatigue_2)) > unit1.mana)
                butt2.Disable();
            if (unit1.Cost_3 * (1 + (0.01F * unit1.fatigue_3)) > unit1.mana)
                butt3.Disable();
        }
        if (whoseTurn == 3)
        {
            if (whichHero == 'l')
            {
                butt0.Type = 'r';
                butt1.Type = 'r';
                butt2.Type = 'b';
                butt3.Type = 'r';
            }
            if (whichHero == 'w')
            {
                butt0.Type = 'r';
                butt1.Type = 'r';
                butt2.Type = 'r';
                butt3.Type = 'r';
            }
            if (whichHero == 'n')
            {
                butt0.Type = 'r';
                butt1.Type = 'r';
                butt2.Type = 'r';
                butt3.Type = 'r';
            }
            if (whichHero == 'b')
            {
                butt0.Type = 'r';
                butt1.Type = 'b';
                butt2.Type = 'r';
                butt3.Type = 's';
            }
            butt1.Set_text(unit2.Cost_1 * (1 + (0.01F * unit2.fatigue_1)), unit2.fatigue_1);
            butt2.Set_text(unit2.Cost_2 * (1 + (0.01F * unit2.fatigue_2)), unit2.fatigue_2);
            butt3.Set_text(unit2.Cost_3 * (1 + (0.01F * unit2.fatigue_3)), unit2.fatigue_3);
            if (unit2.Cost_1 * (1 + (0.01F * unit2.fatigue_1)) > unit2.mana)
                butt1.Disable();
            if (unit2.Cost_2 * (1 + (0.01F * unit2.fatigue_2)) > unit2.mana)
                butt2.Disable();
            if (unit2.Cost_3 * (1 + (0.01F * unit2.fatigue_3)) > unit2.mana)
                butt3.Disable();
        }
        if (whoseTurn == 4)
        {
            if (whichHero == 'l')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'b';
                butt3.Type = 's';
            }
            if (whichHero == 'w')
            {
                butt0.Type = 'm';
                butt1.Type = 'r';
                butt2.Type = 'b';
                butt3.Type = 'm';
            }
            if (whichHero == 'n')
            {
                butt0.Type = 'm';
                butt1.Type = 'r';
                butt2.Type = 'm';
                butt3.Type = 'm';
            }
            if (whichHero == 'b')
            {
                butt0.Type = 'm';
                butt1.Type = 'm';
                butt2.Type = 'm';
                butt3.Type = 's';
            }
            butt1.Set_text(unit3.Cost_1 * (1 + (0.01F * unit3.fatigue_1)), unit3.fatigue_1);
            butt2.Set_text(unit3.Cost_2 * (1 + (0.01F * unit3.fatigue_2)), unit3.fatigue_2);
            butt3.Set_text(unit3.Cost_3 * (1 + (0.01F * unit3.fatigue_3)), unit3.fatigue_3);
            if (unit3.Cost_1 * (1 + (0.01F * unit3.fatigue_1)) > unit3.mana)
                butt1.Disable();
            if (unit3.Cost_2 * (1 + (0.01F * unit3.fatigue_2)) > unit3.mana)
                butt2.Disable();
            if (unit3.Cost_3 * (1 + (0.01F * unit3.fatigue_3)) > unit3.mana)
                butt3.Disable();
        }
        if (whoseTurn == 5)
        {
            if (whichHero == 'l')
            {
                butt0.Type = 'r';
                butt1.Type = 's';
                butt2.Type = 'b';
                butt3.Type = 's';
            }
            if (whichHero == 'w')
            {
                butt0.Type = 'r';
                butt1.Type = 'b';
                butt2.Type = 's';
                butt3.Type = 's';
            }
            if (whichHero == 'n')
            {
                butt0.Type = 'r';
                butt1.Type = 's';
                butt2.Type = 'b';
                butt3.Type = 's';
            }
            if (whichHero == 'b')
            {
                butt0.Type = 'r';
                butt1.Type = 'b';
                butt2.Type = 's';
                butt3.Type = 's';
            }
            butt1.Set_text(unit4.Cost_1 * (1 + (0.01F * unit4.fatigue_1)), unit4.fatigue_1);
            butt2.Set_text(unit4.Cost_2 * (1 + (0.01F * unit4.fatigue_2)), unit4.fatigue_2);
            butt3.Set_text(unit4.Cost_3 * (1 + (0.01F * unit4.fatigue_3)), unit4.fatigue_3);
            if (unit4.Cost_1 * (1 + (0.01F * unit4.fatigue_1)) > unit4.mana)
                butt1.Disable();
            if (unit4.Cost_2 * (1 + (0.01F * unit4.fatigue_2)) > unit4.mana)
                butt2.Disable();
            if (unit4.Cost_3 * (1 + (0.01F * unit4.fatigue_3)) > unit4.mana)
                butt3.Disable();
        }
    }

    public void Attack_Button()
    {
        whichAbility = 0;
        disable_all();
        if (butt0.Type == 'm')
        {
            enable_front();
            if (frontLineE == false)
            {
                enable_back();
            }
        }
        else if (butt0.Type == 'r')
        {
            enable_front();
            enable_back();
        }
    }

    public void Spell1_Button()
    {
        disable_all();
        if ((whoseTurn == 3) && (whichHero == 'n'))
        {
            unit2.volley();
            amount = 3;
            while (amount > 0)
            {
                if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
                {
                    chance = Random.Range(0, 5);
                    if (enemy_alive[chance] == true)
                    {
                        enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit2.volley_dmg(), 1.0F, 'r'), 'r');
                        if (enemy_alive[chance] == true)
                            enemy[chance].GainPoison(unit2.poisoned_tips());
                        amount--;
                    }
                }
                else amount = 0;
            }
            play();
        }
        else if ((whoseTurn == 1) && (whichHero == 'b'))
        {
            hero.cleave();
            amountf = 0;
            if (frontLineE == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        amountf += enemy[i].DamageMultiplyer(hero.cleave_dmg(), hero.pen, 'm');
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(hero.cleave_dmg(), hero.pen, 'm'), 'r');
                    }
                }
            }
            else
            {
                for (int i = 3; i < 5; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        amountf += enemy[i].DamageMultiplyer(hero.cleave_dmg(), hero.pen, 'm');
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(hero.cleave_dmg(), hero.pen, 'm'), 'r');
                    }
                }
            }
            hero.blood_well(amountf * hero.blood_gain());
            play();
        }
        else if ((whoseTurn == 3) && (whichHero == 'b'))
        {
            unit2.blood_boil();
            play();
        }
        else if ((whoseTurn == 5) && (whichHero == 'b'))
        {
            unit4.bloodlust();
            if (hero_alive == true)
            {
                hero.GainRage(unit4.bloodlust_rage() * unit4.number);
                hero.GainIN(hero.IN * 0.002F * unit4.number);
            }
            if (unit1_alive == true)
            {
                unit1.GainRage(unit4.bloodlust_rage() * unit4.number);
                unit1.GainIN(unit1.IN * 0.002F * unit4.number);
            }
            if (unit2_alive == true)
            {
                unit2.GainRage(unit4.bloodlust_rage() * unit4.number);
                unit2.GainIN(unit2.IN * 0.002F * unit4.number);
            }
            if (unit3_alive == true)
            {
                unit3.GainRage(unit4.bloodlust_rage() * unit4.number);
                unit3.GainIN(unit3.IN * 0.002F * unit4.number);
            }
            play();
        }
        else
        {
            whichAbility = 1;
            if (butt1.Type == 'm')
            {
                enable_front();
                if (frontLineE == false)
                {
                    enable_back();
                }
            }
            else if ((butt1.Type == 'r') || (butt1.Type == 's'))
            {
                enable_front();
                enable_back();
            }
            else if (butt1.Type == 'b')
            {
                enable_ally();
            }
        }
    }

    public void Spell2_Button()
    {
        disable_all();
        if ((whoseTurn == 2) && (whichHero == 'l'))
        {
            unit1.fortify();
            if (hero_alive == true)
                hero.GainShield(unit1.fortify_ally());
            if (unit2_alive == true)
                unit2.GainShield(unit1.fortify_ally());
            if (unit3_alive == true)
                unit3.GainShield(unit1.fortify_ally());
            if (unit4_alive == true)
                unit4.GainShield(unit1.fortify_ally());
            play();
        }
        else if ((whoseTurn == 3) && (whichHero == 'l'))
        {
            unit2.empower();
            play();
        }
        else if ((whoseTurn == 4) && (whichHero == 'w'))
        {
            unit3.shells_up();
            play();
        }
        else if ((whoseTurn == 2) && (whichHero == 'n'))
        {
            unit1.meditate();
            play();
        }
        else if ((whoseTurn == 5) && (whichHero == 'n'))
        {
            unit4.innervate();
            play();
        }

        else
        {
            whichAbility = 2;
            if (butt2.Type == 'm')
            {
                enable_front();
                if (frontLineE == false)
                {
                    enable_back();
                }
            }
            else if ((butt2.Type == 'r') || (butt2.Type == 's'))
            {
                enable_front();
                enable_back();
            }
            else if (butt2.Type == 'b')
            {
                enable_ally();
                if (whoseTurn == 4)
                {
                    unit3.disable();
                    unit3.field.blue();
                }
            }
        }
    }

    public void Spell3_Button()
    {
        disable_all();
        if ((whoseTurn == 5) && (whichHero == 'w'))
        {
            unit4.tidal_surge();
            if (backLineE == true)
            {
                for (int i = 3; i < 5; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(unit4.tidal_surge_dmg(), 1.0F, 's'), 'p');
                        if (enemy_alive[i] == true)
                            enemy[i].LoseEnergy(unit4.tidal_surge_en());
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(unit4.tidal_surge_dmg(), 1.0F, 's'), 'p');
                        if (enemy_alive[i] == true)
                            enemy[i].LoseEnergy(unit4.tidal_surge_en());
                    }
                }
            }
            play();
        }
        else if ((whoseTurn == 1) && (whichHero == 'n'))
        {
            hero.savage_roar();
            if (unit1_alive == true)
                unit1.GainAD(unit1.AD * hero.savage_roar_ad());
            if (unit2_alive == true)
                unit2.GainAD(unit2.AD * hero.savage_roar_ad());
            if (unit3_alive == true)
                unit3.GainAD(unit3.AD * hero.savage_roar_ad());
            if (unit4_alive == true)
                unit4.GainAD(unit4.AD * hero.savage_roar_ad());
            play();
        }
        else if ((whoseTurn == 4) && (whichHero == 'n'))
        {
            unit3.stomp();
            amountf = 0;
            if (frontLineE == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        amountf += enemy[i].DamageMultiplyer(unit3.stomp_dmg(), 1.0F, 'm');
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(unit3.stomp_dmg(), 1.0F, 'm'), 'r');
                        if (enemy_alive[i] == true)
                            if (enemy[i].GainDebuff(unit3.stomp_stun()) == 1) enemy[i].GainStun();
                    }
                }
            }
            else
            {
                for (int i = 3; i < 5; i++)
                {
                    if (enemy_alive[i] == true)
                    {
                        amountf += enemy[i].DamageMultiplyer(unit3.stomp_dmg(), 1.0F, 'm');
                        enemy[i].TakeDamage(enemy[i].DamageMultiplyer(unit3.stomp_dmg(), 1.0F, 'm'), 'r');
                        if (enemy_alive[i] == true)
                            if (enemy[i].GainDebuff(unit3.stomp_stun()) == 1) enemy[i].GainStun();
                    }
                }
            }
            unit3.berserkers_rage(amountf * 0.25F / unit3.number);
            play();
        }
        else if ((whoseTurn == 1) && (whichHero == 'b'))
        {
            hero.bloodletting();
            play();
        }
        if ((whoseTurn == 3) && (whichHero == 'b'))
        {
            unit2.barrage();
            amount = 4;
            while (amount > 0)
            {
                if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
                {
                    chance = Random.Range(0, 5);
                    if (enemy_alive[chance] == true)
                    {
                        unit2.brutality(enemy[chance].DamageMultiplyer(unit2.barrage_dmg(), 0.75F, 's') * 0.03F / unit2.number);
                        enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit2.barrage_dmg(), 0.75F, 's'), 'p');
                        if (enemy_alive[chance] == true)
                            if (enemy[chance].GainDebuff(unit2.barrage_ar(enemy[chance].AR)) == 1) enemy[chance].LoseAR(1);
                        amount--;
                    }
                }
                else amount = 0;
            }
            play();
        }
        else
        {
            whichAbility = 3;
            if (butt3.Type == 'm')
            {
                enable_front();
                if (frontLineE == false)
                {
                    enable_back();
                }
            }
            else if ((butt3.Type == 'r') || (butt3.Type == 's'))
            {
                enable_front();
                enable_back();
            }
            else if (butt3.Type == 'b')
            {
                enable_ally();
                if (whoseTurn == 1)
                {
                    hero.disable();
                    hero.field.blue();
                }
            }
        }
    }

    public void AllyAction(int which)
    {
        switch (whichHero)
        {
            // Light ------------------------------------------------------------------------------------------------------------------------------
            case 'l':
                switch (whichAbility)
                {
                    case 0:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.attack_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(hero.crushing_blows_W()) == 1) enemy[which].GainWeakness(1);
                                    if (enemy[which].GainDebuff(hero.crushing_blows_V()) == 1) enemy[which].GainVoulnerable(1);
                                }
                                break;
                            case 2:
                                unit1.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 3:
                                unit2.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r'), 'r');
                                break;
                            case 4:
                                unit3.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 5:
                                unit4.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.attack_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.inner_fire_dmg(), 1.0F, 's'), 'p');
                                break;
                        }
                        break;
                    case 1:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.crippling_strike();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.crippling_strike_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].GainBleed(hero.crippling_strike_bleed());
                                    if (enemy[which].GainDebuff(hero.crushing_blows_W()) == 1) enemy[which].GainWeakness(1);
                                    if (enemy[which].GainDebuff(hero.crushing_blows_V()) == 1) enemy[which].GainVoulnerable(1);
                                }
                                break;
                            case 2:
                                unit1.spear_thrust();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.spear_thrust_dmg(), 0.65F, 'm'), 'r');
                                break;
                            case 3:
                                unit2.bola_shot();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.bola_shot_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit2.bola_shot_S()) == 1) enemy[which].GainSlow(1);
                                    enemy[which].LoseEnergy(unit2.bola_shot_in());
                                }
                                break;
                            case 4:
                                unit3.judgement();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.judgement_dmg(enemy[which].attacked, enemy[which].total_attacks), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit3.judgement_V()) == 1) enemy[which].GainVoulnerable(1);
                                break;
                            case 5:
                                unit4.blinding_light();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.blinding_light_dmg(), 1.0F, 's'), 'p');
                                amount = 0;
                                if (enemy_alive[which] == true)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (enemy[which].GainDebuff(unit4.blinding_light_W()) == 1) amount++;
                                    }
                                }
                                if (amount > 0) enemy[which].GainWeakness(amount);
                                break;
                        }
                        break;
                    case 2:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.smite();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.smite_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(hero.smite_S()) == 1) enemy[which].GainSlow(1);
                                    if (enemy[which].strength > 0)
                                    {
                                        if (enemy[which].GainDebuff(hero.smite_D(enemy[which].strength)) == 1) enemy[which].GainWeakness(1);
                                    }
                                    if (enemy[which].resistance > 0)
                                    {
                                        if (enemy[which].GainDebuff(hero.smite_D(enemy[which].resistance)) == 1) enemy[which].GainVoulnerable(1);
                                    }
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 2:
                                unit1.shield_slam();
                                amountf = enemy[which].DamageMultiplyer(unit1.shield_slam_dmg(), 1.0F, 'p');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.shield_slam_dmg(), 1.0F, 'm'), 'p');
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit1.shield_slam_W(amountf)) == 1) enemy[which].GainWeakness(1);
                                break;
                            case 3:
                                unit2.holy_bolt();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.holy_bolt_dmg(), 1.0F, 's'), 'p');
                                break;
                            case 4:
                                unit3.penance();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.penance_dmg(), 1.0F, 's'), 'p');
                                break;
                            case 5:
                                unit4.chastise();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.chastise_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit4.chastise_stun()) == 1)
                                    {
                                        enemy[which].GainStun();
                                        enemy[which].GainVoulnerable(1);
                                    }
                                }
                                break;
                        }
                        break;
                }
                break;
            // Water ------------------------------------------------------------------------------------------------------------------------------------------------
            case 'w':
                switch (whichAbility)
                {
                    case 0:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.attack_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (Tree[1].perk[14] > 0)
                                        if (enemy[which].GainDebuff((65 + enemy[which].AR * 1.2f) * hero.size) == 1)
                                        {
                                            enemy[which].LoseAR(Tree[1].perk[14]);
                                            hero.GainAR(Tree[1].perk[14]);
                                        }
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.charged_strikes_dmg(), hero.charged_strikes_pen(), 's'), 'p');
                                }
                                break;
                            case 2:
                                unit1.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 0.85F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].GainBleed(unit1.serrated_blade_bleed(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 0.85F, 'r')));
                                break;
                            case 3:
                                unit2.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit2.shred(enemy[which].AR)) == 1) enemy[which].LoseAR(2);
                                break;
                            case 4:
                                unit3.attack();
                                amountf = enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm'), 'r');
                                unit3.revenge_gain(amountf, 0.26f / unit3.number);
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit3.staggering_blows_in(enemy[which].IN)) == 1) enemy[which].LoseIN(1);
                                    if (enemy[which].GainDebuff(unit3.staggering_blows_V()) == 1) enemy[which].GainVoulnerable(1);
                                }
                                break;
                            case 5:
                                unit4.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.attack_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (unit4.torrent_stacks >= 1)
                                    {
                                        enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.torrent(), 1.0F, 's'), 'p');
                                        Ebb_and_Flow();
                                        unit4.torrent_stacks--;
                                    }
                                }
                                Ebb_and_Flow();
                                break;
                        }
                        break;
                    case 1:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.impale();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.impale_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (Tree[1].perk[14] > 0)
                                        if (enemy[which].GainDebuff((65 + enemy[which].AR * 1.2f) * hero.size) == 1)
                                        {
                                            enemy[which].LoseAR(Tree[1].perk[14]);
                                            hero.GainAR(Tree[1].perk[14]);
                                        }
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.charged_strikes_dmg(), hero.charged_strikes_pen(), 's'), 'p');
                                    if (enemy_alive[which] == true)
                                    {
                                        if (enemy[which].GainDebuff(hero.impale_stun(enemy[which].DamageMultiplyer(hero.impale_dmg(), 1.0F, 'm'))) == 1) enemy[which].GainStun();
                                        else hero.GainEnergy(20 * hero.size);
                                    }
                                }
                                break;
                            case 2:
                                unit1.harpoon();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.harpoon_dmg(), 0.85F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].GainBleed(unit1.serrated_blade_bleed(enemy[which].DamageMultiplyer(unit1.harpoon_dmg(), 0.85F, 'r')));
                                    enemy[which].LoseEnergy(unit1.harpoon_en(enemy[which].agility));
                                }
                                break;
                            case 3:
                                unit2.deadly_swings();
                                while (unit2.deadly_swings_tick() == true && enemy_alive[which] == true)
                                {
                                    unit2.advance(1);
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.deadly_swings_dmg(), 1F, 'r'), 'r');
                                    if (enemy_alive[which] == true)
                                        if (enemy[which].GainDebuff(unit2.shred(enemy[which].AR)) == 1) enemy[which].LoseAR(2);
                                }
                                break;
                            case 4:
                                unit3.dredge_line();
                                amountf = enemy[which].DamageMultiplyer(unit3.dredge_line_dmg(), 1.0F, 'r');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.dredge_line_dmg(), 1.0F, 'r'), 'r');
                                unit3.revenge_gain(amountf, 0.26f / unit3.number);
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit3.staggering_blows_in(enemy[which].IN)) == 1) enemy[which].LoseIN(1);
                                    if (enemy[which].GainDebuff(unit3.staggering_blows_V()) == 1) enemy[which].GainVoulnerable(1);
                                    if (enemy[which].GainDebuff(unit3.dredge_line_stun()) == 1)
                                    {
                                        enemy[which].GainStun();
                                        enemy[which].LoseEnergy(15 * enemy[which].number * enemy[which].size);
                                        unit3.GainEnergy(15 * unit3.number * unit3.size);
                                    }
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.rupture();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.rupture_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (Tree[1].perk[14] > 0)
                                        if (enemy[which].GainDebuff((65 + enemy[which].AR * 1.2f) * hero.size) == 1)
                                        {
                                            enemy[which].LoseAR(Tree[1].perk[14]);
                                            hero.GainAR(Tree[1].perk[14]);
                                        }
                                    enemy[which].GainBleed(hero.rupture_bleed());
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.rupture_m_dmg(enemy[which].bleed), 1.0F, 's'), 'p');
                                    if (enemy_alive[which] == true)
                                        enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.charged_strikes_dmg(), hero.charged_strikes_pen(), 's'), 'p');
                                }
                                break;
                            case 2:
                                unit1.ensnare();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.ensnare_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit1.ensnare_S()) == 1)
                                    {
                                        enemy[which].GainStun();
                                        enemy[which].GainSlow(1);
                                    }
                                }
                                break;
                            case 3:
                                unit2.quick_cut();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.quick_cut_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].GainBleed(unit2.quick_cut_bleed());
                                    if (enemy[which].GainDebuff(unit2.shred(enemy[which].AR)) == 1) enemy[which].LoseAR(2);
                                }
                                break;
                            case 5:
                                unit4.riptide();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.riptide_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit4.riptide_S()) == 1) enemy[which].GainSlow(1);
                                    if (enemy[which].GainDebuff(unit4.riptide_S()) == 1) enemy[which].GainFreeze(1);
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.deep_freeze();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.deep_freeze_dmg(), 1.0F, 's'), 'p');
                                amount = 0;
                                if (enemy_alive[which] == true)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (enemy[which].GainDebuff(hero.deep_freeze_F()) == 1) amount++;
                                    }
                                    if (amount > 0) enemy[which].GainFreeze(amount);
                                    if (enemy[which].GainDebuff(hero.deep_freeze_F()) == 1) enemy[which].GainSlow(1);
                                }
                                break;
                            case 2:
                                unit1.vicious_slash();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.vicious_slash_dmg(), 0.85F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].GainBleed(unit1.vicious_slash_bleed(enemy[which].DamageMultiplyer(unit1.vicious_slash_dmg(), 0.85F, 'r')));
                                break;
                            case 3:
                                unit2.tight_grip();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.tight_grip_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit2.shred(enemy[which].AR)) == 1) enemy[which].LoseAR(2);
                                    enemy[which].LoseEnergy(unit2.tight_grip_en(enemy[which].DamageMultiplyer(unit2.tight_grip_dmg(), 1.0F, 'r')));
                                }
                                break;
                            case 4:
                                unit3.revenge();
                                amountf = enemy[which].DamageMultiplyer(unit3.revenge_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.revenge_dmg(), 1.0F, 'm'), 'r');
                                unit3.revenge_gain(amountf, 0.26f / unit3.number);
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.revenge_m(), 1.0F, 's'), 'p');
                                    if (enemy_alive[which] == true)
                                    {
                                        if (enemy[which].GainDebuff(unit3.staggering_blows_in(enemy[which].IN)) == 1) enemy[which].LoseIN(1);
                                        if (enemy[which].GainDebuff(unit3.staggering_blows_V()) == 1) enemy[which].GainVoulnerable(1);
                                        if (enemy[which].GainDebuff(unit3.revenge_F()) == 1) enemy[which].GainFreeze(1);
                                    }
                                }
                                unit3.revenge_b();
                                break;
                        }
                        break;
                }
                break;
            // Nature ------------------------------------------------------------------------------------------------------------------------------------------------
            case 'n':
                switch (whichAbility)
                {
                    case 0:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.attack();
                                if (hero.beast_within_charge > 0)
                                {
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.beast_within_dmg(), 1.0F, 'm'), 'r');
                                    if (Tree[2].perk[20] > 0)
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            if (enemy_alive[i] == true && i != which)
                                                enemy[i].TakeDamage(enemy[i].DamageMultiplyer(hero.beast_within_dmg() * 0.28f, 1.0F, 'm'), 'r');
                                        }
                                    }
                                    if (map.items.collected[48] == true)
                                        hero.advance(1);
                                    hero.IN += 0.12F * Tree[2].perk[16];
                                    hero.beast_within_charge--;
                                }
                                else
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.attack_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 2:
                                unit1.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 3:
                                unit2.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].GainPoison(unit2.poisoned_tips());
                                break;
                            case 4:
                                unit3.attack();
                                amountf = enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm'), 'r');
                                unit3.berserkers_rage(amountf * 0.25F / unit3.number);
                                break;
                            case 5:
                                unit4.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.attack_dmg(), 1.0F, 'r'), 'r');
                                break;
                        }
                        break;
                    case 1:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.entangling_roots();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.entangling_roots_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(hero.entangling_roots_S()) == 1) enemy[which].GainStun();
                                    if (enemy[which].GainDebuff(hero.entangling_roots_S()) == 1) enemy[which].GainWeakness(1);
                                }
                                break;
                            case 2:
                                unit1.twin_slice();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.twin_slice_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.twin_slice_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 4:
                                unit3.axe_throw();
                                amountf = enemy[which].DamageMultiplyer(unit3.axe_throw_dmg(), 1.0F, 'r');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.axe_throw_dmg(), 1.0F, 'r'), 'r');
                                unit3.berserkers_rage(amountf * 0.25F / unit3.number);
                                break;
                            case 5:
                                unit4.poison_seeds();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.poison_seeds_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].GainPoison(unit4.poison_seeds_poison());
                                    enemy[which].LoseEnergy(unit4.poison_seeds_en());
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.feral_rage();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.feral_rage_dmg(), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(hero.feral_rage_W(enemy[which].DamageMultiplyer(hero.feral_rage_dmg(), 1.0F, 'm'))) == 1) enemy[which].GainWeakness(1);
                                break;
                            case 3:
                                unit2.snipe();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.snipe_dmg(enemy[which].position), 0.8F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].GainPoison(unit2.poisoned_tips());
                                break;
                            case 4:
                                unit3.reckless_swing();
                                amountf = enemy[which].DamageMultiplyer(unit3.reckless_swing_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.reckless_swing_dmg(), 1.0F, 'm'), 'r');
                                unit3.berserkers_rage(amountf * 0.25F / unit3.number);
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 2:
                                unit1.clean_cut();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.clean_cut_dmg(), unit1.clean_cut_pen(), 'm'), 'r');
                                break;
                            case 3:
                                unit2.pin_down();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.pin_down_dmg(), 1.0F, 'r'), 'r');
                                if (enemy_alive[which] == true)
                                {
                                    enemy[which].GainPoison(unit2.pin_down_poison());
                                    enemy[which].LoseEnergy(unit2.pin_down_en());
                                }
                                break;
                            case 5:
                                unit4.bogbeam();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.bogbeam_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit4.bogbeam_W()) == 1) enemy[which].GainWeakness(1);
                                    if (enemy[which].GainDebuff(unit4.bogbeam_W()) == 1) enemy[which].GainSlow(1);
                                }
                                break;
                        }
                        break;
                }
                break;
            // Blood ------------------------------------------------------------------------------------------------------------------------------------------------
            case 'b':
                switch (whichAbility)
                {
                    case 0:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.attack();
                                amountf = enemy[which].DamageMultiplyer(hero.attack_dmg(), hero.pen, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.attack_dmg(), hero.pen, 'm'), 'r');
                                hero.blood_well(amountf * hero.blood_gain());
                                break;
                            case 2:
                                unit1.attack();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.attack_dmg(), 1.0F, 'm'), 'r');
                                break;
                            case 3:
                                unit2.attack();
                                unit2.brutality(enemy[which].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r') * 0.03F / unit2.number);
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r'), 'r');
                                break;
                            case 4:
                                unit3.attack();
                                amountf = enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm'), 'r');
                                unit3.wrath(amountf * 0.27F / unit3.number);
                                break;
                            case 5:
                                unit4.attack();
                                if (unit4.spear_stacks > 0)
                                {
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.spear_attack_dmg(), 0.7F, 'r'), 'r');
                                    unit4.spear_stacks--;
                                }
                                else enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.attack_dmg(), 1.0F, 'r'), 'r');
                                break;
                        }
                        break;
                    case 1:
                        switch (whoseTurn)
                        {
                            case 2:
                                unit1.carve();
                                amountf = enemy[which].DamageMultiplyer(unit1.carve_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.carve_dmg(), 1.0F, 'm'), 'r');
                                unit1.carve_hp(amountf * 0.02f);
                                break;
                            case 4:
                                unit3.decimate();
                                amountf = enemy[which].DamageMultiplyer(unit3.decimate_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.decimate_dmg(), 1.0F, 'm'), 'r');
                                unit3.wrath(amountf * 0.27F / unit3.number);
                                break;
                        }
                        break;
                    case 2:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.drain();
                                amountf = enemy[which].DamageMultiplyer(hero.drain_dmg(), hero.drain_pen(), 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(hero.drain_dmg(), hero.drain_pen(), 'm'), 'r');
                                hero.drain_res(amountf);
                                hero.blood_well(amountf * hero.blood_gain());
                                break;
                            case 2:
                                unit1.execute();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.execute_dmg(enemy[which].resistance), 1.0F, 'm'), 'r');
                                if (enemy_alive[which] == true)
                                    enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit1.execute_second_hit_dmg(enemy[which].DamageMultiplyer(unit1.execute_dmg(enemy[which].resistance), 1.0F, 'm'), enemy[which].resistance), 1.0F, 'm'), 'r');
                                break;
                            case 3:
                                unit2.rampage();
                                unit2.brutality(enemy[which].DamageMultiplyer(unit2.rampage_dmg(), 1.0F, 'r') * 0.03F / unit2.number);
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit2.rampage_dmg(), 1.0F, 'r'), 'r');
                                unit2.rampage_stacks++;
                                break;
                            case 4:
                                unit3.trample();
                                amountf = enemy[which].DamageMultiplyer(unit3.trample_dmg(), 1.0F, 'm');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.trample_dmg(), 1.0F, 'm'), 'r');
                                unit3.wrath(amountf * 0.27F / unit3.number);
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit3.trample_stun()) == 1) enemy[which].GainStun();
                                break;
                            case 5:
                                unit4.siphon_life();
                                unit4.siphon_life_res(enemy[which].DamageMultiplyer(unit4.siphon_life_dmg(), 1.0F, 's'));
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.siphon_life_dmg(), 1.0F, 's'), 'p');
                                unit4.GainStrength(1);
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit4.siphon_life_W()) == 1) enemy[which].GainWeakness(1);
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 2:
                                unit1.taunt(enemy[which].AD, enemy[which].strength, enemy[which].number);
                                enemy[which].GainWeakness(1);
                                enemy[which].GainVoulnerable(1);
                                break;
                            case 4:
                                unit3.roar();
                                amountf = enemy[which].DamageMultiplyer(unit3.roar_dmg(), 0F, 's');
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit3.roar_dmg(), 1.0F, 's'), 'p');
                                unit3.wrath(amountf * 0.27F / unit3.number);
                                if (enemy_alive[which] == true)
                                {
                                    if (enemy[which].GainDebuff(unit3.roar_deb()) == 1) enemy[which].GainWeakness(1);
                                    if (enemy[which].GainDebuff(unit3.roar_deb()) == 1) enemy[which].GainVoulnerable(1);
                                    if (enemy[which].GainDebuff(unit3.roar_deb()) == 1) enemy[which].GainSlow(1);
                                }
                                break;
                            case 5:
                                unit4.blood_blast();
                                enemy[which].TakeDamage(enemy[which].DamageMultiplyer(unit4.blood_blast_dmg(), 1.0F, 's'), 'p');
                                if (enemy_alive[which] == true)
                                    if (enemy[which].GainDebuff(unit4.blood_blast_V()) == 1) enemy[which].GainVoulnerable(1);
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void Ebb_and_Flow()
    {
        amount = 1;
        while (amount > 0)
        {
            chance = Random.Range(1, 6);
            switch (chance)
            {
                case 1:
                    if (hero_alive == true)
                    {
                        hero.GainEnergy(unit4.ebb_and_flow_en());
                        if (hero.GainBuff(unit4.ebb_and_flow_in()) == 1)
                        {
                            hero.GainIN(0.3f);
                            hero.GainHaste(3 * unit4.number);
                        }
                        amount--;
                    }
                    break;
                case 2:
                    if (unit1_alive == true)
                    {
                        unit1.GainEnergy(unit4.ebb_and_flow_en());
                        if (unit1.GainBuff(unit4.ebb_and_flow_in()) == 1)
                        {
                            unit1.GainIN(0.3f);
                            unit1.GainHaste(3 * unit4.number);
                        }
                        amount--;
                    }
                    break;
                case 3:
                    if (unit2_alive == true)
                    {
                        unit2.GainEnergy(unit4.ebb_and_flow_en());
                        if (unit2.GainBuff(unit4.ebb_and_flow_in()) == 1)
                        {
                            unit2.GainIN(0.3f);
                            unit2.GainHaste(3 * unit4.number);
                        }
                        amount--;
                    }
                    break;
                case 4:
                    if (unit3_alive == true)
                    {
                        unit3.GainEnergy(unit4.ebb_and_flow_en());
                        if (unit3.GainBuff(unit4.ebb_and_flow_in()) == 1)
                        {
                            unit3.GainIN(0.3f);
                            unit3.GainHaste(3 * unit4.number);
                        }
                        amount--;
                    }
                    break;
                case 5:
                    if (unit4_alive == true)
                    {
                        unit4.GainEnergy(unit4.ebb_and_flow_en());
                        unit4.GainIN(0.3f);
                        unit4.GainHaste(3 * unit4.number);
                        amount--;
                    }
                    break;
            }
        }
    }

    public void Hero_Button()
    {
        switch (whichHero)
        {
            case 'l':
                switch (whichAbility)
                {
                    case 2:
                        switch (whoseTurn)
                        {
                            case 4:
                                unit3.inspire();
                                if (hero.GainBuff(unit3.inspire_S()) == 1)
                                {
                                    hero.GainStrength(1);
                                    hero.GainValor(4 * unit3.number);
                                }
                                if (hero.GainBuff(unit3.inspire_A()) == 1) hero.GainAgility(1);
                                hero.GainEnergy(unit3.inspire_en());
                                break;
                            case 5:
                                unit4.holy_light();
                                hero.RestoreHealth(unit4.holy_light_rest());
                                if (hero.GainBuff(unit4.holy_light_R()) == 1)
                                {
                                    hero.GainResistance(1);
                                    if (hero.resistance > 0)
                                        hero.GainValor((2 + 1 * hero.resistance) * unit4.number);
                                }
                                break;
                        }
                        break;
                }
                break;
            case 'w':
                switch (whichAbility)
                {
                    case 1:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.protective_bubble();
                                hero.GainShield(unit4.protective_bubble_sh());
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void Unit1_Button()
    {
        switch (whichHero)
        {
            case 'l':
                switch (whichAbility)
                {
                    case 2:
                        switch (whoseTurn)
                        {
                            case 4:
                                unit3.inspire();
                                if (unit1.GainBuff(unit3.inspire_S()) == 1)
                                {
                                    unit1.GainStrength(1);
                                    unit1.GainValor(4 * unit3.number);
                                }
                                if (unit1.GainBuff(unit3.inspire_A()) == 1) unit1.GainAgility(1);
                                unit1.GainEnergy(unit3.inspire_en());
                                break;
                            case 5:
                                unit4.holy_light();
                                unit1.RestoreHealth(unit4.holy_light_rest());
                                if (unit1.GainBuff(unit4.holy_light_R()) == 1)
                                {
                                    unit1.GainResistance(1);
                                    if (unit1.resistance > 0)
                                        unit1.GainValor((2 + 1 * unit1.resistance) * unit4.number);
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.bulwark_of_light();
                                unit1.GainShield(hero.bulwark_of_light_sh());
                                unit1.GainValor(Mathf.RoundToInt(16 + 0.09F * hero.bulwark_of_light_sh()));
                                break;
                        }
                        break;
                }
                break;
            case 'w':
                switch (whichAbility)
                {
                    case 1:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.protective_bubble();
                                unit1.GainShield(unit4.protective_bubble_sh());
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void Unit2_Button()
    {
        switch (whichHero)
        {
            case 'l':
                switch (whichAbility)
                {
                    case 2:
                        switch (whoseTurn)
                        {
                            case 4:
                                unit3.inspire();
                                if (unit2.GainBuff(unit3.inspire_S()) == 1)
                                {
                                    unit2.GainStrength(1);
                                    unit2.GainValor(4 * unit3.number);
                                }
                                if (unit2.GainBuff(unit3.inspire_A()) == 1) unit2.GainAgility(1);
                                unit2.GainEnergy(unit3.inspire_en());
                                break;
                            case 5:
                                unit4.holy_light();
                                unit2.RestoreHealth(unit4.holy_light_rest());
                                if (unit2.GainBuff(unit4.holy_light_R()) == 1)
                                {
                                    unit2.GainResistance(1);
                                    if (unit2.resistance > 0)
                                        unit2.GainValor((2 + 1 * unit2.resistance) * unit4.number);
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.bulwark_of_light();
                                unit2.GainShield(hero.bulwark_of_light_sh());
                                unit2.GainValor(Mathf.RoundToInt(16 + 0.09F * hero.bulwark_of_light_sh()));
                                break;
                        }
                        break;
                }
                break;
            case 'w':
                switch (whichAbility)
                {
                    case 1:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.protective_bubble();
                                unit2.GainShield(unit4.protective_bubble_sh());
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void Unit3_Button()
    {
        switch (whichHero)
        {
            case 'l':
                switch (whichAbility)
                {
                    case 2:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.holy_light();
                                unit3.RestoreHealth(unit4.holy_light_rest());
                                if (unit3.GainBuff(unit4.holy_light_R()) == 1)
                                {
                                    unit3.GainResistance(1);
                                    if (unit3.resistance > 0)
                                        unit3.GainValor((2 + 1 * unit3.resistance) * unit4.number);
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.bulwark_of_light();
                                unit3.GainShield(hero.bulwark_of_light_sh());
                                unit3.GainValor(Mathf.RoundToInt(16 + 0.09F * hero.bulwark_of_light_sh()));
                                break;
                        }
                        break;
                }
                break;
            case 'w':
                switch (whichAbility)
                {
                    case 1:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.protective_bubble();
                                unit3.GainShield(unit4.protective_bubble_sh());
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void Unit4_Button()
    {
        switch (whichHero)
        {
            case 'l':
                switch (whichAbility)
                {
                    case 2:
                        switch (whoseTurn)
                        {
                            case 4:
                                unit3.inspire();
                                if (unit4.GainBuff(unit3.inspire_S()) == 1)
                                {
                                    unit4.GainStrength(1);
                                    unit4.GainValor(4 * unit3.number);
                                }
                                if (unit4.GainBuff(unit3.inspire_A()) == 1) unit4.GainAgility(1);
                                unit4.GainEnergy(unit3.inspire_en());
                                break;
                            case 5:
                                unit4.holy_light();
                                unit4.RestoreHealth(unit4.holy_light_rest());
                                unit4.GainResistance(1);
                                if (unit4.resistance > 0)
                                    unit4.GainValor((2 + 1 * unit4.resistance) * unit4.number);
                                break;
                        }
                        break;
                    case 3:
                        switch (whoseTurn)
                        {
                            case 1:
                                hero.bulwark_of_light();
                                unit4.GainShield(hero.bulwark_of_light_sh());
                                unit4.GainValor(Mathf.RoundToInt(16 + 0.09F * hero.bulwark_of_light_sh()));
                                break;
                        }
                        break;
                }
                break;
            case 'w':
                switch (whichAbility)
                {
                    case 1:
                        switch (whoseTurn)
                        {
                            case 5:
                                unit4.protective_bubble();
                                unit4.GainShield(unit4.protective_bubble_sh());
                                break;
                        }
                        break;
                }
                break;
        }
        play();
    }

    public void EnemyTakenDown(int which)
    {
        enemy_alive[which] = false;

        if (enemy_alive[0] == false && enemy_alive[1] == false && enemy_alive[2] == false)
            frontLineE = false;

        if (enemy_alive[3] == false && enemy_alive[4] == false)
            backLineE = false;

        if (frontLineE == false && backLineE == false)
        {
            Invoke("BattleWon", 0.8f);
        }
    }

    private void BattleWon()
    {
        map.blood += (hero.blood_stacks - map.blood) * 0.05f;
        hud.BattleWon();
    }

    public void BossDefeated()
    {
        map.enemies_defeated += enemy[0].size;
        Invoke("ThreatDefeated", 1.2f);
    }

    private void ThreatDefeated()
    {
        map.blood += (hero.blood_stacks - map.blood) * 0.05f;
        hud.ThreatDefeated();
    }

    public void AllyTakenDown(char which)
    {
        switch (which)
        {
            case 'h':
                hero_alive = false;
                break;
            case '1':
                unit1_alive = false;
                break;
            case '2':
                unit2_alive = false;
                break;
            case '3':
                unit3_alive = false;
                break;
            case '4':
                unit4_alive = false;
                break;
        }

        if (hero_alive == false && unit1_alive == false && unit3_alive == false)
            frontLineA = false;

        if (unit2_alive == false && unit4_alive == false)
            backLineA = false;

        if (frontLineA == false && backLineA == false)
        {
            PlayerPrefs.SetString("EndScreenTitle", "My condolences... You have Lost.");
            map_canvas.SetActive(true);
            map.EndScreen();
        }
    }

    public int EnemyTarget(char type)
    {
        viable = false;
        while (viable == false)
        {
            if (frontLineA == false && backLineA == false)
                viable = false;
            chance = Random.Range(1, 6);
            switch (chance)
            {
                case 1:
                    if (hero_alive == true)
                        viable = true;
                    break;
                case 2:
                    if (unit1_alive == true)
                        viable = true;
                    break;
                case 3:
                    if (unit2_alive == true)
                    {
                        if ((type == 'm') && (frontLineA == true))
                            viable = false;
                        else viable = true;
                    }
                    break;
                case 4:
                    if (unit3_alive == true)
                        viable = true;
                    break;
                case 5:
                    if (unit4_alive == true)
                    {
                        if ((type == 'm') && (frontLineA == true))
                            viable = false;
                        else viable = true;
                    }
                    break;
            }
        }
        return chance;
    }

    public bool TargetAvailable(int target)
    {
        viable = false;

        switch (target)
        {
            case 1:
                if (hero_alive == true)
                    viable = true;
                break;
            case 2:
                if (unit1_alive == true)
                    viable = true;
                break;
            case 3:
                if (unit2_alive == true)
                    viable = true;
                break;
            case 4:
                if (unit3_alive == true)
                    viable = true;
                break;
            case 5:
                if (unit4_alive == true)
                    viable = true;
                break;
        }

        return viable;
    }

    public float EnemyActionDmg(float damage, float penetration, char type, int target)
    {
        switch (target)
        {
            case 1:
                amountf = hero.DamageMultiplyer(damage, penetration, type);
                break;
            case 2:
                amountf = unit1.DamageMultiplyer(damage, penetration, type);
                break;
            case 3:
                amountf = unit2.DamageMultiplyer(damage, penetration, type);
                break;
            case 4:
                amountf = unit3.DamageMultiplyer(damage, penetration, type);
                break;
            case 5:
                amountf = unit4.DamageMultiplyer(damage, penetration, type);
                break;
        }
        return amountf;
    }

    public void EnemyActionDealDmg(float damage, float penetration, char type, int target)
    {
        if (type == 's')
            color = 'p';
        else color = 'r';

        switch (target)
        {
            case 1:
                hero.TakeDamage(hero.DamageMultiplyer(damage, penetration, type), color);
                break;
            case 2:
                unit1.TakeDamage(unit1.DamageMultiplyer(damage, penetration, type), color);
                break;
            case 3:
                unit2.TakeDamage(unit2.DamageMultiplyer(damage, penetration, type), color);
                break;
            case 4:
                unit3.TakeDamage(unit3.DamageMultiplyer(damage, penetration, type), color);
                break;
            case 5:
                unit4.TakeDamage(unit4.DamageMultiplyer(damage, penetration, type), color);
                break;
        }
    }

    public void EnemyActionFlat(float value, string type, int target)
    {
        switch (type)
        {
            case "bleed":
                switch (target)
                {
                    case 1:
                        hero.GainBleed(value);
                        break;
                    case 2:
                        unit1.GainBleed(value);
                        break;
                    case 3:
                        unit2.GainBleed(value);
                        break;
                    case 4:
                        unit3.GainBleed(value);
                        break;
                    case 5:
                        unit4.GainBleed(value);
                        break;
                }
                break;

            case "energy":
                switch (target)
                {
                    case 1:
                        hero.LoseEnergy(value);
                        break;
                    case 2:
                        unit1.LoseEnergy(value);
                        break;
                    case 3:
                        unit2.LoseEnergy(value);
                        break;
                    case 4:
                        unit3.LoseEnergy(value);
                        break;
                    case 5:
                        unit4.LoseEnergy(value);
                        break;
                }
                break;
            case "mp":
                switch (target)
                {
                    case 1:
                        hero.LoseMP(value);
                        break;
                    case 2:
                        unit1.LoseMP(value);
                        break;
                    case 3:
                        unit2.LoseMP(value);
                        break;
                    case 4:
                        unit3.LoseMP(value);
                        break;
                    case 5:
                        unit4.LoseMP(value);
                        break;
                }
                break;
            case "ar":
                switch (target)
                {
                    case 1:
                        hero.LoseAR(value);
                        break;
                    case 2:
                        unit1.LoseAR(value);
                        break;
                    case 3:
                        unit2.LoseAR(value);
                        break;
                    case 4:
                        unit3.LoseAR(value);
                        break;
                    case 5:
                        unit4.LoseAR(value);
                        break;
                }
                break;
        }
    }

    public int EnemyActionChance(float chance, int target)
    {
        amount = 0;

        switch (target)
        {
            case 1:
                if (hero.GainDebuff(chance) == 1)
                    amount = 1;
                break;
            case 2:
                if (unit1.GainDebuff(chance) == 1)
                    amount = 1;
                break;
            case 3:
                if (unit2.GainDebuff(chance) == 1)
                    amount = 1;
                break;
            case 4:
                if (unit3.GainDebuff(chance) == 1)
                    amount = 1;
                break;
            case 5:
                if (unit4.GainDebuff(chance) == 1)
                    amount = 1;
                break;
        }

        return amount;
    }

    public void EnemyActionStatus(int value, string type, int target)
    {
        switch (type)
        {
            case "weakness":
                switch (target)
                {
                    case 1:
                        hero.GainWeakness(value);
                        break;
                    case 2:
                        unit1.GainWeakness(value);
                        break;
                    case 3:
                        unit2.GainWeakness(value);
                        break;
                    case 4:
                        unit3.GainWeakness(value);
                        break;
                    case 5:
                        unit4.GainWeakness(value);
                        break;
                }
                break;
            case "vulnerable":
                switch (target)
                {
                    case 1:
                        hero.GainVoulnerable(value);
                        break;
                    case 2:
                        unit1.GainVoulnerable(value);
                        break;
                    case 3:
                        unit2.GainVoulnerable(value);
                        break;
                    case 4:
                        unit3.GainVoulnerable(value);
                        break;
                    case 5:
                        unit4.GainVoulnerable(value);
                        break;
                }
                break;
            case "slow":
                switch (target)
                {
                    case 1:
                        hero.GainSlow(value);
                        break;
                    case 2:
                        unit1.GainSlow(value);
                        break;
                    case 3:
                        unit2.GainSlow(value);
                        break;
                    case 4:
                        unit3.GainSlow(value);
                        break;
                    case 5:
                        unit4.GainSlow(value);
                        break;
                }
                break;
            case "stun":
                switch (target)
                {
                    case 1:
                        hero.GainStun();
                        break;
                    case 2:
                        unit1.GainStun();
                        break;
                    case 3:
                        unit2.GainStun();
                        break;
                    case 4:
                        unit3.GainStun();
                        break;
                    case 5:
                        unit4.GainStun();
                        break;
                }
                break;
        }
    }

    public float ReturnValue(string type, int target)
    {
        switch (type)
        {
            case "number":
                switch (target)
                {
                    case 1:
                        amountf = 1;
                        break;
                    case 2:
                        amountf = unit1.number;
                        break;
                    case 3:
                        amountf = unit2.number;
                        break;
                    case 4:
                        amountf = unit3.number;
                        break;
                    case 5:
                        amountf = unit4.number;
                        break;
                }
                break;
            case "AD":
                switch (target)
                {
                    case 1:
                        amountf = hero.AD;
                        break;
                    case 2:
                        amountf = unit1.AD;
                        break;
                    case 3:
                        amountf = unit2.AD;
                        break;
                    case 4:
                        amountf = unit3.AD;
                        break;
                    case 5:
                        amountf = unit4.AD;
                        break;
                }
                break;
            case "MP":
                switch (target)
                {
                    case 1:
                        amountf = hero.MP;
                        break;
                    case 2:
                        amountf = unit1.MP;
                        break;
                    case 3:
                        amountf = unit2.MP;
                        break;
                    case 4:
                        amountf = unit3.MP;
                        break;
                    case 5:
                        amountf = unit4.MP;
                        break;
                }
                break;
            case "IN":
                switch (target)
                {
                    case 1:
                        amountf = hero.IN;
                        break;
                    case 2:
                        amountf = unit1.IN;
                        break;
                    case 3:
                        amountf = unit2.IN;
                        break;
                    case 4:
                        amountf = unit3.IN;
                        break;
                    case 5:
                        amountf = unit4.IN;
                        break;
                }
                break;
            case "HP":
                switch (target)
                {
                    case 1:
                        amountf = hero.HP;
                        break;
                    case 2:
                        amountf = unit1.HP;
                        break;
                    case 3:
                        amountf = unit2.HP;
                        break;
                    case 4:
                        amountf = unit3.HP;
                        break;
                    case 5:
                        amountf = unit4.HP;
                        break;
                }
                break;
            case "AR":
                switch (target)
                {
                    case 1:
                        amountf = hero.AR;
                        break;
                    case 2:
                        amountf = unit1.AR;
                        break;
                    case 3:
                        amountf = unit2.AR;
                        break;
                    case 4:
                        amountf = unit3.AR;
                        break;
                    case 5:
                        amountf = unit4.AR;
                        break;
                }
                break;
            case "size":
                switch (target)
                {
                    case 1:
                        amountf = hero.size;
                        break;
                    case 2:
                        amountf = unit1.size;
                        break;
                    case 3:
                        amountf = unit2.size;
                        break;
                    case 4:
                        amountf = unit3.size;
                        break;
                    case 5:
                        amountf = unit4.size;
                        break;
                }
                break;
            case "strength":
                switch (target)
                {
                    case 1:
                        amountf = hero.strength;
                        break;
                    case 2:
                        amountf = unit1.strength;
                        break;
                    case 3:
                        amountf = unit2.strength;
                        break;
                    case 4:
                        amountf = unit3.strength;
                        break;
                    case 5:
                        amountf = unit4.strength;
                        break;
                }
                break;
            case "resistance":
                switch (target)
                {
                    case 1:
                        amountf = hero.resistance;
                        break;
                    case 2:
                        amountf = unit1.resistance;
                        break;
                    case 3:
                        amountf = unit2.resistance;
                        break;
                    case 4:
                        amountf = unit3.resistance;
                        break;
                    case 5:
                        amountf = unit4.resistance;
                        break;
                }
                break;
            case "agility":
                switch (target)
                {
                    case 1:
                        amountf = hero.agility;
                        break;
                    case 2:
                        amountf = unit1.agility;
                        break;
                    case 3:
                        amountf = unit2.agility;
                        break;
                    case 4:
                        amountf = unit3.agility;
                        break;
                    case 5:
                        amountf = unit4.agility;
                        break;
                }
                break;
            case "poison":
                switch (target)
                {
                    case 1:
                        amountf = hero.poison;
                        break;
                    case 2:
                        amountf = unit1.poison;
                        break;
                    case 3:
                        amountf = unit2.poison;
                        break;
                    case 4:
                        amountf = unit3.poison;
                        break;
                    case 5:
                        amountf = unit4.poison;
                        break;
                }
                break;
            case "attacks":
                switch (target)
                {
                    case 1:
                        amountf = hero.attacks;
                        break;
                    case 2:
                        amountf = unit1.attacks;
                        break;
                    case 3:
                        amountf = unit2.attacks;
                        break;
                    case 4:
                        amountf = unit3.attacks;
                        break;
                    case 5:
                        amountf = unit4.attacks;
                        break;
                }
                break;
        }

        return amountf;
    }

    public string RandomBuff()
    {
        chance = Random.Range(1, 4);
        switch (chance)
        {
            case 1:
                buff = "strength";
                break;
            case 2:
                buff = "resistance";
                break;
            case 3:
                buff = "agility";
                break;
        }

        return buff;
    }

    public string RandomDebuff()
    {
        chance = Random.Range(1, 4);
        switch (chance)
        {
            case 1:
                buff = "weakness";
                break;
            case 2:
                buff = "vulnerable";
                break;
            case 3:
                buff = "slow";
                break;
        }

        return buff;
    }

    public void Lighting()
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.lightning_strike_dmg(), 1.0F, 's'), 'p');
                    if (enemy_alive[chance] == true)
                    {
                        if (enemy[chance].GainDebuff(hero.lightning_strike_stun()) == 1)
                        {
                            enemy[chance].GainStun();
                            enemy[chance].GainVoulnerable(1);
                        }
                    }
                }
                amount--;
            }
        }
    }

    public void Rage(char unit)
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    switch (unit)
                    {
                        case 'h':
                            hero.advance(1);
                            if (Tree[3].perk[13] > 0)
                            {
                                amountf = enemy[chance].DamageMultiplyer(hero.wrathful_dmg(), hero.pen, 'm');
                                enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.wrathful_dmg(), hero.pen, 'm'), 'r');
                                if (enemy_alive[chance] == true)
                                {
                                    hero.advance(1);
                                    enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.wrathful_dmg(), hero.pen, 'm'), 'r');
                                    amountf += enemy[chance].DamageMultiplyer(hero.wrathful_dmg(), hero.pen, 'm');
                                }
                                hero.blood_well(amountf * hero.blood_gain());
                            }
                            else
                            {
                                amountf = enemy[chance].DamageMultiplyer(hero.attack_dmg(), hero.pen, 'm');
                                enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.attack_dmg(), hero.pen, 'm'), 'r');
                                hero.blood_well(amountf * hero.blood_gain());
                            }
                            break;
                        case '1':
                            unit1.advance(1);
                            enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit1.attack_dmg(), 1.0F, 'm'), 'r');
                            break;
                        case '2':
                            unit2.advance(1);
                            unit2.brutality(enemy[chance].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r') * 0.03F / unit2.number);
                            enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit2.attack_dmg(), 1.0F, 'r'), 'r');
                            break;
                        case '3':
                            unit3.advance(1);
                            amountf = enemy[chance].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm');
                            enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit3.attack_dmg(), 1.0F, 'm'), 'r');
                            unit3.wrath(amountf * 0.27F / unit3.number);
                            break;
                        case '4':
                            unit4.advance(1);
                            if (unit4.spear_stacks > 0)
                            {
                                enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit4.spear_attack_dmg(), 0.7F, 'r'), 'r');
                                unit4.spear_stacks--;
                            }
                            else enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(unit4.attack_dmg(), 1.0F, 'r'), 'r');
                            break;
                    }
                    amount--;
                }
            }
        }
    }

    public void Justice()
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.smite_dmg(), 1.0F, 's'), 'p');
                    hero.justice -= 6;
                    if (enemy_alive[chance] == true)
                    {
                        if (enemy[chance].GainDebuff(hero.smite_S()) == 1) enemy[chance].GainSlow(1);
                        if (enemy[chance].strength > 0)
                        {
                            if (enemy[chance].GainDebuff(hero.smite_D(enemy[chance].strength)) == 1) enemy[chance].GainWeakness(1);
                        }
                        if (enemy[chance].resistance > 0)
                        {
                            if (enemy[chance].GainDebuff(hero.smite_D(enemy[chance].resistance)) == 1) enemy[chance].GainVoulnerable(1);
                        }
                    }
                    amount--;
                }
            }
        }
        else hero.justice -= 6;
    }

    public void Forest_Wrath()
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(hero.entangling_roots_dmg(), 1.0F, 's'), 'p');
                    if (enemy_alive[chance] == true)
                    {
                        if (enemy[chance].GainDebuff(hero.entangling_roots_S()) == 1) enemy[chance].GainStun();
                        if (enemy[chance].GainDebuff(hero.entangling_roots_S()) == 1) enemy[chance].GainWeakness(1);
                    }
                }
                amount--;
            }
        }
    }

    public void Transfusion(float damage)
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    amountf = enemy[chance].DamageMultiplyer(damage, 1.0F, 's');
                    enemy[chance].TakeDamage(enemy[chance].DamageMultiplyer(damage, 1.0F, 's'), 'p');
                    hero.RestoreHealth(amountf * 0.24F);
                    hero.blood_well(amountf * hero.blood_gain());
                }
                amount--;
            }
        }
    }

    public void RottenFruit(float poison)
    {
        if (enemy_alive[0] == true || enemy_alive[1] == true || enemy_alive[2] == true || enemy_alive[3] == true || enemy_alive[4] == true)
        {
            amount = 1;
            while (amount > 0)
            {
                chance = Random.Range(0, 5);
                if (enemy_alive[chance] == true)
                {
                    enemy[chance].GainPoison(poison);
                }
                amount--;
            }
        }
    }

    public void RandomBuffA()
    {
        chance = Random.Range(1, 4);
        switch (chance)
        {
            case 1:
                if (hero_alive == true)
                    hero.GainStrength(1);
                if (unit1_alive == true)
                    unit1.GainStrength(1);
                if (unit2_alive == true)
                    unit2.GainStrength(1);
                if (unit3_alive == true)
                    unit3.GainStrength(1);
                if (unit4_alive == true)
                    unit4.GainStrength(1);
                break;
            case 2:
                if (hero_alive == true)
                    hero.GainResistance(1);
                if (unit1_alive == true)
                    unit1.GainResistance(1);
                if (unit2_alive == true)
                    unit2.GainResistance(1);
                if (unit3_alive == true)
                    unit3.GainResistance(1);
                if (unit4_alive == true)
                    unit4.GainResistance(1);
                break;
            case 3:
                if (hero_alive == true)
                    hero.GainAgility(1);
                if (unit1_alive == true)
                    unit1.GainAgility(1);
                if (unit2_alive == true)
                    unit2.GainAgility(1);
                if (unit3_alive == true)
                    unit3.GainAgility(1);
                if (unit4_alive == true)
                    unit4.GainAgility(1);
                break;
        }
    }

    public void Kill(int kills, int target)
    {
        switch (target)
        {
            case 1:
                hero.TakeDamage(hero.DamageMultiplyer((kills * hero.HP * 0.9F / hero.size), 1, 'm'), 'r');
                break;
            case 2:
                unit1.TakeDamage(kills * unit1.HP, 'z');
                break;
            case 3:
                unit2.TakeDamage(kills * unit2.HP, 'z');
                break;
            case 4:
                unit3.TakeDamage(kills * unit3.HP, 'z');
                break;
            case 5:
                unit4.TakeDamage(kills * unit4.HP, 'z');
                break;
        }
    }
}