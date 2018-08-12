using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTooltipInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public Stats Stat;
	
	// Update is called once per frame
	void Update () {
		
	}

    public string StatDescription()
    {
        switch (Stat)
        {
            case Stats.MaxHP:
                return "Hitpoints = Not dying!";
                break;
            case Stats.CurrentHP:
                return "Hitpoints = Not dying!";
                break;
            case Stats.HealthRegen:
                return "Percentage of maximum health regenerated every second.";
                break;
            case Stats.Strength:
                return "Increases damage dealt.";
                break;
            case Stats.Dexterity:
                return "Increases dodging and precision.";
                break;
            case Stats.Constitution:
                return "Increases maximum hitpoints.";
                break;
            case Stats.Intelligence:
                return "Increases item identification speed.";
                break;
            case Stats.Wisdom:
                return "Increases elemental resistances (important vs bosses).";
                break;
            case Stats.Charisma:
                return "Increases stat gains from selling loot in town.";
                break;
            case Stats.Dodge:
                return "Helps you avoid attacks.";
                break;
            case Stats.Armor:
                return "Reduces damage from attacks.";
                break;
            case Stats.Damage:
                return "The maximum damage you can inflict against enemies.";
                break;
            case Stats.Critical:
                return "Chance to deal a critical strike for double damage.";
                break;
            case Stats.ResCold:
                return "Resistance vs Cold attacks (from bosses).";
                break;
            case Stats.ResElec:
                return "Resistance vs Electrical attacks (from bosses).";
                break;
            case Stats.ResFire:
                return "Resistance vs Fire attacks (from bosses).";
                break;
            case Stats.AttackSpeed:
                return "Increases the speed at which you attack.";
                break;
            case Stats.Precision:
                return "Increases your chance to hit enemies.";
                break;
            case Stats.Luck:
                return "Improves the quality of the loot you get!";
                break;
        }

        return "Unknown stat: " + Stat.ToString();
    }
}
