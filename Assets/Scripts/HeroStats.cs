using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HeroStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject unit;
    public Image hero_im;
    public Sprite Light, Water, Nature, Blood, grave;
    public Button target;
    public field_a field;
    public AllStats stats;
    public HealthBar hpBar;
    public EnergyBar enBar, powBar;
    public TMPro.TextMeshProUGUI manaT;

    public Transform Unit_bod;
    public GameObject Text_prefab;
    private IEnumerator coroutine;
    private int que = 0;
    public UnitDetailsText info;

    public float AD, MP, IN, HP, AR;
    public float hitPoints, shield, energy, mana, bleed, poison, pen = 1;
    public float Cost_1, Cost_2, Cost_3, Cost_4, Cost_5, Cost_6, Cost_7;
    public float fatigue_1, fatigue_2, fatigue_3, fatigue_4, fatigue_5, fatigue_6, fatigue_7;
    public int level, size, strength, resistance, agility, block, stun, freeze;
    public float amount, chance;
    public float mana_spent;
    public float AD_s, MP_s, IN_s, HP_s, AR_s;
    public float base_AD, base_MP, base_IN, base_HP, base_AR;

    // Light Specific
    int valor;
    public int justice;

    // Water Specific
    int haste;
    public int trident_bonus;

    // Nature Specific
    int blossom;
    public float barkskin_charge;
    public float beast_within_charge;

    // Blood Specific
    int rage;
    public float blood_stacks;
    public int attacks, total_attacks;
    public bool inmortal;

    public void attack()
    {
        energy -= 100;
        if (stats.whichHero == 'w')
            GainHaste(Random.Range(5, 9));
        enBar.SetEnergy(energy);
        advance(1);
    }

    public void advance(int count)
    {
        for (int i = 0; i < count; i++)
        {
            attacks++;
            total_attacks++;
            beast_within_charge += 0.03F * stats.Tree[2].perk[19];
            ReduceFatigue(1 * stats.Tree[3].perk[5] * count, 0.01F * stats.Tree[3].perk[5]);
            energy += 2 * stats.Tree[3].perk[7];
            enBar.SetEnergy(energy);
            if (stats.Tree[3].perk[7] > 0)
                GainRage(2 * stats.Tree[3].perk[7]);
            if (stats.whichHero == 'b')
                GainRage(Mathf.RoundToInt(5 + 5 * (AD - base_AD) / base_AD));
            if (stats.Tree[3].perk[12] > 0)
            {
                if (total_attacks % 5 == 0)
                {
                    GainRage(Mathf.RoundToInt(Random.Range((1 + total_attacks * 0.1F) * stats.Tree[3].perk[12], 1 + (3 + total_attacks * 0.1F) * stats.Tree[3].perk[12])));
                    blood_well((22 + total_attacks * 0.5F) * stats.Tree[3].perk[12]);
                }
            }
            stats.ally_attacks++;
            if (stats.map.items.collected[40] == true)
            {
                if (stats.ally_attacks % 16 == 0)
                    stats.RandomBuffA();
            }
        }
    }

    public float attack_dmg()
    {
        amount = AD;
        amount = Damageincrease(amount);
        return amount;
    }

    public float beast_within_dmg()
    {
        amount = (0.12F * (AD * 2 - base_AD)) + (0.41F * MP);
        amount += (0.08F * (AD - base_AD) + 0.14F * (IN - base_IN)) * stats.Tree[2].perk[19];
        amount *= 1 + (0.01F * stats.Tree[2].tree3);
        amount += AD;
        energy += 25 + 1 * stats.Tree[2].tree3;
        enBar.SetEnergy(energy);
        beast_within_charge--;
        amount = Damageincrease(amount);
        return amount;
    }

    // Light Hero Skillset
    public void GainValor(int value)
    {
        valor += value;
        if (valor >= 100)
            Valor();
        powBar.SetEnergy(valor);
    }

    public void Valor()
    {
        valor -= 100;
        if (stats.map.items.collected[58] == true)
        {
            valor += 6;
            if (stats.map.items.collected[32] == true)
                valor += 3;
            if (stats.map.items.collected[33] == true)
                valor += 3;
            if (stats.map.items.collected[34] == true)
                valor += 3;
        }
        if (stats.map.items.collected[60] == true)
            GainResistance(1);
        block++;
        GainShield(5 + 0.01F * HP);
    }

    public float crushing_blows_W()
    {
        amount = (40 + (0.73F * AD)) * size;
        amount *= 1 + (0.02f * stats.Tree[0].tree2);
        return amount;
    }

    public float crushing_blows_V()
    {
        amount = (80 + (1.3F * AR)) * size;
        amount *= 1 + (0.02f * stats.Tree[0].tree3);
        return amount;
    }

    public void resilience()
    {
        if (hitPoints <= HP * 0.4F)
        {
            RestoreHealth(((HP * 2) - base_HP) * 0.015f);
            GainValor(6);
        }
        else RestoreHealth(((HP * 2) - base_HP) * 0.01f);
    }

    public void crippling_strike()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 48;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_1 * fatigue_1 / 2000, Cost_1 * fatigue_1 / 1500)));
            fatigue_1 *= 0.9F;
            Cost_1 *= 0.98f;
        }
        advance(1);
    }

    public float crippling_strike_dmg()
    {
        amount = 28 + (1.2F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float crippling_strike_bleed()
    {
        return 7.2f + (0.12F * AD);
    }

    public void smite()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 42;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_2 * fatigue_2 / 2000, Cost_2 * fatigue_2 / 1500)));
            fatigue_2 *= 0.9F;
            Cost_2 *= 0.98f;
        }
        advance(1);
    }

    public float smite_dmg()
    {
        amount = 32 + (0.53F * AD) + (2.62F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float smite_S()
    {
        return (100 + (1.54F * MP)) * size;
    }

    public float smite_D(int value)
    {
        amount = (44 + 10 / 3 * level) * size;
        if (value > 0)
            amount *= value;
        return amount;
    }

    public void bulwark_of_light()
    {
        energy -= 74;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 48;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_3 * fatigue_3 / 2000, Cost_3 * fatigue_3 / 1500)));
            fatigue_3 *= 0.9F;
            Cost_3 *= 0.98f;
        }
        GainShield(37 + (0.05F * HP) + (0.38F * AR));
    }

    public float bulwark_of_light_sh()
    {
        amount = 18 + (0.69F * MP) + (0.27F * AR);
        return amount;
    }

    public void lights_chosen()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_4 * (1 + (0.01F * fatigue_4)));
        fatigue_4 += 56;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_4 * fatigue_4 / 2000, Cost_4 * fatigue_4 / 1500)));
            fatigue_4 *= 0.9F;
            Cost_4 *= 0.98f;
        }
    }

    public float lights_chosen_S()
    {
        return (147 + (2.29F * MP)) * size;
    }

    public float lights_chosen_R()
    {
        return (105 + (1.51F * MP) + (0.63F * AR)) * size;
    }

    public void avenging_wrath()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_5 * (1 + (0.01F * fatigue_5)));
        fatigue_5 += 40;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_5 * fatigue_5 / 2000, Cost_5 * fatigue_5 / 1500)));
            fatigue_5 *= 0.9F;
            Cost_5 *= 0.98f;
        }
        advance(1);
    }

    public float avenging_wrath_ticks()
    {
        amount = 6 + (stats.enemy_attacks / 6);
        return amount;
    }

    public float avenging_wrath_dmg()
    {
        amount = 16 + (0.35F * AD) + (2.22F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public void war_cry()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_6 * (1 + (0.01F * fatigue_6)));
        fatigue_6 += 66;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_6 * fatigue_6 / 2000, Cost_6 * fatigue_6 / 1500)));
            fatigue_6 *= 0.9F;
            Cost_6 *= 0.98f;
        }
        GainMP(0.6f + 0.1f * level);
    }

    public float war_cry_ad()
    {
        return (1.8F + 0.01F * AD) * 0.01F;
    }

    public float war_cry_en()
    {
        return (12 * size);
    }

    public void righteous_hammer()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_7 * (1 + (0.01F * fatigue_7)));
        fatigue_7 += 70;
        if (stats.map.items.collected[53] == true)
        {
            GainValor(Mathf.RoundToInt(Random.Range(Cost_7 * fatigue_7 / 2000, Cost_7 * fatigue_7 / 1500)));
            fatigue_7 *= 0.9F;
            Cost_7 *= 0.98f;
        }
        advance(1);
    }

    public float righterous_hammer_dmg()
    {
        amount = 42 + (0.95F * AD) + (0.11F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float righterous_hammer_stun()
    {
        amount = (83 + (0.29F * AD) + (3.33F * MP)) * size;
        if (strength > 0) 
            amount *= (1 + (0.03F * strength));
        return amount;
    }

    // Water Hero Skillset
    public void GainHaste(int value)
    {
        haste += value;
        if (haste >= 100)
            Haste();
        powBar.SetEnergy(haste);
    }

    public void Haste()
    {
        haste -= 100;
        if (stats.map.items.collected[58] == true)
        {
            haste += 6;
            if (stats.map.items.collected[32] == true)
                haste += 3;
            if (stats.map.items.collected[33] == true)
                haste += 3;
            if (stats.map.items.collected[34] == true)
                haste += 3;
        }
        if (agility > 0)
            energy += 30 + 3 * agility;
        else
            energy += 30;
        if (stats.map.items.collected[45] == true)
            GainShield(7 + (IN - base_IN));
        enBar.SetEnergy(energy);
        ReduceFatigue(6, 0);
    }

    public float charged_strikes_dmg()
    {
        if (trident_bonus < 16)
        {
            gain_charge(1);
        }
        amount = 4.4F + (0.033F * AD);
        amount += (1.2F + (0.02f * MP)) * stats.Tree[1].perk[9];
        if (stats.map.items.collected[23] == true)
        {
            amount += (1 + 0.1F * level) * trident_bonus;
        }
        amount = Damageincrease(amount);
        amount *= 1 + ((0.05f + 0.01f * stats.Tree[1].perk[0]) * trident_bonus);
        return amount;
    }

    public float charged_strikes_pen()
    {
        amount = 1 - 0.07f * stats.Tree[1].perk[9];
        return amount;
    }

    public void gain_charge(int value)
    {
        trident_bonus += value;
        if (trident_bonus == 16)
        {
            if (stats.Tree[1].perk[20] > 0)
            {
                GainResistance(1);
                GainAgility(1);
                GainShield(40 + 0.6F * AR);
                if (stats.unit1_alive == true)
                {
                    stats.unit1.GainResistance(1);
                    stats.unit1.GainAgility(1);
                    stats.unit1.GainShield(40 + 0.6F * AR);
                }
                if (stats.unit2_alive == true)
                {
                    stats.unit2.GainResistance(1);
                    stats.unit2.GainAgility(1);
                    stats.unit2.GainShield(40 + 0.6F * AR);
                }
                if (stats.unit3_alive == true)
                {
                    stats.unit3.GainResistance(1);
                    stats.unit3.GainAgility(1);
                    stats.unit3.GainShield(40 + 0.6F * AR);
                }
                if (stats.unit4_alive == true)
                {
                    stats.unit4.GainResistance(1);
                    stats.unit4.GainAgility(1);
                    stats.unit4.GainShield(40 + 0.6F * AR);
                }
            }
            if (level >= 14)
                stats.Lighting();
        }
        GainIN((0.5F + 0.15F * stats.Tree[1].perk[4]) * value);
    }

    public void flow_like_water(float value)
    {
        amount = (0.12F + (0.0012F * IN));
        amount *= 1 + (0.02f * stats.Tree[1].tree2);
        energy += value * amount;
        enBar.SetEnergy(energy);
    }

    public void impale()
    {
        energy -= 80;
        GainHaste(Random.Range(4, 7));
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 50;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_1 * fatigue_1 / 2000, Cost_1 * fatigue_1 / 1500)));
            fatigue_1 *= 0.9F;
            Cost_1 *= 0.98f;
        }
        advance(1);
    }

    public float impale_dmg()
    {
        amount = 38 + (1.4F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float impale_stun(float value)
    {
        return (57 + (0.34F * value)) * size;
    }

    public void rupture()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 40;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_2 * fatigue_2 / 2000, Cost_2 * fatigue_2 / 1500)));
            fatigue_2 *= 0.9F;
            Cost_2 *= 0.98f;
        }
        advance(1);
    }

    public float rupture_dmg()
    {
        amount = 10 + (1.21F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float rupture_bleed()
    {
        return 4.5f + (0.04F * AD);
    }

    public float rupture_m_dmg(float value)
    {
        amount = 0.4F + (0.68F * MP) + (0.45F * value);
        amount = Damageincrease(amount);
        return amount;
    }

    public void deep_freeze()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 62;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_3 * fatigue_3 / 2000, Cost_3 * fatigue_3 / 1500)));
            fatigue_3 *= 0.9F;
            Cost_3 *= 0.98f;
        }
        advance(1);
    }

    public float deep_freeze_dmg()
    {
        amount = 17 + (0.28F * AD) + (1.07F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float deep_freeze_F()
    {
        return (50 + (1.45F * MP)) * size;
    }

    public void bouncing_blade()
    {
        energy -= 90;
        GainHaste(Random.Range(5, 8));
        SpendMana(Cost_4 * (1 + (0.01F * fatigue_4)));
        fatigue_4 += 54;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_4 * fatigue_4 / 2000, Cost_4 * fatigue_4 / 1500)));
            fatigue_4 *= 0.9F;
            Cost_4 *= 0.98f;
        }
        advance(1);
    }

    public float bouncing_blade_dmg()
    {
        amount = 4 + (0.45F * AD) + (0.14F * IN);
        amount = Damageincrease(amount);
        return amount;
    }

    public float bouncing_blade_bleed()
    {
        return 2.5F + (0.047F * AD);
    }

    public float bouncing_blade_bounces()
    {
        amount = 2 + ((AD - base_AD) / 30) + ((IN - base_IN) / 10);
        return amount;
    }

    public void rend()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        SpendMana(Cost_5 * (1 + (0.01F * fatigue_5)));
        fatigue_5 += 63;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_5 * fatigue_5 / 2000, Cost_5 * fatigue_5 / 1500)));
            fatigue_5 *= 0.9F;
            Cost_5 *= 0.98f;
        }
        advance(1);
    }

    public float rend_ticks()
    {
        amount = 5 + stats.Tree[1].perk[3] + ((IN - base_IN) / 5);
        return amount;
    }

    public float rend_dmg()
    {
        amount = 5.3F + 0.3f * level;
        amount += (2 + 0.03F * AR) * stats.Tree[1].perk[17];
        GainShield(amount);
        amount = 5.4F + (0.22F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public void tidal_wave()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        SpendMana(Cost_6 * (1 + (0.01F * fatigue_6)));
        fatigue_6 += 63;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_6 * fatigue_6 / 2000, Cost_6 * fatigue_6 / 1500)));
            fatigue_6 *= 0.9F;
            Cost_6 *= 0.98f;
        }
        advance(1);
    }

    public float tidal_wave_dmg()
    {
        amount = 19 + (0.3F * AD) + (0.77F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float tidal_wave_S()
    {
        return (45 + (2.26f * MP)) * size;
    }

    public float tidal_wave_en()
    {
        return (3 + (0.61F * MP)) * size;
    }

    public void maelstrom()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        SpendMana(Cost_7 * (1 + (0.01F * fatigue_7)));
        fatigue_7 += 70;
        if (stats.map.items.collected[53] == true)
        {
            GainHaste(Mathf.RoundToInt(Random.Range(Cost_7 * fatigue_7 / 2000, Cost_7 * fatigue_7 / 1500)));
            fatigue_7 *= 0.9F;
            Cost_7 *= 0.98f;
        }
        advance(1);
    }

    public float maelstrom_dmg()
    {
        amount = 19 + (0.65F * MP) + (0.31F * IN);
        amount = Damageincrease(amount);
        return amount;
    }

    public float maelstrom_en()
    {
        amount = 9 + 0.04F * MP;
        amount *= size;
        return amount;
    }

    public float lightning_strike_dmg()
    {
        amount = 57 + (0.6F * AD) + (2.7F * MP);
        if (stats.map.items.collected[61] == true)
            amount *= 1.1F + 0.008F * (IN - base_IN);
        amount = Damageincrease(amount);
        return amount;
    }

    public float lightning_strike_stun()
    {
        return 250 * size;
    }

    // Nature Hero Skillset
    void GainBlossom(int value)
    {
        blossom += value;
        if (blossom >= 100)
            Blossom();
        powBar.SetEnergy(blossom);
    }

    public void Blossom()
    {
        blossom -= 100;
        if (stats.map.items.collected[58] == true)
        {
            blossom += 6;
            if (stats.map.items.collected[32] == true)
                blossom += 3;
            if (stats.map.items.collected[33] == true)
                blossom += 3;
            if (stats.map.items.collected[34] == true)
                blossom += 3;
        }
        if (stats.map.items.collected[64] == true)
            GainMP(1.2F + 0.005F * MP);
        mana += (20 + 0.2F * MP);
        RestoreHealth(10 + 0.01F * HP);
        if (stats.Tree[2].perk[6] > 0)
            stats.Forest_Wrath();
        if (stats.Tree[2].perk[12] > 0)
            expunge(0.05F);
    }

    public void barkskin(float value)
    {
        barkskin_charge += value;
        if (barkskin_charge >= 150)
        {
            GainAR(0.8f + 0.1f * level);
            if (stats.Tree[2].perk[18] > 0)
                GainAD(0.4f + 0.05f * level * stats.Tree[2].perk[18]);
            amount = 3.4F + (0.21f * MP) + (0.013F * HP);
            amount += (0.6F + 0.01F * AD) * stats.Tree[2].perk[18];
            amount *= 1 + 0.04F * stats.Tree[2].perk[9];
            GainShield(amount);
            barkskin_charge -= 150;
        }
    }

    public void entangling_roots()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 + (1 + (0.01F * fatigue_1)));
        fatigue_1 += 52;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_1 * fatigue_1 / 2000, Cost_1 * fatigue_1 / 1500)));
            fatigue_1 *= 0.9F;
            Cost_1 *= 0.98f;
        }
        advance(1);
        if (stats.Tree[2].perk[5] > 0)
            Cost_1 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float entangling_roots_dmg()
    {
        amount = 39 + (0.47F * AD) + (2.57F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float entangling_roots_S()
    {
        return (80 + (1.6F * MP)) * size;
    }

    public void feral_rage()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 + (1 + (0.01F * fatigue_2)));
        fatigue_2 += 44;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_2 * fatigue_2 / 2000, Cost_2 * fatigue_2 / 1500)));
            fatigue_2 *= 0.9F;
            Cost_2 *= 0.98f;
        }
        advance(1);
        if (stats.Tree[2].perk[5] > 0)
            Cost_2 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float feral_rage_dmg()
    {
        amount = 18 + (1.53F * AD);
        amount *= 1 + 0.03F * (MP - 13);
        amount = Damageincrease(amount);
        if (strength > 0) amount *= 1 + (0.03F * strength);
        return amount;
    }

    public float feral_rage_W(float value)
    {
        return ((28 + (0.19F * value)) * size);
    }

    public void savage_roar()
    {
        energy -= 38;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 + (1 + (0.01F * fatigue_3)));
        fatigue_3 += 58;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_3 * fatigue_3 / 2000, Cost_3 * fatigue_3 / 1500)));
            fatigue_3 *= 0.9F;
            Cost_3 *= 0.98f;
        }
        if (stats.Tree[2].perk[5] > 0)
            Cost_3 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
        amount = 3.36F + (0.02F * AD);
        amount *= 1 + (0.08F * stats.Tree[2].perk[17]);
        GainAD(amount);
        beast_within_charge += 0.5F;
        beast_within_charge += (0.08F + 0.0004F * AD) * stats.Tree[2].perk[17];
    }

    public float savage_roar_ad()
    {
        amount = 0.01f * (2.6F + (0.013F * AD));
        amount *= 1 + (0.08F * stats.Tree[2].perk[17]);
        return amount;
    }

    public void gift_of_the_wild()
    {
        energy -= 70;
        enBar.SetEnergy(energy);
        SpendMana(Cost_4 + (1 + (0.01F * fatigue_4)));
        fatigue_4 += 50;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_4 * fatigue_4 / 2000, Cost_4 * fatigue_4 / 1500)));
            fatigue_4 *= 0.9F;
            Cost_4 *= 0.98f;
        }
        if (stats.Tree[2].perk[5] > 0)
            Cost_4 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float gift_of_the_wild_sh()
    {
        return 8 + (0.3F * MP);
    }

    public float gift_of_the_wild_mana()
    {
        return 8 + (0.53F * MP);
    }

    public void sap_magic()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_5 + (1 + (0.01F * fatigue_5)));
        fatigue_5 += 62;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_5 * fatigue_5 / 2000, Cost_5 * fatigue_5 / 1500)));
            fatigue_5 *= 0.9F;
            Cost_5 *= 0.98f;
        }
        advance(1);
        if (stats.Tree[2].perk[5] > 0)
            Cost_5 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float sap_magic_dmg()
    {
        amount = 17 + (1.56F * MP) + (0.012F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    public void sap_magic_restore(float value)
    {
        amount = value * (0.04F + (0.0013F * MP));
        mana += amount;
        manaT.text = mana.ToString("0");
        RestoreHealth(amount);
    }

    public void mana_blast()
    {
        energy -= 60;
        enBar.SetEnergy(energy);
        SpendMana(Cost_6 + (1 + (0.01F * fatigue_6)));
        fatigue_6 += 63;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_6 * fatigue_6 / 2000, Cost_6 * fatigue_6 / 1500)));
            fatigue_6 *= 0.9F;
            Cost_6 *= 0.98f;
        }
        advance(1);
        if (stats.Tree[2].perk[5] > 0)
            Cost_6 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float mana_blast_dmg()
    {
        amount = 40 + (0.7F * AD) + (3 * MP) + (0.05F * mana_spent);
        amount = Damageincrease(amount);
        return amount;
    }

    public void force_of_nature()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_7 + (1 + (0.01F * fatigue_7)));
        fatigue_7 += 90;
        if (stats.map.items.collected[53] == true)
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(Cost_7 * fatigue_7 / 2000, Cost_7 * fatigue_7 / 1500)));
            fatigue_7 *= 0.9F;
            Cost_7 *= 0.98f;
        }
        advance(1);
        if (stats.Tree[2].perk[5] > 0)
            Cost_7 *= 1 - (0.0075F * stats.Tree[2].perk[5]);
    }

    public float force_of_nature_dmg()
    {
        amount = 105 + (1.13F * AD) + (4.54F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public void force_of_nature_mp(float value)
    {
        GainMP(2.7F + (0.01F * value));
    }

    public float force_of_nature_stun()
    {
        return (40 + (1.59F * MP)) * size;
    }

    void expunge(float procent)
    {
        for (int i = 0; i < 5; i++)
        {
            if (stats.enemy_alive[i] == true)
                stats.enemy[i].GainPoison(stats.enemy[i].poison * procent * stats.Tree[2].perk[12]);
        }
    }

    // Blood Hero Skillset
    public void GainRage(int value)
    {
        rage += value;
        if (rage >= 100)
            Rage();
        powBar.SetEnergy(rage);
    }

    public void Rage()
    {
        rage -= 100;
        if (stats.map.items.collected[58] == true)
        {
            rage += 6;
            if (stats.map.items.collected[32] == true)
                rage += 3;
            if (stats.map.items.collected[33] == true)
                rage += 3;
            if (stats.map.items.collected[34] == true)
                rage += 3;
        }
        advance(1);
        stats.Rage('h');
        if (stats.Tree[3].perk[10] > 0)
            GainAD(0.004F * AD * stats.Tree[3].perk[10]);
        if (stats.map.items.collected[65] == true)
        {
            mana += 12 + 0.22F * AD;
            manaT.text = mana.ToString("0");
            GainHP(0.006F * HP);
        }
    }

    public void blood_well(float value)
    {
        value *= 1 + (0.01F * stats.Tree[3].tree1);
        if (stats.map.items.collected[31] == true)
            value *= 1.05f;
        blood_stacks += value;
        Display_Text(value.ToString("0") + " blood", 'c');
        AD += value / 30;
        MP += value / 135;
        AR += value * stats.Tree[2].perk[14] / 250;
    }

    public float blood_gain()
    {
        return 0.35F + 0.01F * stats.Tree[2].perk[11];
    }

    public float warlord_stance(float value)
    {
        if (attacks >= 1)
        {
            amount = 1.09F + ((AD - base_AD) * 0.001F);
            amount *= 0.75F + (0.25F * attacks);
            value /= amount;
        }
        return value;
    }

    public void cleave()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 + (1 + (0.01F * fatigue_1)));
        fatigue_1 += 42;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_1 * fatigue_1 / 2000, Cost_1 * fatigue_1 / 1500)));
            fatigue_1 *= 0.9F;
            Cost_1 *= 0.98f;
        }
        advance(1);
    }

    public float cleave_dmg()
    {
        amount = 13.5F + (0.53F * AD);
        amount = Damageincrease(amount);
        if (strength > 0) amount *= 1 + (0.03F * strength);
        return amount;
    }

    public void drain()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 + (1 + (0.01F * fatigue_2)));
        fatigue_2 += 55;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_2 * fatigue_2 / 2000, Cost_2 * fatigue_2 / 1500)));
            fatigue_2 *= 0.9F;
            Cost_2 *= 0.98f;
        }
        advance(1);
    }

    public float drain_dmg()
    {
        amount = 26 + (1.5F * AD) + (0.044F * blood_stacks);
        amount *= 1 + (0.03F * stats.Tree[3].perk[2]);
        amount = Damageincrease(amount);
        return amount;
    }

    public float drain_pen()
    {
        amount = pen - 0.05f - 0.01f * level;
        return amount;
    }

    public void drain_res(float value)
    {
        amount = 7 + 0.11F * value;
        amount *= 1 + (0.02F * stats.Tree[3].perk[2]);
        RestoreHealth(amount);
    }

    public void bloodletting()
    {
        energy -= 25;
        enBar.SetEnergy(energy);
        mana += (0.2F + 0.0125F * HP);
        SpendMana(Cost_3 + (1 + (0.01F * fatigue_3)));
        fatigue_3 += 100;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_3 * fatigue_3 / 2000, Cost_3 * fatigue_3 / 1500)));
            fatigue_3 *= 0.9F;
            Cost_3 *= 0.98f;
        }
        TakeDamage(6 + 0.03F * HP, 'r');
        blood_well(0.4F + 0.025F * HP + (0.035F * (HP - base_HP)));
        GainIN(1);
    }

    public void uppercut()
    {
        energy -= 44;
        if (agility > 0)
            energy += 6 * agility;
        energy += 6 * attacks;
        enBar.SetEnergy(energy);
        SpendMana(Cost_4 + (1 + (0.01F * fatigue_4)));
        fatigue_4 += 80;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_4 * fatigue_4 / 2000, Cost_4 * fatigue_4 / 1500)));
            fatigue_4 *= 0.9F;
            Cost_4 *= 0.98f;
        }
        advance(1);
    }

    public float uppercut_dmg()
    {
        amount = 6 + (0.49F * AD) + (0.06F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float uppercut_W()
    {
        return (73 + (0.1F * HP)) * size;
    }

    public void crush()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_5 + (1 + (0.01F * fatigue_5)));
        fatigue_5 += 50;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_5 * fatigue_5 / 2000, Cost_5 * fatigue_5 / 1500)));
            fatigue_5 *= 0.9F;
            Cost_5 *= 0.98f;
        }
        advance(1);
    }

    public float crush_dmg()
    {
        amount = 30 + (0.8F * AD) + (2.3F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float crush_stun()
    {
        amount = 68 + (0.94F * AD) + (2.2F * MP);
        if (strength > 0)
            amount *= 1 + (0.02F * strength);
        return amount;
    }

    public int overpower()
    {
        energy -= 75;
        enBar.SetEnergy(energy);
        SpendMana(Cost_6 + (1 + (0.01F * fatigue_6)));
        fatigue_6 += 75;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_6 * fatigue_6 / 2000, Cost_6 * fatigue_6 / 1500)));
            fatigue_6 *= 0.9F;
            Cost_6 *= 0.98f;
        }
        advance(1);
        return 2;
    }

    public float overpower_S()
    {
        amount = 30 + (0.31F * AD) + (0.99F * MP);
        amount *= size;
        return amount;
    }

    public float overpower_en()
    {
        amount = 25 + (0.11F * AD);
        amount *= size;
        return amount;
    }

    public void massacre()
    {
        energy -= 88;
        enBar.SetEnergy(energy);
        SpendMana(Cost_7 + (1 + (0.01F * fatigue_7)));
        fatigue_7 += 60;
        if (stats.map.items.collected[53] == true)
        {
            GainRage(Mathf.RoundToInt(Random.Range(Cost_7 * fatigue_7 / 2000, Cost_7 * fatigue_7 / 1500)));
            fatigue_7 *= 0.9F;
            Cost_7 *= 0.98f;
        }
        GainStrength(1);
        GainAgility(1);
        if (blood_stacks >= 1000)
            advance(4);
        else
        {
            advance(3);
            GainStun();
        }
    }

    public float massacre_dmg()
    {
        amount = 18 + (0.84F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float wrathful_dmg()
    {
        amount = (0.5F * AD) + (0.12F * (AD - base_AD));
        amount = Damageincrease(amount);
        return amount;
    }

    // Common Skillset
    public float Damageincrease(float value)
    {
        amount = value;
        if (strength >= 0) amount *= 1 + ((0.05F + 0.01F * stats.Tree[3].perk[1]) * strength);
        else amount /= 1 + (-0.05F * strength);
        if (stats.whichHero == 'l') amount *= 1 + (0.01F * stats.unit3.number);

        amount *= 1 + (0.003F * stats.Tree[0].perk[4] * stats.turnTime);
        amount *= 1 + (0.0004F * stats.Tree[1].perk[16] * AR);
        if (blood_stacks > 1000)
            if (stats.Tree[3].perk[6] > 0)
                amount *= 1.16f;
        amount *= 1 + ((0.005F + 0.001F * total_attacks) * stats.Tree[3].perk[9]);
        amount *= 1 + (0.06F * stats.Tree[3].perk[16] * (HP - hitPoints) / HP);

        if (stats.map.items.collected[12] == true)
            amount *= 1 + (0.01F * stats.turnTime);
        if (stats.map.items.collected[56] == true)
        {
            if (stats.turnTime <= 3)
                amount *= 1.2f;
            else amount *= 0.93f;
        }
        if (stats.map.items.collected[57] == true)
            amount *= 0.82f;

        return amount;
    }

    public float DamageMultiplyer(float value, float pen, char range)
    {
        amount = value;

        if ((range == 'm') || (range == 'r'))
            value /= 1 + (AR * (0.02F + 0.0006F * stats.Tree[2].perk[9] + 0.0016F * stats.Tree[3].perk[14]) * pen);
        else if (range == 's')
            value /= 1 + (MP * (0.01F + 0.0008F * stats.Tree[2].perk[2]) * pen);

        if (resistance >= 0) value /= 1 + (0.05F * resistance);
        else value *= 1 + (-0.05F * resistance);
        if (stats.whichHero == 'l') 
            value /= 1 + (0.01F * stats.unit3.number);
        if (stats.whichHero == 'b')
            value = warlord_stance(value);
        if (range == 'r')
        {
            if (stats.whichHero == 'l') value /= 1 + (0.008F * stats.unit1.number);
        }
        if (range == 's')
        {
            if (stats.whichHero == 'n') value /= 1 + (0.01F * stats.unit4.number);
            value /= 1 + (0.06f * stats.Tree[0].perk[11]);
            if (stats.map.items.collected[10] == true)
                value /= 1.08F;
        }

        value /= 1 + (0.003F * stats.Tree[0].perk[4] * stats.turnTime);
        value /= 1 + (0.004F * (IN - base_IN) * stats.Tree[1].perk[18]);
        if (agility > 0)
            value /= 1 + (0.02F * agility * stats.Tree[1].perk[18]);
        value /= 1 + (0.00005F * (fatigue_1 + fatigue_2 + fatigue_3 + fatigue_4 + fatigue_5 + fatigue_6 + fatigue_7) * stats.Tree[3].perk[5]);
        if (stats.Tree[3].perk[6] > 0)
            if (blood_stacks >= 1000)
                value /= 1.12F;

        return value;
    }

    public void TakeDamage(float value, char hue)
    {
        if (block > 0)
        {
            block--;
            Display_Text("blocked", 'w');
            if (stats.Tree[0].perk[10] > 0)
            {
                if (resistance > 0)
                    amount = 0.08f + 0.02f * resistance + 0.002f * AR;
                else amount = 0.08f + 0.002f * AR;
                GainShield(value * amount * stats.Tree[0].perk[10]);
            }
            if (stats.map.items.collected[42] == true)
            {
                mana += value * 0.2f;
                manaT.text = mana.ToString("0");
            }
        }
        else
        {
            Display_Text(value.ToString("0"), hue);
            if (stats.whichHero == 'b')
            {
                blood_well(value * (0.18F + 0.006F * stats.Tree[3].perk[16]));
                GainRage(Mathf.RoundToInt(value / (HP * 0.08f)));
            }
            if (shield > 0)
            {
                shield -= value;
                if (shield < 0)
                {
                    hitPoints += shield;
                    shield = 0;
                }
            }
            else
            {
                hitPoints -= value;
            }
            if (hitPoints <= 0)
            {
                if (inmortal == true)
                {
                    inmortal = false;
                    hitPoints = 0.22F * HP + 0.33F * blood_stacks;
                    AD += 0.03F * blood_stacks;
                }
                else
                {
                    Death();
                }
            }
            hpBar.SetHealth(hitPoints);
        }
    }

    public void Death()
    {
        Display_Text("Hero has fallen", 'z');
        stats.AllyTakenDown('h');
        unit.SetActive(false);
    }

    public void RestoreHealth(float value)
    {
        value *= 1 + (0.04F * stats.Tree[2].perk[11]);
        mana += value * 0.08f * stats.Tree[2].perk[11];
        if (stats.map.items.collected[54] == true)
        {
            value *= 0.75f;
            GainShield(value);
        }
        manaT.text = mana.ToString("0");
        Display_Text(value.ToString("0"), 'g');
        hitPoints += value;
        if (hitPoints > HP) hitPoints = HP;
        hpBar.SetHealth(hitPoints);
    }

    public void GainShield(float value)
    {
        Display_Text(value.ToString("0"), 'w');

        if (stats.Tree[1].perk[11] > 0)
        {
            value *= 1 + (0.03f + MP * 0.0004f) * stats.Tree[1].perk[11];
        }

        if (stats.map.items.collected[3] == true)
        {
            value *= 1.04f;
            GainAD(value / 50 + value * AD / 50000);
        }

        shield += value;
    }

    public void GainEnergy(float value)
    {
        value /= (1F * size);
        energy += value;
        if (energy > 150)
            energy = 150;
        enBar.SetEnergy(energy);
        Display_Text("+ " + value.ToString("0") + " energy", 'y');
    }

    public void LoseEnergy(float value)
    {
        value /= (1F * size);
        energy -= value;
        if (energy < 0)
            energy = 0;
        enBar.SetEnergy(energy);
        Display_Text("- " + value.ToString("0") + " energy", 'y');
    }

    public void SpendMana(float value)
    {
        mana -= value;
        mana_spent += value;
        if (stats.whichHero == 'w')
            flow_like_water(value);
        if (stats.whichHero == 'n')
        {
            GainBlossom(Mathf.RoundToInt(value / 12));
            barkskin(value);
        }
        manaT.text = mana.ToString("0");
        if (stats.Tree[2].perk[5] > 0)
        {
            ReduceFatigue(value * 0.005F * stats.Tree[2].perk[5], 0);
        }
        if (stats.Tree[3].perk[19] > 0)
        {
            amount = value * 0.13F * stats.Tree[3].perk[19];
            amount = Damageincrease(amount);
            stats.Transfusion(amount);
        }
        if (stats.map.items.collected[47] == true)
        {
            amount = 0.12f + 0.005F * level;
            stats.RottenFruit(amount);
        }
    }

    public int GainBuff(float value)
    {
        chance = Random.Range(1, 101);
        value *= 1 + (0.125F * stats.Tree[0].perk[5]);
        value /= size;
        if (stats.map.items.collected[19] == true)
            value *= 1.1f;
        if (chance <= value) return 1;
        else return 0;
    }

    public int GainDebuff(float value)
    {
        chance = Random.Range(1, 101);
        value /= 1 + (0.1F * stats.Tree[0].perk[11]);
        value /= 1 + (0.12F * stats.Tree[1].perk[19]);
        value /= 1 + (0.01F * stats.Tree[3].tree3);
        value /= 1 + (0.16F * stats.Tree[3].perk[18]);
        value /= size;
        if (chance <= value) return 1;
        else return 0;
    }

    void Buffed()
    {
        if (stats.Tree[0].perk[2] > 0)
        {
            GainEnergy(2 * stats.Tree[0].perk[2] * size);
            GainValor(1 * stats.Tree[0].perk[2]);
        }
        if (stats.Tree[0].perk[6] > 0)
        {
            AD *= 1 + (0.001F * stats.Tree[0].perk[6]);
            MP *= 1 + (0.001F * stats.Tree[0].perk[6]);
            AR *= 1 + (0.001F * stats.Tree[0].perk[6]);
            mana += 2 * stats.Tree[0].perk[6];
            manaT.text = mana.ToString("0");
        }
        if (stats.whichHero == 'l')
            GainValor(Random.Range(10, 16));
    }

    void DeBuffed()
    {
        if (stats.whichHero == 'l')
            GainValor(Random.Range(10, 16));
        if (stats.Tree[3].perk[18] > 0) 
            GainRage(Mathf.RoundToInt(2 * Mathf.Pow(2, stats.Tree[3].perk[18])));
    }

    public void GainStrength(int value)
    {
        if (stats.map.items.collected[59] == true)
        {
            resistance += value;
            Display_Text("+ " + value.ToString("0") + " resistance", 's');
            Buffed();
            agility += value;
            Display_Text("+ " + value.ToString("0") + " agility", 'b');
            Buffed();
        }
        else
        {
            strength += value;
            Display_Text("+ " + value.ToString("0") + " strength", 'r');
            Buffed();
        }
    }

    public void GainWeakness(int value)
    {
        strength -= value;
        Display_Text("+ " + value.ToString("0") + " weakness", 'g');
        DeBuffed();
    }

    public void GainResistance(int value)
    {
        if (stats.map.items.collected[59] == true)
        {
            strength += value;
            Display_Text("+ " + value.ToString("0") + " strength", 'r');
            Buffed();
            agility += value;
            Display_Text("+ " + value.ToString("0") + " agility", 'b');
            Buffed();
        }
        else
        {
            resistance += value;
            Display_Text("+ " + value.ToString("0") + " resistance", 's');
            Buffed();
        }
    }

    public void GainVoulnerable(int value)
    {
        resistance -= value;
        Display_Text("+ " + value.ToString("0") + " voulnerable", 'p');
        DeBuffed();
    }

    public void GainAgility(int value)
    {
        if (stats.map.items.collected[59] == true)
        {
            strength += value;
            Display_Text("+ " + value.ToString("0") + " strength", 'r');
            Buffed();
            resistance += value;
            Display_Text("+ " + value.ToString("0") + " resistance", 's');
            Buffed();
        }
        else
        {
            agility += value;
            Display_Text("+ " + value.ToString("0") + " agility", 'b');
            Buffed();
        }
    }

    public void GainSlow(int value)
    {
        agility -= value;
        Display_Text("+ " + value.ToString("0") + " slow", 'o');
        DeBuffed();
    }

    public void GainBleed(float value)
    {
        bleed += value;
        Display_Text("+ " + value.ToString("0") + " bleed", 'c');
    }

    public void GainPoison(float value)
    {
        poison += value;
        Display_Text("+ " + value.ToString("0") + " poison", 'v');
    }

    public void GainStun()
    {
        enBar.SetEnergy(energy);
        stun++;
        Display_Text("Stun", 'p');
        DeBuffed();
    }

    public void GainFreeze(int value)
    {
        freeze += value;
        Display_Text("+ " + value.ToString("0") + " freeze", 'n');
        DeBuffed();
    }

    public void GainAD(float value)
    {
        AD += value;
        Display_Text("+ " + value.ToString("0.0") + " attack damage", 'r');
    }

    public void GainMP(float value)
    {
        MP += value;
        Display_Text("+ " + value.ToString("0.0") + " magic power", 'p');
    }

    public void GainIN(float value)
    {
        IN += value;
        Display_Text("+ " + value.ToString("0.0") + " initiative", 'b');
    }

    public void GainHP(float value)
    {
        HP += value;
        hitPoints += value;
        Display_Text("+ " + value.ToString("0.0") + " hit points", 'g');
        hpBar.SetMaxHealth(HP);
        hpBar.SetHealth(hitPoints);
    }

    public void GainAR(float value)
    {
        AR += value;
        Display_Text("+ " + value.ToString("0.0") + " armor", 'o');
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
        level = stats.map.lvl;
        disable();
        if (stats.whichHero == 'l')
        {
            AD = 105; MP = 28; IN = 32; HP = 828; AR = 42;
            Cost_1 = 44; Cost_2 = 42; Cost_3 = 55; Cost_4 = 85; Cost_5 = 98; Cost_6 = 77; Cost_7 = 146; fatigue_7 = 15;
            hero_im.sprite = Light;
        }
        if (stats.whichHero == 'w')
        {
            AD = 107; MP = 33; IN = 42; HP = 869; AR = 34;
            Cost_1 = 72; Cost_2 = 42; Cost_3 = 85; Cost_4 = 62; Cost_5 = 89; Cost_6 = 163; Cost_7 = 150;
            hero_im.sprite = Water;
        }
        if (stats.whichHero == 'n')
        {
            AD = 99; MP = 44; IN = 36; HP = 1034; AR = 33;
            Cost_1 = 75; Cost_2 = 54; Cost_3 = 69; Cost_4 = 42; Cost_5 = 47; Cost_6 = 92; Cost_7 = 200;
            hero_im.sprite = Nature;
        }
        if (stats.whichHero == 'b')
        {
            AD = 131; MP = 40; IN = 35; HP = 964; AR = 36;
            Cost_1 = 65; Cost_2 = 54; Cost_3 = 20; Cost_4 = 36; Cost_5 = 65; Cost_6 = 75; Cost_7 = 145;
            hero_im.sprite = Blood;
        }

        BaseStatsBonuses();

        for (int i=1; i < level; i++)
        {
            AD += (0.025F * base_AD) + (0.03F * AD);
            MP += (0.025F * base_MP) + (0.015F * MP);
            IN += (0.02F * base_IN) + (0.01F * IN);
            HP += (0.015F * base_HP) + (0.025F * HP);
            AR += (0.025F * base_AR) + (0.02F * AR);
        }
        size = 3 + (level / 3);

        base_AD = AD; base_MP = MP; base_IN = IN; base_HP = HP; base_AR = AR;
        AD_s = 0.012F; MP_s = 0.03F; IN_s = 0.03F; HP_s = 0.03F; AR_s = 0.03F;

        BonusStatsBonuses();

        hitPoints = HP; energy = (IN * 0.2F); mana = (MP * 0.5F);

        if (stats.whichHero == 'b')
            blood_well(stats.map.blood);

        powBar.SetColor(stats.whichHero);

        ResourcesBonuses();

        if (stats.map.hero_condition > 0)
        {
            for (int i = 0; i < stats.map.hero_condition; i++)
            {
                hitPoints *= 0.94f;
                AD_s *= 0.94f; MP_s *= 0.94f; IN_s *= 0.94f; HP_s *= 0.94f; AR_s *= 0.94f;
            }
        }

        hpBar.SetMaxHealth(HP);
        hpBar.SetHealth(hitPoints);
        enBar.SetEnergy(energy);
        manaT.text = mana.ToString("0");

        if (stats.map.items.collected[9] == true)
            GainStun();
    }

    void BaseStatsBonuses()
    {
        HP += stats.Tree[0].perk[1] * 10;
        AR += stats.Tree[0].perk[8] * 2.5F;
        AD += stats.Tree[0].perk[15] * 3;
        AD += stats.Tree[1].perk[2] * 3; HP += stats.Tree[1].perk[2] * 6;
        MP += stats.Tree[1].perk[8] * 2;
        IN += stats.Tree[1].perk[15];
        AR += stats.Tree[1].perk[16] * 2;
        MP += stats.Tree[2].perk[1] * 2;
        MP += stats.Tree[2].perk[8]; HP += stats.Tree[2].perk[8] * 9;
        AD += stats.Tree[2].perk[15] * 3; AR += stats.Tree[2].perk[15] * 2;
        IN += stats.Tree[2].perk[16] * 1.2F;
        AD += stats.Tree[3].perk[8] * 4; IN += stats.Tree[3].perk[8];
        HP += stats.Tree[3].tree3 * 2;
        HP += stats.Tree[3].perk[15] * 9; AR += stats.Tree[3].perk[15] * 2;

        if (stats.map.items.collected[4] == true)
            AD += 4;
        if (stats.map.items.collected[5] == true)
            MP += 3;
        if (stats.map.items.collected[6] == true)
            IN += 2;
        if (stats.map.items.collected[7] == true)
            HP += 25;
        if (stats.map.items.collected[8] == true)
            AR += 5;

        base_AD = AD; base_MP = MP; base_IN = IN; base_HP = HP; base_AR = AR;
    }

    void BonusStatsBonuses()
    {
        MP += stats.Tree[2].perk[2];
        AR += stats.Tree[2].perk[10] * 4;
        pen = 1 - (0.01F * stats.Tree[3].tree2);
        AD += stats.Tree[3].perk[11] * 5;

        if (stats.Tree[0].perk[7] > 0)
            GainShield((4 + 0.7F * stats.allyArmy) * stats.Tree[0].perk[7]);
        if (stats.Tree[0].perk[13] > 0)
            block++;
        if (stats.Tree[1].tree3 > 0)
            GainShield(stats.Tree[1].tree3);
        if (stats.Tree[1].perk[0] > 0)
            gain_charge(stats.Tree[1].perk[0]);
        if (stats.Tree[1].perk[11] > 0)
            GainShield(6 * stats.Tree[1].perk[11]);
        if (stats.Tree[1].perk[13] > 0)
            GainAgility(1);
        if (stats.Tree[1].perk[20] > 0)
        {
            gain_charge(3);
            GainShield(40 + 0.6F * AR);
        }
        if (stats.Tree[2].perk[10] > 0)
            GainShield((10 + 0.13F * MP + 0.06F * AR) * stats.Tree[2].perk[10]);

        HP_s += stats.Tree[0].perk[1] * 0.002F;
        AR_s += stats.Tree[0].perk[8] * 0.002F;
        AD_s += stats.Tree[0].perk[15] * 0.001F;
        MP_s += stats.Tree[1].perk[8] * 0.0012F;

        if (stats.map.items.collected[2] == true)
        {
            HP *= 1.01f;
            HP_s += 0.004f;
        }
        if (stats.map.items.collected[11] == true)
            MP += 3;
        if (stats.map.items.collected[16] == true)
        {
            for (int i = 0; i < AR; i += 20)
            {
                GainValor(1);
                AR++;
            }
        }
        if (stats.map.items.collected[17] == true)
        {
            AR *= 1.01f;
            AR_s += 0.004f;
        }
        if (stats.map.items.collected[22] == true)
        {
            IN *= 1.01f;
            IN_s += 0.004f;
        }
        if (stats.map.items.collected[24] == true)
        {
            for (float i = 0; i < MP; i += 2.5f)
            {
                HP++;
            }
        }
        if (stats.map.items.collected[27] == true)
        {
            MP *= 1.01f;
            MP_s += 0.004f;
        }
        if (stats.map.items.collected[28] == true)
        {
            for (int i = 0; i < AD; i += 17)
            {
                GainRage(1);
                IN += 0.5f;
            }
        }
        if (stats.map.items.collected[29] == true)
        {
            AD *= 1.01f;
            AD_s += 0.002f;
        }
        if (stats.map.items.collected[33] == true)
        {
            if (stats.map.items.collected[58] == true)
                GainResistance(3);
            else GainResistance(1);
        }
        if (stats.map.items.collected[34] == true)
        {
            if (stats.map.items.collected[58] == true)
                GainAgility(3);
            else GainAgility(1);
        }
        if (stats.map.items.collected[50] == true)
            GainShield(0.11F * AD);
        if (stats.map.items.collected[51] == true)
        {
            GainStrength(1);
            fatigue_1 += 8; fatigue_2 += 8; fatigue_3 += 8; fatigue_4 += 8; fatigue_5 += 8; fatigue_6 += 8; fatigue_7 += 8;
        }
        if (stats.map.items.collected[60] == true)
            GainResistance(1);
        if (stats.map.items.collected[62] == true)
            GainResistance(1);
        if (stats.map.items.collected[66] == true)
        {
            amount = 0.006F + 0.001F * level;
            GainAD(AD * amount);
            GainMP(MP * amount);
        }
    }

    void ResourcesBonuses()
    {
        mana += stats.Tree[1].perk[2] * 4;
        energy += stats.Tree[1].perk[15] * 5;
        mana += stats.Tree[2].perk[0] * 10;
        energy += stats.Tree[2].perk[14] * 7;
        beast_within_charge = 0.16F * stats.Tree[2].perk[14];

        if (stats.map.items.collected[20] == true)
        {
            for (int i = 0; i < IN + 4 * (IN - base_IN); i += 12)
            {
                energy += 3;
                mana += 2;
            }
        }
        if (stats.map.items.collected[24] == true)
        {
            for (float i = 0; i < MP; i += 2.5f)
            {
                HP++;
                mana++;
            }
        }
        if (stats.map.items.collected[26] == true)
            mana += 5;
        if (stats.map.items.collected[38] == true)
        {
            if (stats.enemyArmy > stats.allyArmy)
            {
                energy += 3 * (stats.enemyArmy - stats.allyArmy);
                mana += 3 * (stats.enemyArmy - stats.allyArmy);
            }
        }
        if (stats.map.items.collected[52] == true)
        {
            blood_well(8 + stats.enemyArmy);
            if (stats.enemyArmy > 5)
                mana += stats.enemyArmy * 3 - 15;
        }

        if (stats.Tree[3].perk[0] > 0)
            blood_well((7 + 0.05F * AD) * stats.Tree[3].perk[0]);
        if (stats.Tree[3].perk[20] > 0)
            inmortal = true;
        else inmortal = false;

        if (stats.whichHero == 'l')
        {
            GainValor(5 + stats.allyArmy + stats.hero.level);
            if (stats.Tree[0].perk[0] > 0)
                GainValor(Random.Range(6 * stats.Tree[0].perk[0], 7 * stats.Tree[0].perk[0] + 1));
            if (stats.map.items.collected[13] == true)
                GainValor(8);
        }
        else if (stats.whichHero == 'w')
        {
            GainHaste(Mathf.RoundToInt(6 + 0.5F * IN));
            if (stats.map.items.collected[13] == true)
                GainHaste(8);
            if (stats.map.items.collected[45] == true)
                GainHaste(6);
        }
        else if (stats.whichHero == 'n')
        {
            GainBlossom(Mathf.RoundToInt(Random.Range(8, 13) + 0.4F * MP));
            if (stats.map.items.collected[13] == true)
                GainBlossom(8);
        }
        else if (stats.whichHero == 'b')
        {
            GainRage(Random.Range(7, 11));
            if (stats.Tree[3].perk[10] > 0)
                GainRage(Mathf.RoundToInt(Random.Range((3 + 2 * (AD - base_AD) / base_AD) * stats.Tree[3].perk[10], 1 + (6 + 4 * (AD - base_AD) / base_AD) * stats.Tree[3].perk[10])));
            if (stats.map.items.collected[13] == true)
                GainRage(8);
        }
    }

    void Reset()
    {
        shield = 0; energy = 0; mana = 0; bleed = 0; poison = 0;
        fatigue_1 = 0; fatigue_2 = 0; fatigue_3 = 0; fatigue_4 = 0; fatigue_5 = 0; fatigue_6 = 0; fatigue_7 = 0;
        strength = 0; resistance = 0; agility = 0; block = 0; stun = 0; freeze = 0;
        valor = 0; haste = 0; blossom = 0; rage = 0;
        mana_spent = 0; justice = 0; trident_bonus = 0; barkskin_charge = 0; beast_within_charge = 0; blood_stacks = 0; attacks = 0; total_attacks = 0;
    }

    void EnergyRegen()
    {
        amount = (IN + 25F) * 0.4F * Time.deltaTime;

        if (agility >= 0) amount *= 1 + (0.05F * agility);
        else amount /= 1 + (-0.05F * agility);

        if (stats.whichHero == 'l') amount *= 1 + (0.01F * stats.unit3.number);

        if (stats.Tree[0].perk[17] > 0)
            amount *= 1 + (0.02f + 0.001f * stats.enemy_attacks) * stats.Tree[0].perk[17];
        if (stats.Tree[3].perk[6] > 0)
            if (blood_stacks >= 1000)
                amount *= 1.08F;
        amount *= 1 + ((0.005F + 0.001F * total_attacks) * stats.Tree[3].perk[9]);

        if (stats.map.items.collected[56] == true)
        {
            if (stats.turnTime <= 3)
                amount *= 1.2f;
            else amount *= 0.93f;
        }
        if (stats.map.items.collected[62] == true)
        {
            amount *= 1.08F + (0.16F - DamageMultiplyer(0.16F, 1.0F, 'm'));
        }

        energy += amount;
        enBar.SetEnergy(energy);
    }

    void Update()
    {
        if (que > 0)
            que = 0;

        if (stats.whoseTurn == 0)
        {
            EnergyRegen();

            amount = (MP + 20) * 0.2F * Time.deltaTime;
            amount *= 1 + ((0.01F + 0.0025F * trident_bonus) * stats.Tree[1].perk[12]);
            amount *= 1 + (0.01F * stats.Tree[2].tree1);
            if (stats.map.items.collected[56] == true)
            {
                if (stats.turnTime <= 3)
                    amount *= 1.2f;
                else amount *= 0.93f;
            }
            mana += amount;
            manaT.text = mana.ToString("0");

            if (energy >= 100)
            {
                if (stun >= 1)
                {
                    energy -= 100;
                    stun--;
                }
                else
                {
                    Action();
                    stats.whoseTurn = 1;
                    field.blue();
                }
            }
        }
    }

    void Action()
    {
        if (stats.Tree[0].perk[3] > 0)
        {
            stats.unit1.GainEnergy(stats.Tree[0].perk[3] * 3 * size);
            stats.unit2.GainEnergy(stats.Tree[0].perk[3] * 3 * size);
            stats.unit3.GainEnergy(stats.Tree[0].perk[3] * 3 * size);
            stats.unit4.GainEnergy(stats.Tree[0].perk[3] * 3 * size);
        }
        if (stats.Tree[1].perk[5] > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (stats.enemy_alive[i] == true)
                    if (stats.enemy[i].bleed > 0)
                        stats.enemy[i].Bleed(0.05f * stats.Tree[1].perk[5]);
            }
        }
        if (stats.Tree[2].perk[12] > 0)
            expunge(0.02F);

        if (stats.map.items.collected[0] == true)
        {
            mana += 3;
            manaT.text = mana.ToString("0");
        }
    }

    public void NewTurn()
    {
        if (stats.whichHero == 'l')
            GainValor(Random.Range(7, 11));
        else if (stats.whichHero == 'w')
        {
            if (agility > 0)
                GainHaste(Random.Range(5 + agility, 13 + 2 * agility));
            else GainHaste(Random.Range(5, 13));
        }
        else if (stats.whichHero == 'n')
        {
            GainBlossom(Random.Range(7, 15));
            if (stats.map.items.collected[64] == true)
                GainBlossom(Mathf.RoundToInt(Random.Range(2 + 0.26F * level, 3 + 0.41F * level)));
        }
        else if (stats.whichHero == 'b')
            GainRage(Random.Range(9, 11));
        if (stats.map.items.collected[30] == true)
            RestoreHealth((HP - hitPoints) * 0.03f);

        if (stats.map.items.collected[49] == true)
        {
            ReduceFatigue(6, (0.05F + 0.005F * stats.Tree[1].perk[19]));
        }
        else ReduceFatigue(4, (0.04F + 0.005F * stats.Tree[1].perk[19]));

        if (stats.Tree[0].perk[20] > 0)
        {
            justice += 2;
            while (justice >= 6)
            {
                stats.Justice();
            }
        }
        if (stats.Tree[2].perk[4] > 0)
        {
            mana += (5 + 0.08F * MP + 0.0015F * mana_spent) * stats.Tree[2].perk[4];
            manaT.text = mana.ToString("0");
        }
        if (stats.Tree[2].perk[6] > 0)
        {
            GainBlossom(Random.Range(3 + stats.turnTime, 8 + 2 * stats.turnTime));
        }
        if (stats.Tree[3].perk[4] > 0)
        {
            mana += (2 + 0.005F * blood_stacks) * stats.Tree[3].perk[4];
            manaT.text = mana.ToString("0");
        }
        if (stats.Tree[3].perk[17] > 0)
            GainShield(((9 + 0.07F * AR) * stats.Tree[3].perk[17]) * (0.82F + 0.18F * attacks));

        if (stats.whichHero == 'l') resilience();

        if (stats.whichHero == 'n') beast_within_charge += 0.8F + 0.02F * level;

        if (stats.map.items.collected[48] == true)
        {
            GainBlossom(attacks);
            mana += (1 + attacks) * attacks;
            manaT.text = mana.ToString("0");
        }

        if (stats.Tree[3].perk[13] > 0)
            attacks = 1;
        else attacks = 0;

        if (bleed > 0) TakeDamage(DamageMultiplyer(bleed, 1, 'm'), 'r');
        if (poison > 0) TakeDamage(DamageMultiplyer(poison, 0, 's'), 'p');

        if (stats.map.items.collected[1] == true)
        {
            if (bleed > 0) bleed *= 0.94f;
            if (poison > 0) poison *= 0.94f;
        }
    }

    void ReduceFatigue(float flat, float procent)
    {
        fatigue_1 -= flat + (fatigue_1 * procent);
        if (fatigue_1 <= 0) fatigue_1 = 0;

        fatigue_2 -= flat + (fatigue_2 * procent);
        if (fatigue_2 <= 0) fatigue_2 = 0;

        fatigue_3 -= flat + (fatigue_3 * procent);
        if (fatigue_3 <= 0) fatigue_3 = 0;

        fatigue_4 -= flat + (fatigue_4 * procent);
        if (fatigue_4 <= 0) fatigue_4 = 0;

        fatigue_5 -= flat + (fatigue_5 * procent);
        if (fatigue_5 <= 0) fatigue_5 = 0;

        fatigue_6 -= flat + (fatigue_6 * procent);
        if (fatigue_6 <= 0) fatigue_6 = 0;

        fatigue_7 -= flat + (fatigue_7 * procent);
        if (fatigue_7 <= 0) fatigue_7 = 0;
    }

    public void enable()
    {
        target.enabled = true;
        field.green();
    }

    public void disable()
    {
        target.enabled = false;
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
        Unit_bod.rotation = Quaternion.Euler(Unit_bod.rotation.x, Unit_bod.rotation.y, Random.Range(-16f, 16f));
        GameObject text = Instantiate(Text_prefab, Unit_bod.position, Unit_bod.rotation);
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
        text_body.AddForce(Unit_bod.up * Random.Range(10, 14), ForceMode2D.Impulse + 200);
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

    public float Bottled_Light()
    {
        amount = 5 + (0.04F * MP) + level;
        amount = Damageincrease(amount);
        return amount;
    }
}