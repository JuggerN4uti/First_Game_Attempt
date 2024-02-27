using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Unit3Stats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    public NumberText numT;

    public Transform Unit_bod;
    public GameObject Text_prefab;
    private IEnumerator coroutine;
    private int que = 0;
    public UnitDetailsText info;

    public HeroStats hero;
    public Unit1Stats unit1;
    public Unit2Stats unit2;
    public Unit4Stats unit4;

    public float AD, MP, IN, HP, AR;
    public float hitPoints, shield, energy, mana, bleed, poison;
    public int Cost_1, Cost_2, Cost_3;
    public float fatigue_1, fatigue_2, fatigue_3;
    public int number, m_number, size, strength, resistance, agility, block, stun, freeze;
    public float amount, chance;

    // Light Specific
    int valor;

    // Water Specific
    int haste;
    public float revenge_stacks;

    // Nature Specific
    int blossom;
    public float brage, attacks = 0;

    // Blood Specific
    int rage;
    public float wrath_stacks;

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
            if (stats.whichHero == 'b')
                GainRage(Mathf.RoundToInt(5 + 5 * (AD / 101) / 101));
        }
        stats.ally_attacks++;
        if (stats.map.items.collected[40] == true)
        {
            if (stats.ally_attacks % 16 == 0)
                stats.RandomBuffA();
        }
    }

    public float attack_dmg()
    {
        amount = AD;
        amount = Damageincrease(amount);
        return amount;
    }

    // Light Unit Skillset
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
        GainShield((5 + 0.01F * HP) * number);
    }

    public void heavy_armor_ar()
    {
        GainAR(0.04f * AR);
    }

    public float heavy_armor(float value)
    {
        value /= 1 + (0.12F + (0.002F * (AR - 48)));
        return value;
    }

    public void judgement()
    {
        energy -= 90;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;
        advance(1);
    }

    public float judgement_dmg(bool attacked, int total_attacks)
    {
        amount = 3 + (1.37F * AD);
        amount = Damageincrease(amount);
        if (attacked == true)
            amount *= 1.2F;
        amount *= 1 + 0.02F * total_attacks;
        return amount;
    }

    public float judgement_V()
    {
        return (75 + (0.52F * AD)) * number * size;
    }

    public void inspire()
    {
        energy -= 30;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 44;
    }

    public float inspire_S()
    {
        return 100 * number * size;
    }

    public float inspire_A()
    {
        return 116 * number * size;
    }

    public float inspire_en()
    {
        return 12 * number * size;
    }

    public void penance()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 25;
        amount = 3.5F + (0.05F * AD) + (0.2F * MP);
        amount *= number;
        GainShield(amount);
        RestoreHealth(amount);
        advance(1);
    }

    public float penance_dmg()
    {
        amount = 11 + (0.37F * AD) + (1.47f * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    // Water Unit Skillset
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
        enBar.SetEnergy(energy);
        if (stats.map.items.collected[45] == true)
            GainShield((7 + (IN - 34)) * number);
        ReduceFatigue(6, 0);
    }

    public float staggering_blows_in(float value)
    {
        return (82 + (2.06f * value)) * number * size;
    }

    public float staggering_blows_V()
    {
        return 36 * number * size;
    }

    public void revenge_gain(float value, float procentage)
    {
        value = value * procentage;
        revenge_stacks += value;
        Display_Text(value.ToString("0") + " revenge", 'b');
    }

    public void dredge_line()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 46;
        advance(1);
    }

    public float dredge_line_dmg()
    {
        amount = 12 + (0.69F * AD) + (0.07F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float dredge_line_stun()
    {
        return (81 + (0.19F * HP)) * number * size;
    }

    public void shells_up()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 63;
        amount = 16 + (0.08F * HP) + (0.77F * AR);
        revenge_gain(amount, 0.88f);
        GainShield(amount * number);
    }

    public void revenge()
    {
        energy -= 100;
        GainHaste(Random.Range(5, 9));
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 50;
        advance(1);
    }

    public float revenge_dmg()
    {
        amount = 25 + (1.35F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public float revenge_m()
    {
        amount = revenge_stacks;
        amount = Damageincrease(amount);
        return amount;
    }

    public float revenge_F()
    {
        return (70 + (0.52F * MP) + (0.18F * revenge_stacks)) * number * size;
    }

    public void revenge_b()
    {
        GainAD(revenge_stacks / 60);
        revenge_stacks = 0;
    }

    // Nature Unit Skillset
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
        RestoreHealth((10 + 0.01F * HP) * number);
    }

    public void berserkers_rage(float value)
    {
        brage += value;
        Display_Text(value.ToString("0") + " brage", 'g');
        AD += value / 24;
        IN += value / 31;
    }

    public void regeneration()
    {
        amount = 0.2F + (0.003F * HP) + (0.02F * brage);
        amount *= number;
        amount *= 1 + (0.06f * attacks);
        RestoreHealth(amount);
        for (int i = 1; i <= attacks; i++)
        {
            GainBlossom(1 + i * 2);
        }
    }

    public void axe_throw()
    {
        energy -= 84;
        energy += 0.37F * IN;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 37;
        advance(1);
    }

    public float axe_throw_dmg()
    {
        amount = 3 + (0.97F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public void reckless_swing()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 50;
        berserkers_rage(3.3F + (0.016F * HP));
        attacks++;
        advance(1);
    }

    public float reckless_swing_dmg()
    {
        amount = 8 + (1.25F * AD) + (0.08F * brage);
        amount = Damageincrease(amount);
        if (strength > 0)
            amount *= 1 + (0.02F * strength);
        return amount;
    }

    public void stomp()
    {
        energy -= 88;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 44;
        attacks++;
        advance(1);
    }

    public float stomp_dmg()
    {
        amount = 9 + (0.23F * AD) + (0.28F * MP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float stomp_stun()
    {
        amount = 27 + (0.81F * AD) + (0.13F * brage);
        amount *= number * size;
        return amount;
    }

    // Blood Unit Skillset
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
        stats.Rage('1');
        if (stats.Tree[3].perk[10] > 0)
            GainAD(0.004F * AD * stats.Tree[3].perk[10]);
        if (stats.map.items.collected[65] == true)
        {
            mana += 12 + 0.22F * AD;
            manaT.text = mana.ToString("0");
            GainHP(0.006F * HP);
        }
    }

    public void wrath(float value)
    {
        wrath_stacks += value;
        Display_Text(value.ToString("0") + " wrath", 'c');
        AD += value / 13;
        AR += value / 25;
    }

    public void reform(float value)
    {
        GainShield(value * (0.22F + (0.0009F * AR)));
    }

    public void decimate()
    {
        energy -= 100;
        enBar.SetEnergy(energy);
        SpendMana(Cost_1 * (1 + (0.01F * fatigue_1)));
        fatigue_1 += 40;

        amount = 0.7F + (0.01F * HP) + (0.08F * wrath_stacks);
        amount *= number;
        RestoreHealth(amount);
        advance(1);
    }

    public float decimate_dmg()
    {
        amount = 21 + (1.34F * AD);
        amount = Damageincrease(amount);
        return amount;
    }

    public void trample()
    {
        energy -= 54;
        enBar.SetEnergy(energy);
        SpendMana(Cost_2 * (1 + (0.01F * fatigue_2)));
        fatigue_2 += 72;
        advance(1);
    }

    public float trample_dmg()
    {
        amount = 4 + (0.12F * AD) + (0.07F * HP);
        amount = Damageincrease(amount);
        return amount;
    }

    public float trample_stun()
    {
        amount = 44 + (0.21F * HP);
        amount *= number * size;
        return amount;
    }

    public void roar()
    {
        energy -= 80;
        enBar.SetEnergy(energy);
        SpendMana(Cost_3 * (1 + (0.01F * fatigue_3)));
        fatigue_3 += 48;
        advance(1);
        GainRage(Mathf.RoundToInt(4 + 10 * (AD - 101) / 101));
    }

    public float roar_dmg()
    {
        amount = 7.7F + (0.2F * MP) + (0.23F * wrath_stacks);
        amount = Damageincrease(amount);
        return amount;
    }

    public float roar_deb()
    {
        amount = 94 + (0.72F * AD) + (1.03F * MP);
        amount *= number * size;
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

        amount *= number;
        return amount;
    }

    public float DamageMultiplyer(float value, float pen, char range)
    {

        if ((range == 'm') || (range == 'r'))
            value /= 1 + (AR * (0.02F + 0.0006F * stats.Tree[2].perk[9]) * pen);
        else if (range == 's')
            value /= 1 + (MP * (0.01F + 0.0008F * stats.Tree[2].perk[2]) * pen);

        if (resistance >= 0) value /= 1 + (0.05F * resistance);
        else value *= 1 + (-0.05F * resistance);
        if (stats.whichHero == 'l')
        {
            heavy_armor(value);
            value /= 1 + (0.01F * number);
        }
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
        value /= 1 + (0.004F * (IN - 34) * stats.Tree[1].perk[18]);
        if (agility > 0)
            value /= 1 + (0.02F * agility * stats.Tree[1].perk[18]);

        return value;
    }

    public void TakeDamage(float value, char hue)
    {
        amount = 0;
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
            if (stats.whichHero == 'w')
                revenge_gain(value / m_number, 0.36f);
            if (stats.whichHero == 'n')
                berserkers_rage(value * 0.42F / m_number);
            if (stats.whichHero == 'b')
            {
                wrath(value * 0.46F / m_number);
                GainRage(Mathf.RoundToInt(value / (HP * 0.08f)));
            }
            if (shield > 0)
            {
                shield -= value;
                if (shield < 0)
                {
                    value = shield * -1;
                    shield = 0;
                    hitPoints -= value;
                    if (stats.whichHero == 'b')
                        reform(value);
                }
            }
            else
            {
                hitPoints -= value;
                if (stats.whichHero == 'b')
                    reform(value);
            }
            while (hitPoints <= 0)
            {
                hitPoints += HP;
                number--;
                if (stats.Tree[0].perk[19] > 0)
                {
                    hero.energy += 2 * stats.Tree[0].perk[19] * size;
                    hero.mana += stats.Tree[0].perk[19] * size;
                }
                amount++;
                if (number == 0)
                {
                    Death();
                }
            }
            hpBar.SetHealth(hitPoints);
            numT.Display(number);
            if (amount != 0)
                Display_Text(amount.ToString("") + " killed", 'z');
        }
    }

    public void Death()
    {
        Display_Text("Ally has fallen", 'z');
        stats.AllyTakenDown('3');
        unit.SetActive(false);
    }

    public void GainEnergy(float value)
    {
        value /= (1F * number * size);
        energy += value;
        if (energy > 150)
            energy = 150;
        enBar.SetEnergy(energy);
        Display_Text("+ " + value.ToString("0") + " energy", 'y');
    }

    public void LoseEnergy(float value)
    {
        value /= (1F * number * size);
        energy -= value;
        if (energy < 0)
            energy = 0;
        enBar.SetEnergy(energy);
        Display_Text("- " + value.ToString("0") + " energy", 'y');
    }

    public void RestoreHealth(float value)
    {
        amount = 0;
        value *= 1 + (0.04F * stats.Tree[2].perk[11]);
        mana += (value * 0.08f * stats.Tree[2].perk[11]) / size / number;
        if (stats.map.items.collected[54] == true)
        {
            value *= 0.75f;
            GainShield(value);
        }
        manaT.text = mana.ToString("0");
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
        Display_Text(value.ToString("0"), 'w');

        if (stats.Tree[1].perk[11] > 0)
        {
            value *= 1 + (0.03f + stats.hero.MP * 0.0004f) * stats.Tree[1].perk[11];
        }

        if (stats.map.items.collected[3] == true)
        {
            value *= 1.04f;
            GainAD(value / 50 + value * AD / 50000);
        }
        shield += value;
    }

    public void SpendMana(float value)
    {
        mana -= value;
        if (stats.whichHero == 'n')
            GainBlossom(Mathf.RoundToInt(value / 12));
        manaT.text = mana.ToString("0");
    }

    public int GainBuff(float value)
    {
        chance = Random.Range(1, 101);
        value *= 1 + (0.125F * stats.Tree[0].perk[5]);
        value /= number * size;
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
        value /= 1 + (0.16F * stats.Tree[3].perk[18]);
        value /= number * size;
        if (stats.whichHero == 'w')
            value /= (1.26f + 0.0022f * (HP - 451));
        if (chance <= value) return 1;
        else return 0;
    }

    void Buffed()
    {
        if (stats.Tree[0].perk[2] > 0)
        {
            GainEnergy(2 * stats.Tree[0].perk[2] * size * number);
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

    public void Hero_sh()
    {
        AD += (hero.AD * hero.AD_s) * size * (1 + stats.Tree[0].tree1 * 0.01F);
        MP += (hero.MP * hero.MP_s) * size * (1 + stats.Tree[0].tree1 * 0.01F);
        IN += (hero.IN * hero.IN_s) * (1 + stats.Tree[0].tree1 * 0.01F);
        HP += (hero.HP * hero.HP_s) * size * (1 + stats.Tree[0].tree1 * 0.01F);
        AR += (hero.AR * hero.AR_s) * (1 + stats.Tree[0].tree1 * 0.01F);
    }

    public void Set()
    {
        Reset();
        disable();
        m_number = number;
        if (number <= 0)
        {
            stats.AllyTakenDown('3');
            unit.SetActive(false);
        }
        else
        {
            if (stats.Tree[0].perk[6] > 0)
                number++;
            numT.SetMax(m_number);
            numT.Display(number);
            if (stats.whichHero == 'l')
            {
                AD = 55; MP = 22; IN = 32; HP = 240; AR = 48; size = 2;
                Cost_1 = 44; Cost_2 = 32; Cost_3 = 70;
                hero_im.sprite = Light;
            }
            if (stats.whichHero == 'w')
            {
                AD = 71; MP = 31; IN = 34; HP = 451; AR = 46; size = 3;
                Cost_1 = 60; Cost_2 = 71; Cost_3 = 84;
                hero_im.sprite = Water;
            }
            if (stats.whichHero == 'n')
            {
                AD = 39; MP = 27; IN = 41; HP = 371; AR = 25; size = 2;
                Cost_1 = 22; Cost_2 = 43; Cost_3 = 74;
                hero_im.sprite = Nature;
            }
            if (stats.whichHero == 'b')
            {
                AD = 101; MP = 39; IN = 30; HP = 583; AR = 55; size = 4;
                Cost_1 = 62; Cost_2 = 28; Cost_3 = 76;
                hero_im.sprite = Blood;
            }
            Hero_sh();
            MP += stats.Tree[2].perk[2];
            AR += stats.Tree[2].perk[10] * 4;

            if (stats.Tree[0].perk[7] > 0)
                GainShield((4 + 0.7F * stats.allyArmy) * stats.Tree[0].perk[7]);
            if (stats.Tree[0].perk[13] > 0)
                block++;
            if (stats.Tree[1].tree3 > 0)
                GainShield(stats.Tree[1].tree3);
            if (stats.Tree[1].perk[11] > 0)
                GainShield(6 * stats.Tree[1].perk[11]);
            if (stats.Tree[1].perk[13] > 0)
                GainAgility(1);
            if (stats.Tree[2].perk[10] > 0)
                GainShield((10 + 0.13F * MP + 0.06F * AR) * stats.Tree[2].perk[10]);

            if (stats.map.items.collected[11] == true)
                MP += 1;
            if (stats.map.items.collected[16] == true)
            {
                for (int i = 0; i < AR; i += 20)
                {
                    GainValor(1);
                    AR++;
                }
            }
            if (stats.map.items.collected[24] == true)
            {
                for (float i = 0; i < MP; i += 2.5f)
                {
                    HP++;
                }
            }
            if (stats.map.items.collected[26] == true)
                mana += 5;
            if (stats.map.items.collected[28] == true)
            {
                for (int i = 0; i < AD; i += 17)
                {
                    GainRage(1);
                    IN += 0.5f;
                }
            }
            if (stats.map.items.collected[32] == true)
            {
                if (stats.map.items.collected[58] == true)
                    GainStrength(3);
                else GainStrength(1);
            }
            if (stats.map.items.collected[33] == true)
            {
                if (stats.map.items.collected[58] == true)
                    GainResistance(3);
                else GainResistance(1);
            }
            if (stats.map.items.collected[50] == true)
                GainShield(0.11F * AD * number);
            if (stats.map.items.collected[51] == true)
            {
                GainStrength(1);
                fatigue_1 += 8; fatigue_2 += 8; fatigue_3 += 8;
            }
            if (stats.map.items.collected[60] == true)
                GainResistance(1);
            if (stats.map.items.collected[62] == true)
                GainResistance(1);
            if (stats.map.items.collected[66] == true)
            {
                amount = 0.006F + 0.001F * stats.hero.level;
                GainAD(AD * amount);
                GainMP(MP * amount);
            }

            if (stats.whichHero == 'l')
                heavy_armor_ar();

            hitPoints = HP; energy = (IN * 0.2F); mana = (MP * 0.5F);

            mana += stats.Tree[2].perk[0] * 3;
            if (stats.map.items.collected[20] == true)
            {
                for (int i = 0; i < IN + 4 * (IN - 30); i += 12)
                {
                    energy += 3;
                    mana += 2;
                }
            }
            if (stats.map.items.collected[24] == true)
            {
                for (float i = 0; i < MP; i += 2.5f)
                {
                    mana++;
                }
            }
            if (stats.map.items.collected[38] == true)
            {
                if (stats.enemyArmy > stats.allyArmy)
                {
                    energy += 3 * (stats.enemyArmy - stats.allyArmy);
                    mana += 3 * (stats.enemyArmy - stats.allyArmy);
                }
            }

            enBar.SetEnergy(energy);
            manaT.text = mana.ToString("0");
            hpBar.SetMaxHealth(HP);
            hpBar.SetHealth(hitPoints);

            powBar.SetColor(stats.whichHero);
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
                    GainRage(Mathf.RoundToInt(Random.Range((3 + 2 * (AD - 101) / 101) * stats.Tree[3].perk[10], 1 + (6 + 4 * (AD - 101) / 101) * stats.Tree[3].perk[10])));
                if (stats.map.items.collected[13] == true)
                    GainRage(8);
            }

            if (stats.map.items.collected[9] == true)
                GainStun();
        }
    }

    void Reset()
    {
        shield = 0; energy = 0; mana = 0; bleed = 0; poison = 0;
        fatigue_1 = 0; fatigue_2 = 0; fatigue_3 = 0;
        strength = 0; resistance = 0; agility = 0; block = 0; stun = 0; freeze = 0;
        valor = 0; haste = 0; blossom = 0; rage = 0;
        revenge_stacks = 0; brage = 0; attacks = 0; wrath_stacks = 0;
    }

    void EnergyRegen()
    {
        amount = (IN + 25F) * 0.4F * Time.deltaTime;

        if (agility >= 0) amount *= 1 + (0.05F * agility);
        else amount /= 1 + (-0.05F * agility);

        if (stats.Tree[0].perk[17] > 0)
            amount *= 1 + (0.02f + 0.001f * stats.enemy_attacks) * stats.Tree[0].perk[17];

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
                    if (stats.whichHero == 'w')
                        energy += 22;
                    enBar.SetEnergy(energy);
                    stun--;
                }
                else
                {
                    Action();
                    stats.whoseTurn = 4;
                    field.blue();
                }
            }
        }
    }

    void Action()
    {
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
            regeneration();
            GainBlossom(Random.Range(7, 15));
            if (stats.map.items.collected[64] == true)
                GainBlossom(Mathf.RoundToInt(Random.Range(2 + 0.26F * stats.hero.level, 3 + 0.41F * stats.hero.level)));
        }
        else if (stats.whichHero == 'b')
            GainRage(Random.Range(9, 11));

        if (stats.map.items.collected[49] == true)
        {
            ReduceFatigue(6, (0.05F + 0.005F * stats.Tree[1].perk[19]));
        }
        else ReduceFatigue(4, (0.04F + 0.005F * stats.Tree[1].perk[19]));

        attacks = 0;

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
            case 'w':
                text.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                break;
            case 'z':
                text.GetComponent<TextMeshPro>().color = new Color(0, 0, 0, 1);
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
}