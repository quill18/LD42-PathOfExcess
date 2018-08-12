using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Stats
{
    MaxHP,
    CurrentHP,
    HealthRegen,
    Strength,       // Bonus Damage
    Dexterity,      // Boosts dodge, accuracy?
    Constitution,   // +10 HP per CON
    Intelligence,   // Increases identification speed!
    Wisdom,         // Boosts all res?
    Charisma,       // Joke dump stat
    Dodge,
    Armor,          // Damage reduction
    Damage,         // Base Weapon Damage Bonus
    Critical,
    ResCold,
    ResElec,
    ResFire,
    AttackSpeed,    // Percentage -- Starts at zero, base of 100 assumed by combat math
    Precision,
    Luck            // Increases drop rate/quality?
};

public enum DamageType
{
    Physical,
    Fire,
    Electrical,
    Cold
}


public class StatManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        statValues = new Dictionary<Stats, float>();

        SetValue(Stats.Strength, 10);
        SetValue(Stats.Dexterity, 10);
        SetValue(Stats.Constitution, 10);
        SetValue(Stats.Intelligence, 8);
        SetValue(Stats.Wisdom, 8);
        SetValue(Stats.Charisma, 5);
        SetValue(Stats.AttackSpeed, 0);
        SetValue(Stats.HealthRegen, Difficulty.DiffLevel == 0 ? 2 : 1);

        SetValue(Stats.Dodge, 50);
        SetValue(Stats.Precision, 100);

        SetValue(Stats.CurrentHP, 999999999);

        SetValue(Stats.Critical, 5);


        statStringers = new Dictionary<Stats, StatStringer>();
        statStringers[Stats.MaxHP] = (s) => { return "Hit Points: " + GetValue(Stats.CurrentHP) + "/" + GetValue(Stats.MaxHP); };
        statStringers[Stats.CurrentHP] = (s) => { return ""; };
        statStringers[Stats.Dodge] = (s) => { return "Dodge: " + DerivedDodge().ToString("#.#"); };
        statStringers[Stats.Precision] = (s) => { return "Precision: " + DerivedPrecision().ToString("#.#"); };
        statStringers[Stats.ResFire] = (s) => { return "Fire Resist: " + DerivedResFire().ToString("#.#"); };
        statStringers[Stats.ResElec] = (s) => { return "Elec Resist: " + DerivedResElec().ToString("#.#"); };
        statStringers[Stats.ResCold] = (s) => { return "Cold Resist: " + DerivedResCold().ToString("#.#"); };
        statStringers[Stats.Damage] = (s) => { return "Damage: " + DerivedDamage().ToString("#.#"); };

        statStringers[Stats.HealthRegen] = (s) => { return "HP Regen: " + GetValueF(Stats.HealthRegen).ToString("#.#") + " pct/sec"; };

        statTextBoxes = new Dictionary<Stats, Text>();

        // Create all the text boxes
        int rowLimit = 6;
        int rowCount = 0;
        int colCount = 0;

        Canvas canvas = GetComponentInParent<Canvas>();

        foreach (Stats s in System.Enum.GetValues(typeof(Stats)))
        {
            if (GetStatString(s) == "")
                continue;

            GameObject go = Instantiate(StatTextPrefab, this.transform);
            Text t = go.GetComponentInChildren<Text>();
            StatTooltipInfo sti = go.GetComponentInChildren<StatTooltipInfo>();

            statTextBoxes.Add(s, t);
            sti.Stat = s;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.position +=
                new Vector3(
                    rt.sizeDelta.x * colCount * canvas.scaleFactor,
                    -(rt.sizeDelta.y-2) * rowCount * canvas.scaleFactor,
                    0);

            rowCount++;
            if(rowCount >= rowLimit)
            {
                rowCount = 0;
                colCount++;
            }
        }
    }

    string GetStatString(Stats s)
    {
        string txt = "";
        if (statStringers.ContainsKey(s))
        {
            string str = statStringers[s](s);

            txt = str;
        }
        else
        {
            txt = s.ToString() + ": " + GetValueF(s).ToString("0.#");
        }

        return txt;
    }

    static StatManager __Instance;

    public static StatManager Instance
    {
        get
        {
            if (__Instance == null)
                __Instance = GameObject.FindObjectOfType<StatManager>();

            return __Instance;
        }
    }

    Dictionary<Stats, float> statValues;
    Dictionary<Stats, Text> statTextBoxes;

    public GameObject StatTextPrefab;

    delegate string StatStringer(Stats s);
    Dictionary<Stats, StatStringer> statStringers;
	
	// Update is called once per frame
	void Update () {
        int rowCount = 0;
        int childCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = "";
        }

        foreach (Stats s in System.Enum.GetValues(typeof(Stats)))
        {
            if (statTextBoxes.ContainsKey(s) == false)
                continue;

            statTextBoxes[s].text = GetStatString(s);
        }

	}

    public int GetValue(Stats s)
    {
        if (statValues.ContainsKey(s))
        {
            return (int)statValues[s];
        }

        return 0;
    }

    public float GetValueF(Stats s)
    {
        if (statValues.ContainsKey(s))
        {
            return statValues[s];
        }

        return 0;
    }

    public void ChangeValue(Stats s, float amount)
    {
        SetValue(s, GetValueF(s) + amount);
    }

    public void SetValue(Stats s, float value)
    {
        statValues[s] = value;

        if(s == Stats.Constitution)
        {
            SetValue(Stats.MaxHP, 50 + value * 5);
        }

        if (GetValue(Stats.CurrentHP) > GetValue(Stats.MaxHP))
        {
            SetValue(Stats.CurrentHP, GetValue(Stats.MaxHP));
        }
        if (GetValue(Stats.CurrentHP) < 0)
        {
            SetValue(Stats.CurrentHP, 0);
        }
    }

    public float DerivedDodge()
    {
        return GetValueF(Stats.Dodge) + GetValueF(Stats.Dexterity) * 2;
    }

    public float DerivedPrecision()
    {
        return GetValueF(Stats.Precision) + GetValueF(Stats.Dexterity) * 2;
    }

    public float DerivedResFire()
    {
        return GetValueF(Stats.ResFire) + GetValueF(Stats.Wisdom) * 2;
    }
    public float DerivedResCold()
    {
        return GetValueF(Stats.ResCold) + GetValueF(Stats.Wisdom) * 2;
    }
    public float DerivedResElec()
    {
        return GetValueF(Stats.ResElec) + GetValueF(Stats.Wisdom) * 2;
    }

    public float DerivedDamage()
    {
        float maxDamage = GetValueF(Stats.Damage);
        float strBonus = GetValueF(Stats.Strength) / 2;

        return maxDamage + strBonus;
    }

    public void HealPlayer()
    {
        SetValue(Stats.CurrentHP, GetValueF(Stats.MaxHP));
    }
}
