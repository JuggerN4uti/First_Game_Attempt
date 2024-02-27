using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Enemy1Stats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject enemy;
    public Image enemy_im;
    public Sprite[] sprites;
    public Sprite lord_of_torment, ratking, glorious_creation, core, bog_thing, mud_pile, corrupted_treant, tar_lord;
    public Button button;
    public field_e field;
    public AllStats stats;
    public int order, target;
    public HealthBar hpBar;
    public EnergyBar enBar;
    public TMPro.TextMeshProUGUI manaT;
    public NumberText numT;

    public Transform Enemy_bod;
    public GameObject Text_prefab;
    private IEnumerator coroutine;
    private int que = 0;
    public UnitDetailsText info;

    public string character;
    public char position;
    public float AD, MP, IN, HP, AR, pen;
    public float base_AD, base_MP, base_IN, base_HP, base_AR;
    public float hitPoints, shield, energy, mana, bleed, poison;
    public int Cost_1, Cost_2, Cost_3;
    public float fatigue_1, fatigue_2, fatigue_3;
    public int number, m_number, size, strength, resistance, agility, block, stun, freeze;
    public float amount, chance, amountt;
    public int total_attacks, attacks, roll;
    public bool attacked, boss;

    // living corpse
    private float fuel_left;
    // mad scientist
    private int unleash_chance;
    // ratking
    private float flesh;
    public int[] chained;
    // glorious creation
    private int electric_charges;
    // corrupted treant
    private float root_charges;
    private int cursed_targets;
    // bog-thing 
    private int amalgam_charges;

    public void attack()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        advance(1);
    }

    public float attack_dmg()
    {
        amount = AD;
        amount = Damageincrease(amount);
        return amount;
    }

    private void advance(int count)
    {
        attacks += count;
        total_attacks += count;
        stats.enemy_attacks += count;
        attacked = true;
        if (stats.Tree[0].perk[19] > 0)
        {
            stats.hero.energy += 2 * stats.Tree[0].perk[19];
            stats.hero.mana +=  stats.Tree[0].perk[19];
        }
        if (stats.Tree[0].perk[20] > 0)
            stats.hero.justice++;
        if (stats.map.items.collected[41] == true)
        {
            if (total_attacks % 5 == 0)
            {
                GainWeakness(1);
                GainVoulnerable(1);
                TakeDamage(7 * size * number, 'r');
            }
        }
    }

    // --- Revenants ---
    // 100% bleed resistant

    // Reanimated Soldier Skillset
    public void unholy_shlash()
    {
        energy -= 75;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
    }

    public float unholy_shlash_dmg()
    {
        amount = 1.12f * AD;
        amount = Damageincrease(amount);
        return amount;
    }

    public float unholy_slash_bleed()
    {
        return 4 * number;
    }

    public void might_of_the_grave()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 50;
        GainAD(0.85f);
        GainIN(0.6f);
        advance(1);
    }

    public float might_of_the_grave_dmg()
    {
        amount = (1.17f * AD) + (0.36f * (AD - base_AD));
        amount = Damageincrease(amount);
        return amount;
    }

    // Reanimated Jailer Skillset
    public float binding_chains()
    {
        return (40 * number * size);
    }

    public void flog()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 50;
        advance(1);
    }

    public float flog_dmg()
    {
        amount = 0.7f * AD;
        amount = Damageincrease(amount);
        return amount;
    }

    public float flog_mdmg()
    {
        amount = 0.6f * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    public int flog_sanity()
    {
        return 2 * number;
    }

    public void chain_lash(float targets_agility)
    {
        energy -= 90;
        if (targets_agility < 0)
            GainEnergy(-5 * targets_agility * number * size);
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 32;
        advance(1);
    }

    public float chain_lash_dmg()
    {
        amount = 1.1f * AD;
        amount = Damageincrease(amount);
        return amount;
    }

    public float chain_lash_en(float targets_agility)
    {
        amount = 15;
        if (targets_agility < 0)
            amount -= 5 * targets_agility;
        amount *= number * size;
        return amount;
    }

    // Soul Collector
    private void dark_shroud(float mana_spent)
    {
        GainShield(mana_spent * 0.3f);
    }

    private void soul_drain()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 42;
        advance(1);
    }

    public float soul_drain_dmg()
    {
        amount = 10 + (0.4f * AD) + (0.6f * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float soul_drain_MP()
    {
        return ((50 + (MP - base_MP)) * number * size);
    }

    private void piercing_pain()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_1 += 55;
        advance(1);
    }

    public float piercing_pain_dmg()
    {
        amount = 16 + (1.18f * MP) + (0.66f * (MP - base_MP));
        amount = Damageincrease(amount);
        return amount;
    }

    public float piercing_pain_pen()
    {
        return 0.26f + 0.0039f * (MP - base_MP);
    }

    // Revenant Legionary
    public float unholy_armor(float value)
    {
        value *= 0.75f;
        return value;
    }

    private void trapped_souls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            roll = Random.Range(1, 6);
            switch (roll)
            {
                case 1:
                    GainAD(0.03f * AD);
                    break;
                case 2:
                    GainMP(0.03f * MP);
                    break;
                case 3:
                    GainAR(0.04f * AR);
                    break;
                case 4:
                    mana += 30;
                    break;
                case 5:
                    GainEnergy(25 * size * number);
                    break;
            }
        }
    }

    private void obliterate()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        Cost_1 += 4;
        fatigue_1 += 20;
        advance(1);
    }

    public float obliterate_dmg()
    {
        amount = 19 + (1.12F * AD) + Cost_1 * 2;
        amount = Damageincrease(amount);
        return amount;
    }

    public void death_sentance()
    {
        energy -= 50;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 50;
        advance(1);
    }

    public float death_sentance_V()
    {
        return (84 * number * size);
    }

    public float death_sentance_dmg(float targets_resistance)
    {
        amount = 15 + (1.62f * MP) + (10 * -targets_resistance);
        amount = Damageincrease(amount);
        return amount;
    }

    // Lord of Torment Skillset
    private float share_torment_dmg(float target_number, float target_size)
    {
        // reduce sanity by 3
        amount = 10 + (0.04F * MP);
        amount *= target_number * target_size;
        amount = Damageincrease(amount);
        return amount;
    }

    private void costly_power()
    {
        fatigue_1 -= 5;
        if (fatigue_1 <= 0) fatigue_1 = 0;
        fatigue_2 -= 5;
        if (fatigue_2 <= 0) fatigue_2 = 0;
        fatigue_3 -= 5;
        if (fatigue_3 <= 0) fatigue_3 = 0;
        GainMP(2);
        pen += 0.005F;
        if (pen > 1f)
            pen = 1f;
    }

    private void costly_power_deb()
    {
        switch (stats.RandomDebuff())
        {
            case "weakness":
                GainWeakness(1);
                break;
            case "vulnerable":
                GainVoulnerable(1);
                break;
            case "slow":
                GainSlow(1);
                break;
        }
    }

    private void crushing_pain()
    {
        energy -= 55;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 37;
    }

    private float crushing_pain_dmg()
    {
        amount = 24 + (0.18F * AD) + (1.13F * MP);
        if (resistance < 0)
            amount *= 1 - (0.07f * resistance);
        amount = Damageincrease(amount);
        return amount;
    }

    private float crushing_pain_W()
    {
        amount = 75 + (0.25F * MP);
        if (strength < 0)
            amount += 40 * (-strength);
        amount *= size * number;
        return amount;
    }

    private void anguish()
    {
        energy -= 70;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 48;
    }

    private void anguish_att()
    {
        if (stats.TargetAvailable(1) == true || stats.TargetAvailable(2) == true || stats.TargetAvailable(3) == true)
        {
            for (int i = 1; i < 4; i++)
            {
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionDealDmg(anguish_dmg(), anguish_pen(), 'm', i);
                    if (stats.TargetAvailable(i) == true)
                    {
                        if (stats.EnemyActionChance(anguish_V(), i) == 1)
                        {
                            stats.EnemyActionStatus(1, "vulnerable", target);
                            stats.EnemyActionDealDmg(share_torment_dmg(stats.ReturnValue("number", i), stats.ReturnValue("size", i)), (1f - pen), 's', i);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 4; i < 6; i++)
            {
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionDealDmg(anguish_dmg(), anguish_pen(), 'm', i);
                    if (stats.TargetAvailable(i) == true)
                    {
                        if (stats.EnemyActionChance(anguish_V(), i) == 1)
                        {
                            stats.EnemyActionStatus(1, "vulnerable", target);
                            stats.EnemyActionDealDmg(share_torment_dmg(stats.ReturnValue("number", i), stats.ReturnValue("size", i)), (1f - pen), 's', i);
                        }
                    }
                }
            }
        }
    }

    private float anguish_dmg()
    {
        amount = 4 + (0.56F * AD) + (0.14F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    private float anguish_pen()
    {
        amount = 1 - pen;
        if (agility < 0)
            amount -= 0.04f * (-agility);
        return amount;
    }

    private float anguish_V()
    {
        amount = 40 + (0.1F * AD);
        if (resistance < 0)
            amount += 20 * (-resistance);
        amount *= size * number;
        return amount;
    }

    private void crippling_fear()
    {
        energy -= 44;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 62;
    }

    private void crippling_fear_att()
    {
        if (stats.TargetAvailable(4) == true || stats.TargetAvailable(5) == true)
        {
            for (int i = 4; i < 6; i++)
            {
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionDealDmg(crippling_fear_dmg(), (1 - pen), 'r', i);
                    if (stats.TargetAvailable(i) == true)
                    {
                        if (stats.EnemyActionChance(crippling_fear_S(), i) == 1)
                        {
                            stats.EnemyActionStatus(1, "slow", target);
                            stats.EnemyActionDealDmg(share_torment_dmg(stats.ReturnValue("number", i), stats.ReturnValue("size", i)), (1f - pen), 's', i);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 1; i < 4; i++)
            {
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionDealDmg(crippling_fear_dmg(), (1 - pen), 'r', i);
                    if (stats.TargetAvailable(i) == true)
                    {
                        if (stats.EnemyActionChance(crippling_fear_S(), i) == 1)
                        {
                            stats.EnemyActionStatus(1, "slow", target);
                            stats.EnemyActionDealDmg(share_torment_dmg(stats.ReturnValue("number", i), stats.ReturnValue("size", i)), (1f - pen), 's', i);
                        }
                    }
                }
            }
        }
    }

    private float crippling_fear_dmg()
    {
        amount = 28 + (1.3F * MP) + (0.58F * (MP - base_MP));
        amount = Damageincrease(amount);
        return amount;
    }

    private float crippling_fear_S()
    {
        amount = 90;
        if (agility < 0)
            amount += 30 * (-agility);
        amount *= size * number;
        return amount;
    }

    private float crippling_fear_en()
    {
        amount = 4 * (-strength);
        amount *= size * number;
        return amount;
    }

    // --- Undeads ---
    // 60% slow resistant

    // Rotfiend Skillset
    public void rot()
    {
        GainAD(0.9F);
        GainMP(0.4F);
        LoseHP(3.3F);
        LoseAR(0.4F);
    }

    public void tear_flesh()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 46;
        GainHP(5);
        advance(1);
    }

    public float tear_flesh_dmg()
    {
        amount = 2 + (1.03F * AD) + (0.33F * (AD - base_AD));
        amount = Damageincrease(amount);
        return amount;
    }

    public void decay()
    {
        energy -= 60;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 37;
        advance(1);
    }

    public float decay_dmg()
    {
        amount = 4 + (0.62F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float decay_deb()
    {
        amount = 40 + (0.85F * MP);
        amount *= size * number;
        return amount;
    }

    public int decay_sanity()
    {
        return 1 * number;
    }

    // Husk Skillset
    private void fly_swarm()
    {
        for (int i = 0; i < 4; i++)
        {
            target = stats.EnemyTarget('r');
            stats.EnemyActionDealDmg(fly_swarm_dmg(), (1 - pen), 's', target);
        }
    }

    private float fly_swarm_dmg()
    {
        amount = 2;
        amount = Damageincrease(amount);
        return amount;
    }

    private void flies_nest()
    {
        energy -= 64;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 48;
        GainResistance(1);
        amount = 4 + (0.04F * HP) + (0.12F * AR);
        GainShield(amount);
        fly_swarm();
    }

    private void haunting_wail()
    {
        energy -= 56;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 43;
        advance(1);
    }

    public float haunting_wail_dmg()
    {
        amount = 8 + (0.07F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public int haunting_wail_sanity()
    {
        return 3 * number;
    }

    // Ghoul Skillset
    public float bone_claws(float targets_AR)
    {
        amount = 5 * number;
        amount /= (1 + targets_AR * 0.02f);
        return amount;
    }

    public void only_purpose()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 32;
        advance(2);
    }

    public float only_purpose_dmg()
    {
        amount = 1 + (0.62F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public void cannibalize()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 49;
        advance(1);
    }

    public float cannibalize_dmg()
    {
        amount = 8 + AD + (0.12F * IN);
        amount = Damageincrease(amount);
        return amount;
    }

    public void cannibalize_restore(float damage_dealt)
    {
        amount = damage_dealt / 10;
        RestoreHealth(amount);
    }

    // Shambler Skillset
    public float hollow(float value)
    {
        value *= 0.7f;
        GainBleed(value * 0.025f);
        return value;
    }
    public int empty_stare()
    {
        return 2 * number;
    }

    public void eye_for_en_eye()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
    }

    public float eye_for_en_eye_dmg()
    {
        amount = (0.17F * AD) + (1.9F * MP) + bleed;
        amount = Damageincrease(amount);
        return amount;
    }

    public void eye_for_en_eye_bleed()
    {
        GainBleed(bleed * 0.12f);
    }

    public void no_regrets()
    {
        energy -= 80;
        energy += 2 * total_attacks;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 32;
        advance(1);
    }

    public float no_regrets_dmg()
    {
        amount = 9 + (1.35F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    // --- Constructs ---
    // 60% weak resistant

    // Living Corpse Skillset
    private float fuel_tank(float energy)
    {
        energy *= 1 + fuel_left;
        return energy;
    }

    private void lose_fuel(float value_lost)
    {
        fuel_left -= value_lost;

        if (fuel_left < -0.1f)
        {
            energy += (fuel_left + 0.1f) * 700;
            enBar.SetEnergy(energy);
            fuel_left = -0.1f;
        }
    }

    private void blind_rage()
    {
        energy -= 50;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
        lose_fuel(0.04f);
    }

    public float blind_rage_dmg()
    {
        amount = 1 + (1.21F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    private void running_on_fumes()
    {
        energy -= 60;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 30;

        amount = 5 + (0.05F * HP) + (0.16F * AR);
        GainShield(amount);

        if (fuel_left < 0f)
        {
            fatigue_2 -= 16;
            energy += 32;
            enBar.SetEnergy(energy);
        }
    }

    // Mad Scientist Skillset
    private void monster_within()
    {
        roll = Random.Range(1, 101);
        if (roll <= unleash_chance)
            monster_unleashed();
        else unleash_chance += 4;
    }

    private void toxic_flask()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 28;
        advance(1);
    }

    public float toxic_flask_dmg()
    {
        amount = 2;
        amount = Damageincrease(amount);
        return amount;
    }

    public float toxic_flask_poison()
    {
        return 7 * number;
    }

    public float toxic_flask_deb()
    {
        amount = 49 + (0.94F * AD);
        amount *= size * number;
        return amount;
    }

    private void experimental_concoction()
    {
        energy -= 30;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 33;
        GainAD(1);
        GainStrength(1);
        GainAgility(1);
        GainVoulnerable(1);
        monster_within();
    }

    // Monstrosity Skillset
    private void monster_unleashed()
    {
        Cost_1 = 30; Cost_2 = 44;
        fatigue_1 = 0; fatigue_2 = 0;
        enemy_im.sprite = sprites[11];
        character = "monstrosity";
        GainAD(3 + 0.25F * AD);
        GainIN(.1F + 0.05F * AD);
        GainHP(18 + 0.36F * AD);
        GainAR(3 + 0.14F * AD);
        size = 2;
    }

    private void corrosive_cask()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 42;
    }

    private void corrosive_cask_att()
    {
        for (int i = 1; i < 6; i++)
        {
            if (stats.TargetAvailable(i) == true)
            {
                stats.EnemyActionDealDmg(corrosive_cask_dmg(), (1 - pen), 'r', i);
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionFlat(corrosive_cask_poison(), "poison", i);
                    if (stats.EnemyActionChance(corrosive_cask_ar(), i) == 1)
                        stats.EnemyActionFlat(stats.ReturnValue("AR", i) * 0.06F, "ar", i);
                }
            }
        }
    }

    public float corrosive_cask_dmg()
    {
        amount = 2 + (0.08F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float corrosive_cask_poison()
    {
        return 3 * number;
    }

    public float corrosive_cask_ar()
    {
        amount = 22 + (0.36F * AD);
        amount *= size * number;
        return amount;
    }

    private void purge()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 33;
    }

    private void purge_att()
    {
        for (int i = 0; i < 5; i++)
        {
            target = stats.EnemyTarget('r');
            stats.EnemyActionDealDmg(purge_dmg(stats.ReturnValue("strength", target)), purge_pen(stats.ReturnValue("strength", target)), 'm', target);
        }
    }

    private float purge_dmg(float target_strength)
    {
        amount = 3.9F + (0.24F * AD);
        if (target_strength < 0)
            amount *= 1 + (-0.06F * target_strength);
        amount = Damageincrease(amount);
        return amount;
    }

    private float purge_pen(float target_strength)
    {
        amount = 1f;
        if (target_strength < 0)
            amount += 0.05f * target_strength;
        return amount;
    }

    // Dreadnought Skillset
    private void iron_casing()
    {
        amount = 23 + (0.25F * HP);
        amount *= number;
        GainShield(amount);
    }

    private float iron_casing_en(float energy)
    {
        energy /= 1 + ((shield / 800) / number);
        return energy;
    }

    private void slam()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
    }

    private float slam_dmg()
    {
        amount = 4 + (0.85F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    private float slam_dmg_others()
    {
        amount = 2 + (0.34F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    private void grab()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 40;
        advance(1);
    }

    private float grab_dmg()
    {
        amount = 20 + (0.44F * AD) + (0.05F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    private void grab_en(int targeted)
    {
        amount = stats.ReturnValue("number", targeted) * stats.ReturnValue("size", targeted);
        amount /= number;
        amount *= 5;
        energy -= amount;
        enBar.SetEnergy(energy);
    }

    // Abomination Skillset
    private void vile_gas()
    {
        for (int i = 1; i < 6; i++)
        {
            if (stats.TargetAvailable(i) == true)
            {
                amount = 4 + (stats.ReturnValue("poison", i) * 0.1F);
                amount *= number;
                stats.EnemyActionFlat(amount, "poison", i);
            }
        }
    }

    private void play_time()
    {
        if (attacks < 2)
        {
            energy += 22 - (attacks * 11);
        }
    }

    private float play_time_dmg()
    {
        amount = 0.02F * HP;
        amount = Damageincrease(amount);
        return amount;
    }

    private void meat_hook()
    {
        energy -= 60;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 45;
        advance(1);
    }

    private float meat_hook_bleed()
    {
        amount = 5.1F + (0.08F * AD);
        amount *= number;
        return amount;
    }

    private float meat_hook_stun()
    {
        amount = 56 + (0.05F * HP);
        amount *= size * number;
        return amount;
    }

    private void gorge(float target_size, float target_AD, float target_HP, float target_AR)
    {
        energy -= 55;
        energy += 20 * target_size;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 15 + 35 * target_size;

        amount = 4 / (target_size + 1);
        amount *= 0.01F;
        GainAD(amount * target_AD);
        GainHP(amount * target_HP);
        GainAR(amount * target_AR);

        amount = target_size - 1;
        for (int i = 0; i < amount; i++)
        {
            GainStun();
        }
    }

    // Ratking Skillset
    private void more_flesh(float damage_dealt)
    {
        amount = damage_dealt * 0.025F;
        GainHP(amount);
        flesh += amount;
        if (flesh >= 800)
        {
            flesh -= 800;
            size++;
        }
    }

    private void chain_up(int chains)
    {
        for (int i = 0; i < chains; i++)
        {
            target = stats.EnemyTarget('r');
            chained[target - 1]++;
        }
    }

    private void lambs_to_the_slaughter()
    {
        for (int i = 0; i < 5; i++)
        {
            if (chained[i] > 0)
            {
                if (stats.TargetAvailable(i+1))
                {
                    stats.EnemyActionDealDmg(lambs_to_the_slaughter_dmg(chained[i]), (0.8f - pen), 'm', i + 1);
                    stats.EnemyActionFlat(lambs_to_the_slaughter_bleed(chained[i]), "bleed", i + 1);
                    more_flesh(stats.EnemyActionDmg(lambs_to_the_slaughter_dmg(chained[i]), (0.8f - pen), 'm', i + 1));
                }
            }
        }
    }

    private float lambs_to_the_slaughter_dmg(int chain_amount)
    {
        amount = AD * 0.3F * chain_amount;
        amount = Damageincrease(amount);
        return amount;
    }

    private float lambs_to_the_slaughter_bleed(int chain_amount)
    {
        amount = (3 + 0.01F * (HP - base_HP)) * chain_amount;
        return amount;
    }

    private void stitched_up()
    {
        amount = 0.06f * (HP - base_HP);
        RestoreHealth(amount);
        amount *= 0.16f;
        GainBleed(amount);
    }

    private void pulverize()
    {
        energy -= 68;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 43;
        advance(1);
    }

    private float pulverize_dmg()
    {
        amount = AD * 1.55F + 0.1F * (HP - base_HP);
        amount = Damageincrease(amount);
        return amount;
    }

    private float pulverize_S()
    {
        amount = 23 + 0.004F * HP;
        amount *= size * number;
        if (size > 20)
            amount *= 1 + 0.08F * (size - 20);
        return amount;
    }

    private void new_playthings()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 57;
        advance(1);
    }

    private float new_playthings_dmg()
    {
        amount = AD * 0.15F + MP * 0.37F;
        amount = Damageincrease(amount);
        return amount;
    }

    private float new_playthings_S()
    {
        amount = 22 + 0.25F * MP + 0.01F * HP;
        amount *= size * number;
        return amount;
    }

    private void new_playthings_buff()
    {
        for (int i = 0; i < 5; i++)
        {
            energy += 2 * chained[i];
            mana += chained[i];
        }
        enBar.SetEnergy(energy);
        manaT.text = mana.ToString("0");
    }

    private void chop_chop()
    {
        energy -= 72;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 54;
        advance(1);
    }

    private float chop_chop_dmg()
    {
        amount = 28 + AD * 0.64F;
        amount += (4 + 0.03F * AD) * total_attacks;
        amount = Damageincrease(amount);
        return amount;
    }

    // Glorious Creation Skillset
    private void electrically_charged()
    {
        electric_charges++;
        target = stats.EnemyTarget('r');
        stats.EnemyActionDealDmg(electrically_charged_dmg(), 1, 's', target);
        if (stats.EnemyActionChance(electrically_charged_dmg_S(stats.EnemyActionDmg(electrically_charged_dmg(), 1, 's', target)), target) == 1)
            stats.EnemyActionStatus(1, "stun", target);
    }

    private float electrically_charged_dmg()
    {
        amount = (0.4F + MP * 0.04F) * electric_charges;
        amount = Damageincrease(amount);
        return amount;
    }

    private float electrically_charged_dmg_S(float damage_dealt)
    {
        amount = 20 + 0.12F * damage_dealt;
        amount *= size * number;
        return amount;
    }

    private float accustomed_to_pain(float damage)
    {
        damage = damage / (1 + 0.005F * damage);
        return damage;
    }

    private void overcharged_mp()
    {
        mana += 2 + 0.4F * electric_charges;
        manaT.text = mana.ToString("0");
    }

    private void overcharged_dmg(float fatigue)
    {
        TakeDamage(fatigue, 'p');
    }

    private void chain_lighting()
    {
        energy -= 70;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 50;
        overcharged_dmg(fatigue_1);
        advance(1);
        electric_charges += 4;
    }

    private float chain_lighting_dmg()
    {
        amount = 12 + 0.21F * MP;
        amount += (0.3F + 0.02F * MP) * electric_charges;
        amount = Damageincrease(amount);
        return amount;
    }

    private void battery_drain()
    {
        energy -= 86;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 62;
        overcharged_dmg(fatigue_2);
        electric_charges += 9;
        if (stats.enemy_alive[3] == true)
        {
            electric_charges += 3;
            GainStrength(2);
            stats.enemy[3].GainStun();
        }
        if (stats.enemy_alive[4] == true)
        {
            electric_charges += 3;
            GainAgility(2);
            stats.enemy[4].GainStun();
        }
    }

    private void power_overwhelming()
    {
        energy -= 94 / (1 + 0.01F * electric_charges);
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 62 / (1 + 0.01F * electric_charges);
        overcharged_dmg(fatigue_3);
        advance(1);
    }

    private float power_overwhelming_dmg()
    {
        amount = 83 + 1.48F * AD;
        if (strength > 0)
            amount += (4.5F + 0.05F * AD) * strength;
        amount = Damageincrease(amount);
        return amount;
    }

    // Energy Core Skillset
    private void charge_up()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        if (order == 3)
        {
            stats.enemy[0].GainAD(stats.enemy[0].AD * 0.01F);
            stats.enemy[0].GainEnergy(15 * stats.enemy[0].size);
        }
        else
        {
            stats.enemy[0].electric_charges++;
            stats.enemy[0].mana += 12;
            stats.enemy[0].manaT.text = stats.enemy[0].mana.ToString("0");
        }
        GainMP(1);
    }

    private void electric_surge()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 50;
        if (order == 3)
        {
            stats.enemy[0].mana += Cost_3 * (1 + (0.01F * fatigue_3)) * electric_surge_mana();
            stats.enemy[0].manaT.text = stats.enemy[0].mana.ToString("0");
            stats.enemy[0].fatigue_3 -= 4 + fatigue_3 * 0.06F;
            if (stats.enemy[0].fatigue_3 < 0)
                stats.enemy[0].fatigue_3 = 0;
        }
        else
        {
            stats.enemy[0].electric_charges += 3;
            stats.enemy[0].mana += Cost_1 * (1 + (0.01F * fatigue_1)) * electric_surge_mana();
            stats.enemy[0].manaT.text = stats.enemy[0].mana.ToString("0");
            stats.enemy[0].fatigue_1 -= 5 + fatigue_1 * 0.08F;
            if (stats.enemy[0].fatigue_1 < 0)
                stats.enemy[0].fatigue_1 = 0;
        }
    }

    private float electric_surge_mana()
    {
        amount = 11F + 0.3F * MP;
        amount /= 100;
        return amount;
    }

    // Corrupted Treant Skillset
    private void thick_bark(float damage)
    {
        LoseAR(damage / 40);
    }

    private void twisted_magic(float damage)
    {
        GainMP(damage / 50);
        mana += damage * 0.22f;
    }

    private void vengeful_roots()
    {
        root_charges += (2 + 0.008F * MP) * size * 0.05F;
        if (root_charges >= 10)
        {
            target = stats.EnemyTarget('r');
            stats.EnemyActionStatus(1, "stun", target);
            stats.EnemyActionStatus(1, "vulnerable", target);
            root_charges -= 3 + stats.ReturnValue("size", target) * stats.ReturnValue("number", target);
        }
    }

    private void bramble_smash()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
    }

    private float bramble_smash_dmg()
    {
        amount = 7 + 1.13F * AD;
        amount = Damageincrease(amount);
        return amount;
    }

    private float bramble_smash_S()
    {
        amount = 22 + 0.31F * MP;
        amount *= size * number;
        return amount;
    }

    private void regrowth()
    {
        energy -= 60;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 60;

        amount = 0.2F * MP + 2 * (MP - base_MP) + 0.07F * HP;
        RestoreHealth(amount);
        if (AR < base_AR)
        {
            amount = 14 + (MP - base_MP);
            amount /= 100;
            GainAR((base_AR - AR) * amount);
        }
        fatigue_1 *= 0.84f;
        fatigue_3 *= 0.84f;
    }

    private void cursed_existance()
    {
        energy -= 50;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 70;
        advance(1);
    }

    private float cursed_existance_dmg()
    {
        amount = 0.85F * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float cursed_existance_poison()
    {
        amount = 10 + 0.03F * MP;
        amount *= number;
        return amount;
    }

    private float cursed_existance_W()
    {
        amount = 10 + 0.25F * MP;
        amount *= size * number;
        return amount;
    }

    // Tar Lord Skillset
    private float intangible_body(float damage)
    {
        return damage *= 0.12F;
    }

    private void intangible_body_hp()
    {
        TakeDamage(63 + 0.04F * HP, 'r');
    }

    private float trapped_in_tar_dmg()
    {
        amount = 6 + 0.1F * (AD + MP);
        amount = Damageincrease(amount);
        return amount;
    }

    private void drown()
    {
        energy -= 54;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 42;
        advance(1);
    }

    private float drown_dmg()
    {
        amount = 18 + 0.21F * AD + 1.56F * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float drown_S()
    {
        amount = 22 + 0.12F * MP;
        amount *= size * number;
        return amount;
    }

    private void combust()
    {
        energy -= 66;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 52;
        advance(1);
        TakeDamage(31 + 0.025F * HP, 'r');
    }

    private float combust_dmg()
    {
        amount = 11 + 0.29F * MP + 0.004F * HP;
        amount = Damageincrease(amount);
        return amount;
    }

    private void no_escape()
    {
        energy -= 85;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 68;
        advance(1);
    }

    private float no_escape_dmg()
    {
        amount = 11 + 0.28F * AD + 1.78F * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float no_escape_dmg_ot()
    {
        amount = 37 + 0.63F * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float no_escape_S()
    {
        amount = 20 + 0.26F * MP;
        amount *= size * number;
        return amount;
    }

    // Bog-Thing Skillset
    private void pile_destroyed_e()
    {
        TakeDamage(0.04F * HP, 'r');
    }

    private void pile_destroyed_a()
    {
        size++;
        for (int i = 0; i < 2; i++)
        {
            switch (stats.RandomBuff())
            {
                case "strength":
                    GainStrength(1);
                    break;
                case "resistance":
                    GainResistance(1);
                    break;
                case "agility":
                    GainAgility(1);
                    break;
            }
        }
    }

    private void disgusting_mix()
    {
        for (int i = 0; i < 3; i++)
        {
            roll = Random.Range(1, 10);
            if (roll < 6)
            {
                stats.EnemyActionDealDmg(disgusting_mix_dmg(), 1, 's', roll);
            }
            else
            {
                stats.enemy[roll - 5].TakeDamage(stats.enemy[roll - 5].DamageMultiplyer(disgusting_mix_dmg_pile(), 1.0F, 's'), 'p');
            }
        }
    }

    private float disgusting_mix_dmg()
    {
        amount = 0.41F * MP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float disgusting_mix_dmg_pile()
    {
        amount = disgusting_mix_dmg();
        amount *= 1.05f + 0.01F * size;
        return amount;
    }

    private void amalgamation()
    {
        amalgam_charges++;
        if (amalgam_charges >= 5)
        {
            amalgam_charges -= 5;
            GainAD(AD * 0.01F);
            GainMP(MP * 0.01F);
            GainAR(AR * 0.01F);
        }
    }

    private void smash()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 30;
        advance(1);
    }

    private float smash_dmg()
    {
        amount = 11 + 0.3F * AD + 3 * size;
        amount = Damageincrease(amount);
        return amount;
    }

    private void smash_att()
    {
        for (int i = 1; i < 10; i++)
        {
            if (i < 6)
            {
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionDealDmg(smash_dmg(), 0.8F, 'm', i);
                }
            }
            else
            {
                stats.enemy[i - 5].TakeDamage(stats.enemy[i - 5].DamageMultiplyer(smash_dmg(), 0.8F, 'm'), 'r');
            }
        }
    }

    private void assimilate()
    {
        energy -= 40;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 50;

        amount = 0;
        for (int i = 1; i < 5; i++)
        {
            amount += stats.enemy[i].HP * 0.05F;
            stats.enemy[i].LoseHP(stats.enemy[i].HP * 0.05F);
        }
        GainHP(amount);

        for (int i = 0; i < 1 + amount * 0.01F; i++)
        {
            size++;
            switch (stats.RandomBuff())
            {
                case "strength":
                    GainStrength(1);
                    break;
                case "resistance":
                    GainResistance(1);
                    break;
                case "agility":
                    GainAgility(1);
                    break;
            }
        }
    }

    private void dissolving_ooze()
    {
        energy -= 20;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 75;
        advance(1);
    }

    private float dissolving_ooze_dmg()
    {
        amount = 0.44F * MP + 0.011F * HP;
        amount = Damageincrease(amount);
        return amount;
    }

    private float dissolving_ooze_poison()
    {
        amount = 1 + 0.001F * HP;
        amount *= number;
        return amount;
    }

    private float dissolving_ooze_W()
    {
        amount = 10 + 0.1F * MP + 0.5F * size;
        amount *= size * number;
        return amount;
    }

    private void dissolving_ooze_att()
    {
        for (int i = 1; i < 6; i++)
        {
            if (stats.TargetAvailable(i) == true)
            {
                stats.EnemyActionDealDmg(dissolving_ooze_dmg(), 1F, 's', i);
                if (stats.TargetAvailable(i) == true)
                {
                    stats.EnemyActionFlat(dissolving_ooze_poison(), "poison", i);
                    if (stats.EnemyActionChance(dissolving_ooze_W(), i) == 1)
                        stats.EnemyActionStatus(1, "weakness", target);
                    if (stats.EnemyActionChance(dissolving_ooze_W(), i) == 1)
                        stats.EnemyActionStatus(1, "slow", target);
                    if (stats.EnemyActionChance(dissolving_ooze_W(), i) == 1)
                        stats.EnemyActionFlat(stats.ReturnValue("AR", i) * 0.04F, "ar", i);
                    if (stats.EnemyActionChance(dissolving_ooze_W(), i) == 1)
                        stats.EnemyActionFlat(stats.ReturnValue("AR", i) * 0.04F, "ar", i);
                }
            }
        }
    }

    // Mud Pile Skillset
    private void dead()
    {
        GainHP(HP * 0.01F);
        mana += HP * 0.025f;
        manaT.text = mana.ToString("0");
    }

    private void pile_death()
    {
        HP *= 1.2F;
        hitPoints = HP;
        AR *= 1.02F;
        number = 1;

        if (stats.whoseTurn != 0)
            stats.enemy[0].pile_destroyed_e();
        else
            stats.enemy[0].pile_destroyed_a();
    }

    private void it_lives()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 100;

        GainAD(15 + 0.2F * HP);
        GainIN(6 + 0.08F * HP);
        size += 3;
    }

    // Common Skillset
    public float Damageincrease(float value)
    {
        amount = value;
        if (character == "tar_lord")
            amount *= 1 + 0.032F * stats.turnTime;
        if (stats.Tree[0].perk[12] > 0)
        {
            if (strength < 0)
                amount /= 1 + (0.005F + (0.015F + 0.005F * strength) * attacks) * stats.Tree[0].perk[12];
            else amount /= 1 + (0.005F + 0.015F * attacks) * stats.Tree[0].perk[12];
        }
        if (strength >= 0) amount *= 1 + (0.05F * strength);
        else amount /= 1 + (-0.05F - 0.01F * stats.Tree[0].perk[9]) * strength;
        amount *= number;
        return amount;
    }

    public float DamageMultiplyer(float value, float pen, char range)
    {
        if ((range == 'm') || (range == 'r'))
            value /= 1 + (AR * 0.02F * pen);
        else if (range == 's')
        {
            value /= 1 + (MP * 0.01F * pen);
            if (character == "revenant_legionary")
                value = unholy_armor(value);
        }

        if (resistance >= 0) value /= 1 + (0.05F * resistance);
        else value *= 1 + (-0.05F - 0.01F * stats.Tree[0].perk[16]) * resistance;
        if (range == 'r')
            if (position == 'r')
                if (stats.frontLineE == true)
                    value *= 0.6F;
        if (character == "shambler")
            value = hollow(value);
        else if (character == "glorious_creation")
            value = accustomed_to_pain(value);
        else if (character == "corrupted_treant")
        {
            if (range != 's')
                value *= 0.5f;
        }
        else if (character == "tar_lord")
            value = intangible_body(value);

        if (attacks > 0)
        {
            value *= 1 + ((0.04f + 0.02f * attacks) * stats.Tree[0].perk[14]);
        }
        return value;
    }

    public void TakeDamage(float value, char hue)
    {
        amount = 0;
        if (character == "corrupted_treant")
        {
            if (hue == 'p')
                twisted_magic(value);
            else
                thick_bark(value);
        }
        if (block > 0)
        {
            block--;
            Display_Text("blocked", 'w');
        }
        else
        {
            Display_Text(value.ToString("0"), hue);
            if (shield > 0)
            {
                shield -= value;
                if (shield < 0)
                {
                    hitPoints += shield;
                    shield = 0;
                }
            }
            else hitPoints -= value;
            while (hitPoints <= 0)
            {
                hitPoints += HP;
                number--;
                if (stats.Tree[3].perk[3] > 0)
                {
                    stats.hero.GainAD(stats.Tree[3].perk[3] * 4 * size);
                    stats.hero.GainMP(stats.Tree[3].perk[3] * size);
                    stats.hero.blood_well(stats.Tree[3].perk[3] * 25 * size);
                }
                amount++;
                if (number == 0)
                    Death();
            }
            hpBar.SetHealth(hitPoints);
            numT.Display(number);
            if (amount != 0)
                if (stats.enemy_alive[order] == true)
                    Display_Text(amount.ToString("") + " killed", 'z');
        }
    }

    public void Death()
    {
        if (character == "mud_pile")
            pile_death();
        else
        {
            if (boss == true)
            {
                Display_Text("Threat eliminated!", 'z');
                stats.BossDefeated();
            }
            else
            {
                Display_Text("Enemy vanquished", 'z');
                stats.EnemyTakenDown(order);
            }
            enemy.SetActive(false);
        }
    }

    public void RestoreHealth(float value)
    {
        amount = 0;
        Display_Text(value.ToString("0"), 'g');
        hitPoints += value;
        while (hitPoints > HP)
        {
            if (number < m_number)
            {
                number++;
                amount++;
                hitPoints -= HP;
            }
            else hitPoints = HP;
        }
        hpBar.SetHealth(hitPoints);
        numT.Display(number);
        if (amount != 0)
            Display_Text(amount.ToString("") + " resurrected", 'w');
    }

    public void GainShield(float value)
    {
        shield += value;
    }

    public void SpendMana(float value)
    {
        mana -= value;
        manaT.text = mana.ToString("0");
        if (character == "soul_collector")
            dark_shroud(value);
    }

    public void LoseEnergy(float value)
    {
        value /= (1F * number * size);
        energy -= value;
        if (energy < 0)
            energy = 0;
        Display_Text("- " + value.ToString("0") + " energy", 'y');
        enBar.SetEnergy(energy);
    }

    public void GainEnergy(float value)
    {
        value /= (1F * number * size);
        energy += value;
        if (energy > 150)
            energy = 150;
        Display_Text("+ " + value.ToString("0") + " energy", 'y');
        enBar.SetEnergy(energy);
    }

    public int GainBuff(float value)
    {
        chance = Random.Range(1, 101);
        value /= number * size;
        if (chance <= value) return 1;
        else return 0;
    }

    public int GainDebuff(float value)
    {
        chance = Random.Range(1, 101);
        value /= number * size;
        if (stats.map.items.collected[19] == true)
            value *= 1.1f;
        if (chance <= value) return 1;
        else return 0;
    }

    public void GainStrength(int value)
    {
        strength += value;
        Display_Text("+ " + value.ToString("0") + " strength", 'r');
        if (character == "bog-thing")
            amalgamation();
    }

    public void GainWeakness(int value)
    {
        strength -= value;
        Display_Text("+ " + value.ToString("0") + " weakness", 'g');
        if (stats.Tree[0].perk[18] > 0)
        {
            GainBleed(stats.Tree[0].perk[18] * size * number);
            LoseEnergy(3 * stats.Tree[0].perk[18] * size * number);
        }
        if (stats.map.items.collected[37] == true)
        {
            fatigue_1 += 7;
            fatigue_2 += 7;
            fatigue_3 += 7;
        }

        if (character == "lord_of_torment")
            costly_power();
        else if (character == "bog-thing")
            amalgamation();
    }

    public void GainResistance(int value)
    {
        resistance += value;
        Display_Text("+ " + value.ToString("0") + " resistance", 's');
        if (character == "bog-thing")
            amalgamation();
    }

    public void GainVoulnerable(int value)
    {
        resistance -= value;
        Display_Text("+ " + value.ToString("0") + " voulnerable", 'p');

        if (character == "lord_of_torment")
            costly_power();
        else if (character == "bog-thing")
            amalgamation();
    }

    public void GainAgility(int value)
    {
        agility += value;
        Display_Text("+ " + value.ToString("0") + " agility", 'b');
        if (character == "bog-thing")
            amalgamation();
    }

    public void GainSlow(int value)
    {
        agility -= value;
        Display_Text("+ " + value.ToString("0") + " slow", 'o');

        if (stats.map.items.collected[63] == true)
        {
            GainPoison(4 * size * number);
            Poison(0.25f);
        }

        if (character == "lord_of_torment")
            costly_power();
        else if (character == "bog-thing")
            amalgamation();
    }

    public void GainBleed(float value)
    {
        value *= 1 + (0.01f * stats.Tree[1].tree1);
        bleed += value;
        Display_Text("+ " + value.ToString("0") + " bleed", 'c');
    }

    public void GainPoison(float value)
    {
        value *= 1 + (0.01f * stats.Tree[2].tree2);
        if (stats.map.items.collected[25] == true)
            value *= 1.12f;
        if (stats.Tree[2].perk[13] > 0)
            LoseEnergy(value);
        poison += value;
        Display_Text("+ " + value.ToString("0") + " poison", 'v');
    }

    public void GainStun()
    {
        stun++;
        Display_Text("Stunned", 'p');

        if (stats.Tree[2].perk[3] > 0)
            LoseEnergy(stats.Tree[2].perk[3] * 8 * size * number);
        if (stats.map.items.collected[35] == true)
            GainVoulnerable(1);

        if (character == "lord_of_torment")
        costly_power();
        else if (character == "bog-thing")
            amalgamation();
    }

    public void GainFreeze(int value)
    {
        freeze += value;
        Display_Text("+ " + value.ToString("0") + " freeze", 'n');

        if (stats.Tree[1].perk[7] > 0)
            TakeDamage(DamageMultiplyer(4 * stats.Tree[1].perk[7] * size * number, 1, 's'), 's');

        if (stats.map.items.collected[44] == true)
            LoseAR(1 + 0.08F * AR);

        if (character == "lord_of_torment")
            costly_power();
        else if (character == "bog-thing")
            amalgamation();
    }

    public void GainAD(float value)
    {
        AD += value;
        Display_Text("+ " + value.ToString("0") + " attack damage", 'r');
    }

    public void GainMP(float value)
    {
        MP += value;
        Display_Text("+ " + value.ToString("0") + " magic power", 'p');
    }

    public void GainIN(float value)
    {
        IN += value;
        Display_Text("+ " + value.ToString("0") + " initiative", 'b');
    }

    public void GainHP(float value)
    {
        HP += value;
        Display_Text("+ " + value.ToString("0") + " hit points", 'g');
        hpBar.SetMaxHealth(HP);
        hpBar.SetHealth(hitPoints);
    }

    public void GainAR(float value)
    {
        AR += value;
        Display_Text("+ " + value.ToString("0") + " armor", 'o');
    }

    public void LoseAD(float value)
    {
        AD -= value;
        Display_Text("- " + value.ToString("0") + " attack damage", 'r');
    }

    public void LoseMP(float value)
    {
        MP -= value;
        Display_Text("- " + value.ToString("0") + " magic power", 'p');
    }

    public void LoseIN(float value)
    {
        IN -= value;
        Display_Text("- " + value.ToString("0") + " initiative", 'b');
    }

    public void LoseHP(float value)
    {
        HP -= value;
        if (hitPoints > HP)
            hitPoints = HP;
        Display_Text("- " + value.ToString("0") + " hit points", 'g');
        hpBar.SetMaxHealth(HP);
        hpBar.SetHealth(hitPoints);
    }

    public void LoseAR(float value)
    {
        AR -= value;
        Display_Text("- " + value.ToString("0") + " armor", 'o');
    }

    public void Set()
    {
        Reset();
        disable();
        enemy.SetActive(true);
        m_number = number;
        if (number <= 0)
        {
            stats.EnemyTakenDown(order);
            enemy.SetActive(false);
        }
        numT.SetMax(m_number);
        numT.Display(number);
        switch (character)
        {
            case "reanimated_soldier":
                AD = 36; MP = 18; IN = 29; HP = 117; AR = 21; size = 1; pen = 0.15f; position = 'm';
                Cost_1 = 43; Cost_2 = 62;
                enemy_im.sprite = sprites[0];
                break;
            case "reanimated_jailer":
                AD = 25; MP = 15; IN = 37; HP = 89; AR = 28; size = 1; pen = 0; position = 'r';
                Cost_1 = 35; Cost_2 = 26;
                enemy_im.sprite = sprites[1];
                break;
            case "soul_collector":
                AD = 40; MP = 39; IN = 34; HP = 158; AR = 32; size = 2; pen = 0; position = 'r';
                Cost_1 = 35; Cost_2 = 46;
                enemy_im.sprite = sprites[3];
                break;
            case "revenant_legionary":
                AD = 158; MP = 65; IN = 36; HP = 536; AR = 73; size = 6; pen = 0; position = 'm';
                Cost_1 = 40; Cost_2 = 50;
                enemy_im.sprite = sprites[4];
                break;
            case "rotfiend":
                AD = 25; MP = 15; IN = 28; HP = 266; AR = 13; size = 1; pen = 0; position = 'm';
                Cost_1 = 33; Cost_2 = 26;
                enemy_im.sprite = sprites[5];
                break;
            case "husk":
                AD = 21; MP = 22; IN = 24; HP = 211; AR = 17; size = 1; pen = 0; position = 'r';
                Cost_1 = 32; Cost_2 = 29;
                enemy_im.sprite = sprites[6];
                break;
            case "ghoul":
                AD = 31; MP = 16; IN = 36; HP = 116; AR = 21; size = 1; pen = 0; position = 'm';
                Cost_1 = 20; Cost_2 = 32;
                enemy_im.sprite = sprites[7];
                break;
            case "shambler":
                AD = 107; MP = 49; IN = 34; HP = 803; AR = 37; size = 5; pen = 0; position = 'm';
                Cost_1 = 40; Cost_2 = 32;
                enemy_im.sprite = sprites[8];
                break;
            case "living_corpse":
                AD = 42; MP = 16; IN = 53; HP = 175; AR = 40; size = 2; pen = 0; position = 'm';
                Cost_1 = 35; Cost_2 = 55;
                enemy_im.sprite = sprites[9];
                fuel_left = 0.22f;
                break;
            case "mad_scientist":
                AD = 24; MP = 21; IN = 39; HP = 124; AR = 11; size = 1; pen = 0; position = 'r';
                Cost_1 = 20; Cost_2 = 36;
                enemy_im.sprite = sprites[10];
                unleash_chance = 20;
                break;
            case "dreadnought":
                AD = 90; MP = 32; IN = 38; HP = 443; AR = 66; size = 4; pen = 0; position = 'm';
                Cost_1 = 40; Cost_2 = 48;
                enemy_im.sprite = sprites[12];
                break;
            case "abomination":
                AD = 183; MP = 66; IN = 35; HP = 898; AR = 53; size = 7; pen = 0; position = 'm';
                Cost_1 = 36; Cost_2 = 40;
                enemy_im.sprite = sprites[13];
                break;
            case "lord_of_torment":
                AD = 374 + 23 * stats.map.floor + 3.1F * stats.map.floor * (stats.map.floor + 1); 
                MP = 202 + 12 * stats.map.floor + 2.2F * stats.map.floor * (stats.map.floor + 1);
                IN = 58 + 2.4F * stats.map.floor;
                HP = 3948 + 329 * stats.map.floor + 13 * stats.map.floor * (stats.map.floor + 1);
                AR = 117 + 7.3F * stats.map.floor + 1.65F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0.2f; position = 'm';
                Cost_1 = 30; Cost_2 = 40; Cost_3 = 52;
                enemy_im.sprite = lord_of_torment;
                boss = true;
                break;
            case "ratking":
                AD = 384 + 24 * stats.map.floor + 3.2F * stats.map.floor * (stats.map.floor + 1);
                MP = 115 + 7.1F * stats.map.floor + 1.6F * stats.map.floor * (stats.map.floor + 1);
                IN = 52 + 2.1F * stats.map.floor;
                HP = 5108 + 425 * stats.map.floor + 15 * stats.map.floor * (stats.map.floor + 1);
                AR = 88 + 5.5F * stats.map.floor + 1.4F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0; position = 'm';
                Cost_1 = 44; Cost_2 = 37; Cost_3 = 44;
                enemy_im.sprite = ratking;
                flesh = 0;
                for (int i = 0; i < 5; i++)
                {
                    chained[i] = 0;
                }
                chain_up(3);
                boss = true;
                break;
            case "glorious_creation":
                AD = 320 + 20 * stats.map.floor + 2.9F * stats.map.floor * (stats.map.floor + 1);
                MP = 157 + 9.8F * stats.map.floor + 1.95F * stats.map.floor * (stats.map.floor + 1);
                IN = 60 + 2.5F * stats.map.floor;
                HP = 4644 + 387 * stats.map.floor + 14 * stats.map.floor * (stats.map.floor + 1);
                AR = 113 + 7 * stats.map.floor + 1.6F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0; position = 'm';
                Cost_1 = 40; Cost_2 = 50; Cost_3 = 55;
                enemy_im.sprite = glorious_creation;
                electric_charges = 30;
                boss = true;
                break;
            case "energy_core":
                AD = 0;
                MP = 30 + 1.8F * stats.map.floor + 0.7F * stats.map.floor * (stats.map.floor + 1);
                IN = 40 + 1.6F * stats.map.floor;
                HP = 700 + 58 * stats.map.floor + 5 * stats.map.floor * (stats.map.floor + 1);
                AR = 80 + 5 * stats.map.floor + 1.3F * stats.map.floor * (stats.map.floor + 1);
                size = 5 + stats.map.floor;
                pen = 0; position = 'r';
                Cost_1 = 100; Cost_2 = 0; Cost_3 = 0;
                enemy_im.sprite = core;
                break;
            case "corrupted_treant":
                AD = 359 + 22 * stats.map.floor + 3.05F * stats.map.floor * (stats.map.floor + 1);
                MP = 170 + 10 * stats.map.floor + 2 * stats.map.floor * (stats.map.floor + 1);
                IN = 55 + 2.2F * stats.map.floor;
                HP = 4252 + 354 * stats.map.floor + 13.5F * stats.map.floor * (stats.map.floor + 1);
                AR = 124 + 7.7F * stats.map.floor + 1.7F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0; position = 'm';
                Cost_1 = 40; Cost_2 = 60; Cost_3 = 70;
                enemy_im.sprite = corrupted_treant;
                boss = true;
                break;
            case "tar_lord":
                AD = 348 + 21 * stats.map.floor + 3 * stats.map.floor * (stats.map.floor + 1);
                MP = 158 + 9.8F * stats.map.floor + 1.95F * stats.map.floor * (stats.map.floor + 1);
                IN = 65 + 2.7F * stats.map.floor;
                HP = 4889 + 407 * stats.map.floor + 14.5F * stats.map.floor * (stats.map.floor + 1);
                AR = 75 + 4.6F * stats.map.floor + 1.25F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0; position = 'm';
                Cost_1 = 32; Cost_2 = 40; Cost_3 = 54;
                enemy_im.sprite = tar_lord;
                boss = true;
                break;
            case "bog-thing":
                AD = 410 + 25 * stats.map.floor + 3.25F * stats.map.floor * (stats.map.floor + 1);
                MP = 150 + 9.3F * stats.map.floor + 1.9F * stats.map.floor * (stats.map.floor + 1);
                IN = 58 + 2.4F * stats.map.floor;
                HP = 4406 + 367 * stats.map.floor + 14 * stats.map.floor * (stats.map.floor + 1);
                AR = 96 + 6 * stats.map.floor + 1.5F * stats.map.floor * (stats.map.floor + 1);
                size = 20 + 2 * stats.map.floor + stats.map.floor * (stats.map.floor + 1) / 2;
                pen = 0; position = 'm';
                Cost_1 = 43; Cost_2 = 60; Cost_3 = 80;
                enemy_im.sprite = bog_thing;
                boss = true;
                break;
            case "mud_pile":
                AD = 1;
                MP = 1;
                IN = 1;
                HP = 400 + 33 * stats.map.floor + 3.8F * stats.map.floor * (stats.map.floor + 1);
                AR = 56 + 3.5F * stats.map.floor + 1.05F * stats.map.floor * (stats.map.floor + 1);
                size = 2;
                pen = 0; position = 'm';
                if (order > 2)
                    position = 'r';
                Cost_1 = 250; Cost_2 = 0; Cost_3 = 0;
                enemy_im.sprite = mud_pile;
                break;
        }

        // zbalansowanie ostatniej walki
        if (boss == true)
        {
            AD *= 0.8f; MP *= 0.75f; IN -= 3; HP *= 0.85f; AR *= 0.75f; size -= 4;
        }
        else
        {
            AD *= Random.Range(0.95f, 1.05f); MP *= Random.Range(0.95f, 1.05f); IN *= Random.Range(0.97f, 1.03f); HP *= Random.Range(0.98f, 1.02f); AR *= Random.Range(0.95f, 1.05f);
        }
        base_AD = AD; base_MP = MP; base_IN = IN; base_HP = HP; base_AR = AR;
        hitPoints = HP; energy = (IN * 0.08F) * Random.Range(1f, 4f); mana = (MP * 0.25F) * Random.Range(1f, 4f);
        fatigue_1 = Random.Range(0f, 15f); fatigue_2 = Random.Range(0f, 15f); fatigue_3 = Random.Range(0f, 15f);

        if (character == "revenant_legionary")
            trapped_souls(5);
        else if (character == "dreadnought")
            iron_casing();

        if (stats.Tree[1].perk[13] > 0)
        {
            GainSlow(1);
            GainFreeze(1);
        }
        if (stats.Tree[2].perk[7] > 0)
        {
            GainPoison((1 + stats.hero.MP * 0.05F) * stats.Tree[2].perk[7]);
        }
        if (stats.map.items.collected[21] == true)
        {
            GainBleed(6 + stats.hero.level * 0.5F);
        }
        if (stats.map.items.collected[26] == true)
        {
            fatigue_1 += 8;
            fatigue_2 += 8;
            fatigue_3 += 8;
        }
        if (stats.map.items.collected[43] == true)
        {
            GainWeakness(3);
            GainAgility(2);
            energy += 8;
        }
        if (stats.map.items.collected[63] == true)
            GainSlow(1);

        hpBar.SetMaxHealth(HP);
        hpBar.SetHealth(hitPoints);
        enBar.SetEnergy(energy);
        manaT.text = mana.ToString("0");
        disable();

        if (stats.map.items.collected[9] == true)
            GainStun();
    }

    void Reset()
    {
        energy = 0; mana = 0;
        Cost_3 = 0;
        strength = 0; resistance = 0; agility = 0; shield = 0; bleed = 0; poison = 0; block = 0; stun = 0; freeze = 0;
    }

    void Update()
    {
        if (que > 0)
            que = 0;

        if (stats.whoseTurn == 0)
        {
            amount = (IN + 25F) * 0.4F * Time.deltaTime;
            if (agility >= 0) 
                amount *= 1 + (0.05F * agility);
            else 
                amount /= 1 + (-0.05F * agility);

            if (freeze > 0)
                amount /= 1 + ((0.15F + 0.01F * stats.Tree[1].perk[10]) * freeze);

            if (character == "dreadnought")
                amount = iron_casing_en(amount);

            amount /= 1 + (0.025F * stats.Tree[2].perk[3]);

            energy += amount;
            enBar.SetEnergy(energy);

            mana += (MP + 20) * 0.2F * Time.deltaTime;
            manaT.text = mana.ToString("0");

            if (energy >= 100)
            {
                if (stun >= 1)
                {
                    energy -= 100;
                    if (stats.map.items.collected[55] == true)
                    {
                        mana -= 12 + mana * 0.16F;
                        if (mana < 0)
                            mana = 0;
                        manaT.text = mana.ToString("0");
                    }
                    stun--;
                }
                else
                {
                    if (freeze > 0)
                    {
                        if (Random.Range(0f, 10f) > stats.Tree[1].perk[10])
                            freeze--;
                    }
                    Action();
                }
            }
        }
    }

    public void Action()
    {
        //if (character == "shambler")
        // to ba added empty_stare();
        if (character == "ratking")
            lambs_to_the_slaughter();
        else if (character == "glorious_creation")
            overcharged_mp();
        else if (character == "corrupted_treant")
            vengeful_roots();
        else if (character == "bog-thing")
            disgusting_mix();

        if (mana >= Cost_1 * (1 + (0.01f * fatigue_1)))
        {
            switch (character)
            {
                case "reanimated_soldier":
                    unholy_shlash();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(unholy_shlash_dmg(), (1 - pen), 'm', target);
                    if (stats.TargetAvailable(target) == true)
                        stats.EnemyActionFlat(unholy_slash_bleed(), "bleed", target);
                    break;
                case "reanimated_jailer":
                    flog();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(flog_dmg(), (1 - pen), 'r', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionDealDmg(flog_mdmg(), 1.0f, 's', target);
                        if (stats.TargetAvailable(target) == true)
                            if (stats.EnemyActionChance(binding_chains(), target) == 1)
                                stats.EnemyActionStatus(1, "slow", target);
                    }
                    //to be added flog_sanity();
                    break;
                case "soul_collector":
                    soul_drain();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(soul_drain_dmg(), (1 - pen), 's', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(soul_drain_MP(), target) == 1)
                        {
                            stats.EnemyActionFlat(2, "mp", target);
                            GainMP(2);
                        }
                    }
                    break;
                case "revenant_legionary":
                    obliterate();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(obliterate_dmg(), (1 - pen), 'm', target);
                    break;
                case "lord_of_torment":
                    crushing_pain();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(crushing_pain_dmg(), (1 - pen), 's', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(crushing_pain_W(), target) == 1)
                        {
                            stats.EnemyActionStatus(1, "weakness", target);
                            stats.EnemyActionDealDmg(share_torment_dmg(stats.ReturnValue("number", target), stats.ReturnValue("size", target)), (1f - pen), 's', target);
                        }
                    }
                    costly_power_deb();
                    break;
                case "rotfiend":
                    tear_flesh();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(tear_flesh_dmg(), (1 - pen), 'm', target);
                    break;
                case "husk":
                    flies_nest();
                    break;
                case "ghoul":
                    only_purpose();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(only_purpose_dmg(), (1 - pen), 'm', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionFlat(bone_claws(stats.ReturnValue("AR", target)), "bleed", target);
                        stats.EnemyActionDealDmg(only_purpose_dmg(), (1 - pen), 'm', target);
                        if (stats.TargetAvailable(target) == true)
                            stats.EnemyActionFlat(bone_claws(stats.ReturnValue("AR", target)), "bleed", target);
                    }
                    break;
                case "shambler":
                    eye_for_en_eye();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(eye_for_en_eye_dmg(), (1 - pen), 's', target);
                    eye_for_en_eye_bleed();
                    break;
                case "living_corpse":
                    blind_rage();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(blind_rage_dmg(), (1 - pen), 'm', target);
                    break;
                case "mad_scientist":
                    toxic_flask();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(toxic_flask_dmg(), (1 - pen), 'r', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionFlat(toxic_flask_poison(), "poison", target);
                        if (stats.EnemyActionChance(toxic_flask_deb(), target) == 1)
                            stats.EnemyActionStatus(1, stats.RandomDebuff(), target);
                    }
                    break;
                case "monstrosity":
                    corrosive_cask();
                    corrosive_cask_att();
                    break;
                case "dreadnought":
                    slam();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(slam_dmg(), (1 - pen), 'm', target);
                    for (int i = 1; i < 6; i++)
                    {
                        if (i != target)
                        {
                            if (stats.TargetAvailable(i) == true)
                            {
                                stats.EnemyActionDealDmg(slam_dmg_others(), (1 - pen), 'm', i);
                            }
                        }
                    }
                    break;
                case "abomination":
                    meat_hook();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(attack_dmg(), (1 - pen), 'r', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionDealDmg(play_time_dmg(), 0.75f, position, target);
                        if (stats.TargetAvailable(target) == true)
                        {
                            stats.EnemyActionFlat(meat_hook_bleed(), "bleed", target);
                            if (stats.EnemyActionChance(meat_hook_stun(), target) == 1)
                                stats.EnemyActionStatus(1, "stun", target);
                        }
                    }
                    break;
                case "ratking":
                    pulverize();
                    target = stats.EnemyTarget('m');
                    amountt = stats.EnemyActionDmg(pulverize_dmg(), (1 - pen), 'm', target);
                    stats.EnemyActionDealDmg(pulverize_dmg(), (1 - pen), 'm', target);
                    more_flesh(amountt);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(pulverize_S(), target) == 1)
                        {
                            stats.EnemyActionStatus(1, "stun", target);
                            stats.EnemyActionFlat(40F, "energy", target);
                        }
                    }
                    break;
                case "glorious_creation":
                    chain_lighting();
                    for (int i = 0; i < 3; i++)
                    {
                        target = stats.EnemyTarget('r');
                        stats.EnemyActionDealDmg(chain_lighting_dmg(), 1, 's', target);
                    }
                    break;
                case "energy_core":
                    electric_surge();
                    break;
                case "corrupted_treant":
                    bramble_smash();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(bramble_smash_dmg(), (1 - pen), 'm', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(bramble_smash_S(), target) == 1)
                        {
                            stats.EnemyActionStatus(1, "stun", target);
                            stats.EnemyActionStatus(1, "slow", target);
                        }
                    }
                    break;
                case "tar_lord":
                    drown();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(drown_dmg(), (1 - pen), 's', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(drown_S(), target) == 1)
                        {
                            stats.EnemyActionStatus(1, "stun", target);
                            stats.EnemyActionStatus(1, "slow", target);
                            stats.EnemyActionDealDmg(trapped_in_tar_dmg(), (1 - pen), 's', target);
                        }
                        if (stats.TargetAvailable(target) == true)
                        {
                            if (stats.EnemyActionChance(drown_S(), target) == 1)
                            {
                                stats.EnemyActionStatus(1, "slow", target);
                                stats.EnemyActionDealDmg(trapped_in_tar_dmg(), (1 - pen), 's', target);
                            }
                        }
                    }
                    break;
                case "bog-thing":
                    smash();
                    smash_att();
                    break;
                case "mud_pile":
                    it_lives();
                    break;
            }
        }
        else if ((Cost_2 != 0) && (mana >= Cost_2 * (1 + (0.01f * fatigue_2))))
        {
            switch (character)
            {
                case "reanimated_soldier":
                    might_of_the_grave();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(might_of_the_grave_dmg(), (1 - pen), 'm', target);
                    break;
                case "reanimated_jailer":
                    target = stats.EnemyTarget('r');
                    chain_lash(stats.ReturnValue("agility", target));
                    stats.EnemyActionDealDmg(chain_lash_dmg(), (1 - pen), 'r', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionFlat(chain_lash_en(stats.ReturnValue("agility", target)), "energy", target);
                        if (stats.EnemyActionChance(binding_chains(), target) == 1)
                            stats.EnemyActionStatus(1, "slow", target);
                    }
                    break;
                case "soul_collector":
                    piercing_pain();
                    stats.EnemyActionDealDmg(piercing_pain_dmg(), (1 - pen - piercing_pain_pen()), 's', target);
                    break;
                case "revenant_legionary":
                    death_sentance();
                    target = stats.EnemyTarget('r');
                    if (stats.EnemyActionChance(death_sentance_V(), target) == 1)
                        stats.EnemyActionStatus(2, "vulnerable", target);
                    stats.EnemyActionDealDmg(death_sentance_dmg(stats.ReturnValue("resistance", target)), (0.4f - pen), 's', target);
                    break;
                case "lord_of_torment":
                    anguish();
                    anguish_att();
                    costly_power_deb();
                    break;
                case "rotfiend":
                    decay();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(decay_dmg(), 1.0f, 's', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(decay_deb(), target) == 1)
                            stats.EnemyActionStatus(1, "vulnerable", target);
                        if (stats.EnemyActionChance(decay_deb(), target) == 1)
                            stats.EnemyActionStatus(1, "slow", target);
                    }
                    //to be added decay_sanity();
                    break;
                case "husk":
                    haunting_wail();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(haunting_wail_dmg(), 1.0f, 's', target);
                    // to be added haunting_wail_sanity();
                    break;
                case "ghoul":
                    cannibalize();
                    target = stats.EnemyTarget('m');
                    amountt = stats.EnemyActionDmg(cannibalize_dmg(), (1 - pen), 'm', target);
                    stats.EnemyActionDealDmg(cannibalize_dmg(), (1 - pen), 'm', target);
                    cannibalize_restore(amountt);
                    if (stats.TargetAvailable(target) == true)
                        stats.EnemyActionFlat(bone_claws(stats.ReturnValue("AR", target)), "bleed", target);
                    break;
                case "shambler":
                    no_regrets();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(no_regrets_dmg(), (1 - pen), 'm', target);
                    break;
                case "living_corpse":
                    running_on_fumes();
                    break;
                case "mad_scientist":
                    experimental_concoction();
                    break;
                case "monstrosity":
                    purge();
                    purge_att();
                    break;
                case "dreadnought":
                    grab();
                    target = stats.EnemyTarget('r');
                    grab_en(target);
                    stats.EnemyActionDealDmg(grab_dmg(), (1 - pen), 'r', target);
                    if (stats.TargetAvailable(target) == true)
                    {
                        stats.EnemyActionStatus(1, "stun", target);
                        stats.EnemyActionStatus(1, "slow", target);
                    }
                    break;
                case "abomination":
                    target = stats.EnemyTarget('r');
                    gorge(stats.ReturnValue("size", target), stats.ReturnValue("AD", target), stats.ReturnValue("HP", target), stats.ReturnValue("AR", target));
                    stats.Kill(number, target);
                    break;
                case "ratking":
                    new_playthings();
                    target = stats.EnemyTarget('r');
                    amountt = stats.EnemyActionDmg(new_playthings_dmg(), (1 - pen), 's', target);
                    stats.EnemyActionDealDmg(new_playthings_dmg(), (1 - pen), 's', target);
                    more_flesh(amountt);
                    chained[target - 1]++;
                    if (total_attacks >= 8)
                    {
                        for (int i = 7; i < total_attacks; i += 8)
                        {
                            chain_up(1);
                        }
                    }
                    new_playthings_buff();
                    if (stats.TargetAvailable(target) == true)
                    {
                        if (stats.EnemyActionChance(new_playthings_S(), target) == 1)
                            stats.EnemyActionStatus(1, "slow", target);
                        if (stats.EnemyActionChance(new_playthings_S(), target) == 1)
                            stats.EnemyActionStatus(1, "slow", target);
                    }
                    break;
                case "glorious_creation":
                    battery_drain();
                    break;
                case "corrupted_treant":
                    regrowth();
                    break;
                case "tar_lord":
                    combust();
                    for (int i = 0; i < 6; i++)
                    {
                        target = stats.EnemyTarget('r');
                        stats.EnemyActionDealDmg(combust_dmg(), (1 - pen), 's', target);
                    }
                    break;
                case "bog-thing":
                    assimilate();
                    break;
            }
        }
        else if ((Cost_3 != 0) && (mana >= Cost_3 * (1 + (0.01f * fatigue_3))))
        {
            switch (character)
            {
                case "lord_of_torment":
                    crippling_fear();
                    crippling_fear_att();
                    costly_power_deb();
                    break;
                case "ratking":
                    chop_chop();
                    target = stats.EnemyTarget('m');
                    amountt = stats.EnemyActionDmg(chop_chop_dmg(), (1 - pen), 's', target) * 2;
                    stats.EnemyActionDealDmg(chop_chop_dmg(), (1 - pen), 'm', target);
                    more_flesh(amountt);
                    break;
                case "glorious_creation":
                    power_overwhelming();
                    target = stats.EnemyTarget('m');
                    stats.EnemyActionDealDmg(power_overwhelming_dmg(), (1 - pen), 'm', target);
                    break;
                case "corrupted_treant":
                    cursed_existance();
                    for (int i = 0; i < cursed_targets; i++)
                    {
                        target = stats.EnemyTarget('r');
                        stats.EnemyActionDealDmg(cursed_existance_dmg(), (1 - pen), 's', target);
                        if (stats.TargetAvailable(target) == true)
                        {
                            stats.EnemyActionFlat(cursed_existance_poison(), "poison", target);
                            if (stats.EnemyActionChance(cursed_existance_W(), target) == 1)
                            {
                                stats.EnemyActionStatus(1, "weakness", target);
                                stats.EnemyActionStatus(1, "slow", target);
                            }
                        }
                    }
                    cursed_targets++;
                    break;
                case "tar_lord":
                    no_escape();
                    target = stats.EnemyTarget('r');
                    stats.EnemyActionDealDmg(no_escape_dmg(), (1 - pen), 's', target);
                    for (int i = 1; i < 6; i++)
                    {
                        if (stats.TargetAvailable(i) == true)
                        {
                            if (stats.ReturnValue("attacks", i) == 0)
                            {
                                stats.EnemyActionDealDmg(no_escape_dmg_ot(), (1 - pen), 's', i);
                                if (stats.TargetAvailable(i) == true)
                                {
                                    if (stats.EnemyActionChance(no_escape_S(), i) == 1)
                                    {
                                        stats.EnemyActionStatus(1, "slow", i);
                                        stats.EnemyActionDealDmg(trapped_in_tar_dmg(), (1 - pen), 's', i);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "bog-thing":
                    dissolving_ooze();
                    dissolving_ooze_att();
                    break;
            }
        }
        else
        {
            if (character == "energy_core")
                charge_up();
            else if (character == "mud_pile" && size < 5)
                dead();
            else
            {
                attack();
                target = stats.EnemyTarget(position);
                amountt = stats.EnemyActionDmg(attack_dmg(), (1 - pen), position, target);
                stats.EnemyActionDealDmg(attack_dmg(), (1 - pen), position, target);
                if (stats.TargetAvailable(target) == true)
                {
                    switch (character)
                    {
                        case "reanimated_jailer":
                            if (stats.EnemyActionChance(binding_chains(), target) == 1)
                                stats.EnemyActionStatus(1, "slow", target);
                            break;
                        case "ghoul":
                            stats.EnemyActionFlat(bone_claws(stats.ReturnValue("AR", target)), "bleed", target);
                            break;
                        case "abomination":
                            stats.EnemyActionDealDmg(play_time_dmg(), 0.75f, position, target);
                            break;
                    }
                }
                if (character == "ratking")
                    more_flesh(amountt);
            }
        }
    }

    public void NewTurn()
    {
        fatigue_1 -= 4 + (fatigue_1 / 25);
        if (fatigue_1 <= 0) fatigue_1 = 0;

        fatigue_2 -= 4 + (fatigue_2 / 25);
        if (fatigue_2 <= 0) fatigue_2 = 0;

        fatigue_3 -= 4 + (fatigue_3 / 25);
        if (fatigue_3 <= 0) fatigue_3 = 0;

        if (character == "rotfiend")
            rot();
        else if (character == "husk")
            fly_swarm();
        else if (character == "living_corpse")
            lose_fuel(0.05f);
        else if (character == "mad_scientist")
            monster_within();
        else if (character == "abomination")
        {
            vile_gas();
            play_time();
        }
        else if (character == "ratking")
            stitched_up();
        else if (character == "tar_lord")
            intangible_body_hp();

        if (bleed > 0) Bleed(1f);
        if (poison > 0) Poison(1f);

        attacked = false;
        attacks = 0;

        if (stats.Tree[1].perk[7] > 0)
            if (freeze > 0)
                TakeDamage(DamageMultiplyer(4 * freeze * stats.Tree[1].perk[7] * size * number, 1, 's'), 's');
        if (stats.map.items.collected[18] == true)
        {
            TakeDamage(DamageMultiplyer(stats.hero.Bottled_Light(), 1.0F, 's'), 's');
        }
        if (stats.map.items.collected[57] == true)
            TakeDamage(HP * 0.07F * number, 'r');
    }

    public void Bleed(float efficiency)
    {
        amount = DamageMultiplyer(bleed, (1 - 0.06f * stats.Tree[1].perk[1]), 'm');
        if (stats.map.items.collected[46] == true)
        {
            if (attacks < 4)
                amount *= 1.2F - 0.05F * attacks;
        }
        TakeDamage(amount, 'r');
        if (stats.Tree[1].perk[6] > 0)
            GainBleed(amount * 0.2f);
    }

    public void Poison(float efficiency)
    {
        TakeDamage(DamageMultiplyer(poison * (1 + (0.1f + stats.hero.MP * 0.0012F) * stats.Tree[2].perk[13]), 0, 's'), 'p');
    }

    public void enable()
    {
        button.enabled = true;
        field.red();
    }

    public void disable()
    {
        button.enabled = false;
        field.normal();
    }

    public void Display_Text(string text_to_display, char color_of_text)
    {
        if (que == 0)
            Fire_Text(text_to_display, color_of_text);
        else
        {
            coroutine = Wait_Display(text_to_display, color_of_text, que * 0.2f);
            StartCoroutine(coroutine);
        }
        que++;
    }

    private IEnumerator Wait_Display(string text_to_display, char color_of_text, float delay)
    {
        yield return new WaitForSeconds(delay);
        Fire_Text(text_to_display, color_of_text);
    }

    private void Fire_Text(string text_to_display, char color_of_text)
    {
        Enemy_bod.rotation = Quaternion.Euler(Enemy_bod.rotation.x, Enemy_bod.rotation.y, Random.Range(-16f, 16f));
        GameObject text = Instantiate(Text_prefab, Enemy_bod.position, Enemy_bod.rotation);
        Rigidbody2D text_body = text.GetComponent<Rigidbody2D>();
        Transform text_transform = text.GetComponent<Transform>();
        text_transform.rotation = Quaternion.Euler(0, 0, 0);
        text.GetComponent<TextMeshPro>().text = text_to_display;
        switch (color_of_text)
        {
            case 'p':
                text.GetComponent<TextMeshPro>().color = new Color(0.4F, 0.2F, 0.9f, 1);
                break;
            case 'r':
                text.GetComponent<TextMeshPro>().color = new Color(1, 0, 0, 1);
                break;
            case 'g':
                text.GetComponent<TextMeshPro>().color = new Color(0, 0.7F, 0.1F, 1);
                break;
            case 'y':
                text.GetComponent<TextMeshPro>().color = new Color(1, 1, 0, 1);
                break;
            case 's':
                text.GetComponent<TextMeshPro>().color = new Color(0.6f, 0.6f, 0.6f, 1);
                break;
            case 'b':
                text.GetComponent<TextMeshPro>().color = new Color(0, 0.2f, 1f, 1);
                break;
            case 'o':
                text.GetComponent<TextMeshPro>().color = new Color(1, 0.5f, 0.2f, 1);
                break;
            case 'c':
                text.GetComponent<TextMeshPro>().color = new Color(0.5f, 0.1f, 0.1f, 1);
                break;
            case 'v':
                text.GetComponent<TextMeshPro>().color = new Color(0.4f, 0.65f, 0.1f, 1);
                break;
            case 'a':
                text.GetComponent<TextMeshPro>().color = new Color(0.25f, 0.7f, 0.65f, 1);
                break;
            case 'z':
                text.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 1);
                break;
            case 'w':
                text.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                break;
        }
        text_body.AddForce(Enemy_bod.up * Random.Range(10, 14), ForceMode2D.Impulse + 200);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        info.Detail_Hud.SetActive(true);
        info.details[0].text = size.ToString("0");
        info.details[1].text = IN.ToString("0");
        info.details[2].text = AD.ToString("0");
        info.details[3].text = MP.ToString("0");
        info.details[4].text = HP.ToString("0");
        info.details[5].text = AR.ToString("0");

        if (strength == 0)
        {
            info.details[6].text = "-";
            info.details[6].color = new Color(1, 1, 1, 1);
        }
        else if (strength > 0)
        {
            info.details[6].text = strength.ToString("0");
            info.details[6].color = new Color(0, 0.7F, 1, 1);
        }
        else if (strength < 0)
        {
            info.details[6].text = strength.ToString("0");
            info.details[6].color = new Color(1, 0, 0, 1);
        }

        if (resistance == 0)
        {
            info.details[7].text = "-";
            info.details[7].color = new Color(1, 1, 1, 1);
        }
        else if (resistance > 0)
        {
            info.details[7].text = resistance.ToString("0");
            info.details[7].color = new Color(0, 0.7F, 1, 1);
        }
        else if (resistance < 0)
        {
            info.details[7].text = resistance.ToString("0");
            info.details[7].color = new Color(1, 0, 0, 1);
        }

        if (agility == 0)
        {
            info.details[8].text = "-";
            info.details[8].color = new Color(1, 1, 1, 1);
        }
        else if (agility > 0)
        {
            info.details[8].text = agility.ToString("0");
            info.details[8].color = new Color(0, 0.8F, 1, 1);
        }
        else if (agility < 0)
        {
            info.details[8].text = agility.ToString("0");
            info.details[8].color = new Color(1, 0, 0, 1);
        }

        info.details[9].text = shield.ToString("0");
        info.details[10].text = bleed.ToString("0");
        info.details[11].text = poison.ToString("0");
        info.details[12].text = block.ToString("0");
        info.details[13].text = stun.ToString("0");
        info.details[14].text = freeze.ToString("0");
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        info.Detail_Hud.SetActive(false);
    }
}