using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class ItemMod 
{
    public readonly Stats Stat;
    public int Value;

    public ItemMod(Stats s, int v)
    {
        Stat = s;
        Value = v;
    }
}

public enum ItemTypes
{
    Helmet,
    Armor,
    Weapon,
    Shield,
    Belt,
    Boots,
    Gloves,
    Trinket,
    Potion
}

struct ItemModChart
{
    public int iLvl;
    public string Name;
    public Stats Stat;
    public float Value;
}

struct ItemSubType
{
    public string Name;
    public string Image;
    public int Width;
    public int Height;
    public int Value;
}

public static class ItemGenerator
{
    static ItemModChart[] prefixes = new ItemModChart[]
    {
        new ItemModChart{ iLvl = 5,  Name = "Bronze",   Stat = Stats.Strength, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Iron",     Stat = Stats.Strength, Value = 20 },
        new ItemModChart{ iLvl = 15, Name = "Silver",   Stat = Stats.Strength, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "Golden",   Stat = Stats.Strength, Value = 45 },
        new ItemModChart{ iLvl = 25, Name = "Mighty",   Stat = Stats.Strength, Value = 60 },
        new ItemModChart{ iLvl = 30, Name = "Giant",    Stat = Stats.Strength, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Angry",    Stat = Stats.Strength, Value = 100 },


        new ItemModChart{ iLvl = 1, Name = "Bulky",     Stat = Stats.Dexterity, Value = -5 },
        new ItemModChart{ iLvl = 1, Name = "Adequate",  Stat = Stats.Dexterity, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "Ergonomic", Stat = Stats.Dexterity, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Agile",     Stat = Stats.Dexterity, Value = 20 },
        new ItemModChart{ iLvl = 15, Name = "Slippery",  Stat = Stats.Dexterity, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "Invisible", Stat = Stats.Dexterity, Value = 40 },
        new ItemModChart{ iLvl = 25, Name = "Swag",      Stat = Stats.Dexterity, Value = 50 },
        new ItemModChart{ iLvl = 35, Name = "Mercurial",    Stat = Stats.Dexterity, Value = 100 },

        new ItemModChart{ iLvl = 1, Name = "Healthy",         Stat = Stats.Constitution, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "Hairy",          Stat = Stats.Constitution, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Mythril",       Stat = Stats.Constitution, Value = 15 },
        new ItemModChart{ iLvl = 15, Name = "Pickled",       Stat = Stats.Constitution, Value = 25 },
        new ItemModChart{ iLvl = 20, Name = "Adamantite",    Stat = Stats.Constitution, Value = 35 },
        new ItemModChart{ iLvl = 25, Name = "Vegemite",      Stat = Stats.Constitution, Value = 50 },
        new ItemModChart{ iLvl = 30, Name = "Bistromathic",  Stat = Stats.Constitution, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Thicc",         Stat = Stats.Constitution, Value = 100 },


        new ItemModChart{ iLvl = 5,  Name = "Brilliant", Stat = Stats.Intelligence, Value = 10 },
        new ItemModChart{ iLvl = 15, Name = "Runic",    Stat = Stats.Intelligence, Value = 20 },
        new ItemModChart{ iLvl = 25, Name = "Eldrich",  Stat = Stats.Intelligence, Value = 30 },
        new ItemModChart{ iLvl = 35, Name = "Iconic",   Stat = Stats.Intelligence, Value = 50 },


        new ItemModChart{ iLvl = 1, Name = "Drunken", Stat = Stats.Wisdom, Value = -5 },
        new ItemModChart{ iLvl = 1, Name = "Holy", Stat = Stats.Wisdom, Value = 5 },
        new ItemModChart{ iLvl = 10, Name = "Edible", Stat = Stats.Wisdom, Value = 10 },
        new ItemModChart{ iLvl = 18, Name = "Delectable", Stat = Stats.Wisdom, Value = 25 },
        new ItemModChart{ iLvl = 25, Name = "Naughty", Stat = Stats.Wisdom, Value = 50 },
        new ItemModChart{ iLvl = 35, Name = "Belgian", Stat = Stats.Wisdom, Value = 100 },

        new ItemModChart{ iLvl = 10, Name = "Shiny",     Stat = Stats.Charisma, Value = 25 },
        new ItemModChart{ iLvl = 20, Name = "Purring",   Stat = Stats.Charisma, Value = 50 },
        new ItemModChart{ iLvl = 30, Name = "Glittery",  Stat = Stats.Charisma, Value = 100 },

        new ItemModChart{ iLvl = 1, Name = "Keen",    Stat = Stats.Critical, Value = 5 },
        new ItemModChart{ iLvl = 10, Name = "Surgical",    Stat = Stats.Critical, Value = 10 },
        new ItemModChart{ iLvl = 20, Name = "Serrated",    Stat = Stats.Critical, Value = 20 },
        new ItemModChart{ iLvl = 30, Name = "Vorpal",    Stat = Stats.Critical, Value = 30 },

        new ItemModChart{ iLvl = 1, Name = "Quick",    Stat = Stats.AttackSpeed, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Rapid",    Stat = Stats.AttackSpeed, Value = 20 },
        new ItemModChart{ iLvl = 20, Name = "Speedy",    Stat = Stats.AttackSpeed, Value = 30 },

        new ItemModChart{ iLvl = 1, Name = "Accurate",    Stat = Stats.Precision, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Precise",    Stat = Stats.Precision, Value = 20 },
        new ItemModChart{ iLvl = 20, Name = "True Shot",    Stat = Stats.Precision, Value = 40 },

        new ItemModChart{ iLvl = 1,  Name = "Reinforced", Stat = Stats.Armor, Value = 2 },
        new ItemModChart{ iLvl = 10,  Name = "Stiffened", Stat = Stats.Armor, Value = 10 },

    };

    static ItemModChart[] possessive_prefixes = new ItemModChart[]
    {
        new ItemModChart{ iLvl = 1, Name = "Subscriber's", Stat = Stats.Intelligence, Value = 5 },
        new ItemModChart{ iLvl = 1, Name = "Quill's", Stat = Stats.Charisma, Value = 5 },
        new ItemModChart{ iLvl = 1, Name = "Essentia's", Stat = Stats.Wisdom, Value = 5 },
        new ItemModChart{ iLvl = 1, Name = "Gilbert's", Stat = Stats.Dexterity, Value = 5 },
        new ItemModChart{ iLvl = 18, Name = "Danika's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Programmer's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Demonac's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Pacifist's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Leader's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Orator's", Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 18, Name = "Moobot's", Stat = Stats.Damage, Value = 50 },
    };

    static ItemModChart[] suffixes = new ItemModChart[]
    {
        new ItemModChart{ iLvl = 1,  Name  = "Strength"  , Stat = Stats.Strength, Value = 5 },
        new ItemModChart{ iLvl = 5,  Name  = "Might"     , Stat = Stats.Strength, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Ogre Might", Stat = Stats.Strength, Value = 20 },
        new ItemModChart{ iLvl = 15, Name = "Smashing"    , Stat = Stats.Strength, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "Violence"     , Stat = Stats.Strength, Value = 40 },
        new ItemModChart{ iLvl = 25, Name = "Mighty Might"     , Stat = Stats.Strength, Value = 50 },
        new ItemModChart{ iLvl = 30, Name = "Brutality", Stat = Stats.Strength, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Slaying", Stat = Stats.Strength, Value = 100 },
        new ItemModChart{ iLvl = 40, Name = "Obliteration", Stat = Stats.Strength, Value = 150 },
        new ItemModChart{ iLvl = 45, Name = "Power Overwhelming", Stat = Stats.Strength, Value = 200 },
        new ItemModChart{ iLvl = 45, Name = "Power Underwhelming", Stat = Stats.Strength, Value = -200 },

        new ItemModChart{ iLvl = 1, Name = "Dexterity", Stat = Stats.Dexterity, Value = 5 },
        new ItemModChart{ iLvl = 1, Name = "Poise", Stat = Stats.Dexterity, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "Traveling", Stat = Stats.Dexterity, Value = 10 },
        new ItemModChart{ iLvl = 15, Name = "Swag", Stat = Stats.Dexterity, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "Covered in Bees", Stat = Stats.Dexterity, Value = 40 },
        new ItemModChart{ iLvl = 20, Name = "Chaos", Stat = Stats.Dexterity, Value = 40 },
        new ItemModChart{ iLvl = 25, Name = "Fury", Stat = Stats.Dexterity, Value = 50 },
        new ItemModChart{ iLvl = 30, Name = "Defenestration", Stat = Stats.Dexterity, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Dancing Phalanges", Stat = Stats.Dexterity, Value = 100 },


        new ItemModChart{ iLvl = 1, Name = "Constitution", Stat = Stats.Constitution, Value = 5 },
        new ItemModChart{ iLvl = 1, Name = "Endurance", Stat = Stats.Constitution, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "Redemption", Stat = Stats.Constitution, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Life", Stat = Stats.Constitution, Value = 20 },
        new ItemModChart{ iLvl = 20, Name = "The Sisterhood", Stat = Stats.Constitution, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "The Brotherhood", Stat = Stats.Constitution, Value = 30 },
        new ItemModChart{ iLvl = 30, Name = "Orichalchum", Stat = Stats.Constitution, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Tegernseer", Stat = Stats.Constitution, Value = 100 },

        new ItemModChart{ iLvl = 5, Name = "Intelligence", Stat = Stats.Intelligence, Value = 5 },
        new ItemModChart{ iLvl = 10, Name = "The Force", Stat = Stats.Intelligence, Value = 10 },
        new ItemModChart{ iLvl = 20, Name = "Elbereth", Stat = Stats.Intelligence, Value = 25 },
        new ItemModChart{ iLvl = 30, Name = "Insanity", Stat = Stats.Intelligence, Value = 40 },

        new ItemModChart{ iLvl = 1, Name = "Wisdom", Stat = Stats.Wisdom, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "The Owl", Stat = Stats.Wisdom, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "Chocolate", Stat = Stats.Wisdom, Value = 20 },
        new ItemModChart{ iLvl = 15, Name = "Honey", Stat = Stats.Wisdom, Value = 30 },
        new ItemModChart{ iLvl = 20, Name = "Waffles", Stat = Stats.Wisdom, Value = 40 },
        new ItemModChart{ iLvl = 25, Name = "Whisky", Stat = Stats.Wisdom, Value = 50 },
        new ItemModChart{ iLvl = 30, Name = "Salt", Stat = Stats.Wisdom, Value = 75 },
        new ItemModChart{ iLvl = 35, Name = "Antioch", Stat = Stats.Wisdom, Value = 100 },
        new ItemModChart{ iLvl = 40, Name = "God", Stat = Stats.Wisdom, Value = 150 },
        new ItemModChart{ iLvl = 45, Name = "Brussels", Stat = Stats.Wisdom, Value = 200 },

        new ItemModChart{ iLvl = 1, Name = "Charisma", Stat = Stats.Charisma, Value = 5 },
        new ItemModChart{ iLvl = 5, Name = "Beauty", Stat = Stats.Charisma, Value = 10 },
        new ItemModChart{ iLvl = 10, Name = "DLC", Stat = Stats.Charisma, Value = 25 },
        new ItemModChart{ iLvl = 20, Name = "Comet Sighted", Stat = Stats.Charisma, Value = 20 },
        new ItemModChart{ iLvl = 30, Name = "Too Many Words", Stat = Stats.Charisma, Value = 30 },

        new ItemModChart{ iLvl = 1, Name = "The Rabbit", Stat = Stats.Luck, Value = 5 },
        new ItemModChart{ iLvl = 10, Name = "The Kiss", Stat = Stats.Luck, Value = 20 },
        new ItemModChart{ iLvl = 25, Name = "Petra", Stat = Stats.Luck, Value = 50 },
        new ItemModChart{ iLvl = 35, Name = "RNGesus", Stat = Stats.Luck, Value = 100 },

        new ItemModChart{ iLvl = 20, Name = "The Tank", Stat = Stats.Armor, Value = 10 },

        new ItemModChart{ iLvl = 10, Name = "Cooling", Stat = Stats.ResFire, Value = 5 },
        new ItemModChart{ iLvl = 20, Name = "Flaming", Stat = Stats.ResFire, Value = 10 },

        new ItemModChart{ iLvl = 10, Name = "Warming", Stat = Stats.ResCold, Value = 5 },
        new ItemModChart{ iLvl = 20, Name = "Ice", Stat = Stats.ResCold, Value = 10 },

        new ItemModChart{ iLvl = 10, Name = "Rubber", Stat = Stats.ResElec, Value = 5 },
        new ItemModChart{ iLvl = 20, Name = "Insulated", Stat = Stats.ResElec, Value = 10 },

        new ItemModChart{ iLvl = 5, Name = "Regeneration", Stat = Stats.HealthRegen, Value = 0.1f },
        new ItemModChart{ iLvl = 20, Name = "Troll Blood", Stat = Stats.HealthRegen, Value = 0.25f },


    };

    static ItemModChart[] WeaponPrefixes = new ItemModChart[]
    {
        new ItemModChart{ iLvl = 1,  Name = "Broken",       Stat = Stats.Damage, Value = 5 },
        new ItemModChart{ iLvl = 1,  Name = "Rusty",        Stat = Stats.Damage, Value = 5 },
        new ItemModChart{ iLvl = 5,  Name = "Honed",        Stat = Stats.Damage, Value = 10 },
        new ItemModChart{ iLvl = 10,  Name = "Crushing",    Stat = Stats.Damage, Value = 20 },
        new ItemModChart{ iLvl = 15,  Name = "Sharp",       Stat = Stats.Damage, Value = 30 },
        new ItemModChart{ iLvl = 20,  Name = "Balanced",    Stat = Stats.Damage, Value = 40 },
        new ItemModChart{ iLvl = 25,  Name = "Tempered",    Stat = Stats.Damage, Value = 50 },
        new ItemModChart{ iLvl = 30,  Name = "Psionic",     Stat = Stats.Damage, Value = 75 },
    };

    static ItemModChart[] PotionPrefixes = new ItemModChart[]
    {
        new ItemModChart{ iLvl = 1,  Name = "Health",       Stat = Stats.HealthRegen, Value = 5 },
        new ItemModChart{ iLvl = 10,  Name = "Large Health",       Stat = Stats.HealthRegen, Value = 10 },
        new ItemModChart{ iLvl = 20,  Name = "Massive Health",       Stat = Stats.HealthRegen, Value = 25 },
    };


    static ItemSubType[] WeaponSubTypes = new ItemSubType[]
    {
        new ItemSubType{ Name = "Longsword",    Image = "Longsword2D", Width = 2, Height = 3},
        new ItemSubType{ Name = "Battle Axe",     Image = "Axe2D", Width = 2, Height = 3 },
        //new ItemSubType{ Name = "Scimitar",     Image = "Scimitar", Width = 2, Height = 3 },
        new ItemSubType{ Name = "Shortsword",   Image = "Shortsword2D", Width = 1, Height = 3 },
        //new ItemSubType{ Name = "Short Bow",    Image = "Bow", Width = 2, Height = 3 },
        //new ItemSubType{ Name = "Long Bow",     Image = "Bow", Width = 2, Height = 3 },
        //new ItemSubType{ Name = "Hunter's Bow", Image = "Bow", Width = 2, Height = 3 },
        //new ItemSubType{ Name = "Death Bow",    Image = "Bow", Width = 2, Height = 3 },
    };

    static Dictionary<ItemTypes, ItemSubType[]> ItemSubTypesBySlot;


    static ItemGenerator()
    {
        random = new System.Random();
        ItemSubTypesBySlot = new Dictionary<ItemTypes, ItemSubType[]>();
        ItemSubTypesBySlot[ItemTypes.Weapon] = WeaponSubTypes;

        
    }
    static System.Random random;
    static public void MakeItem(LootStats ls, int iLvl, int rarity)
    {
        System.Array values = System.Enum.GetValues(typeof(ItemTypes));
        
        ItemTypes randomBar = (ItemTypes)values.GetValue(random.Next(values.Length));

        MakeItem(ls, randomBar, iLvl, rarity);
    }

    static void ItemSizeByType(ItemTypes slot, ref int width, ref int height)
    {
        switch (slot)
        {
            case ItemTypes.Helmet:
                width = 2;
                height = 2;
                break;
            case ItemTypes.Armor:
                width = 2;
                height = 3;
                break;
            case ItemTypes.Weapon:
                width = 2;
                height = 3;
                break;
            case ItemTypes.Shield:
                width = 2;
                height = 3;
                break;
            case ItemTypes.Belt:
                width = 2;
                height = 1;
                break;
            case ItemTypes.Boots:
                width = 2;
                height = 2;
                break;
            case ItemTypes.Gloves:
                width = 2;
                height = 2;
                break;
            case ItemTypes.Trinket:
                width = 1;
                height = 1;
                break;
            case ItemTypes.Potion:
                width = 2;
                height = 2;
                break;
            default:
                width = 2;
                height = 2;
                Debug.LogError("Unknown item slot type.");
                break;
        }
    }

    static public void MakeItem( LootStats ls, ItemTypes slot, int iLvl, int rarity )
    {
        string name = slot.ToString();
        List<ItemMod> itemMods = new List<ItemMod>();

        string image_name = name;
        int width = 1;
        int height = 1;

        ItemSizeByType(slot, ref width, ref height);

        if (ItemSubTypesBySlot.ContainsKey(slot))
        {
            ItemSubType sub = ItemSubTypesBySlot[slot][Random.Range(0, ItemSubTypesBySlot[slot].Length)];
            name = sub.Name;
            width = sub.Width;
            height = sub.Height;
            image_name = sub.Image;
        }

        int numPrefixes = Random.Range(0, rarity+1);

        if(slot == ItemTypes.Weapon && numPrefixes==0)
        {
            numPrefixes = 1;
        }

        int numSuffixes = rarity - numPrefixes;

        if (numPrefixes > 3)
        {
            numSuffixes += numPrefixes - 3;
            numPrefixes = 3;
        }
        else if (numSuffixes > 3)
        {
            numPrefixes += numSuffixes - 3;
            numSuffixes = 3;
        }

        List<ItemModChart> legalPrefs = ModsOfLevel(prefixes, iLvl);

        if (slot == ItemTypes.Potion)
        {
            rarity = 2;
            numPrefixes = 1;
            numSuffixes = 0;

            legalPrefs = ModsOfLevel(PotionPrefixes, iLvl);
        }

        if (slot == ItemTypes.Weapon)
            numPrefixes--;

        for (int i = 0; i < numPrefixes; i++)
        {
            int r = Random.Range(0, legalPrefs.Count);
            int value = Mathf.CeilToInt(legalPrefs[r].Value * Random.Range(0.5f, 1.0f));

            if (value == 0)
                value = -1;

            itemMods.Add(
                new ItemMod(
                    legalPrefs[r].Stat,
                    value
                )
                );

            name = legalPrefs[r].Name + " " + name;
            legalPrefs.RemoveAt(r);
        }

        if (slot == ItemTypes.Weapon)
        {
            List<ItemModChart> legalWeaponPrefs = ModsOfLevel(WeaponPrefixes, iLvl);
            int r = Random.Range(0, legalWeaponPrefs.Count);

            itemMods.Add(
                new ItemMod(
                    legalWeaponPrefs[r].Stat,
                    Mathf.CeilToInt(legalWeaponPrefs[r].Value * Random.Range(0.5f, 1.0f))
                )
                );

            name = legalWeaponPrefs[r].Name + " " + name;
            legalWeaponPrefs.RemoveAt(r);
        }


        List<ItemModChart> legalSuffs = ModsOfLevel(suffixes, iLvl);
        for (int i = 0; i < numSuffixes; i++)
        {
            int r = Random.Range(0, legalSuffs.Count);

            itemMods.Add(
                new ItemMod(
                    legalSuffs[r].Stat,
                    Mathf.CeilToInt(legalSuffs[r].Value * Random.Range(0.5f, 1.0f))
                )
                );

            if (i == 0)
                name += " of ";
            else
                name += " and ";

            name += legalSuffs[r].Name;

            legalSuffs.RemoveAt(r);
        }


        // Copy all the data into the gameobject
        ls.gameObject.name = name;


        Image img = ls.transform.Find("Image").GetComponent<Image>();

        img.sprite = Resources.Load<Sprite>("ItemGFX/" + image_name);

        if (img.sprite == null)
        {
            Debug.Log("No sprite for: " + "ItemGFX/" + image_name);
        }

        img.color = Random.ColorHSV(0f, 1f, 0f, 0.5f, 0.8f, 1.0f);

        //if(img.sprite!= null)
        //    img.SetNativeSize();
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(50 * width, 50 * height);

        ls.itemType = slot;
        ls.itemMods = itemMods.ToArray();
        ls.Rarity = rarity;

        Loot loot = ls.GetComponent<Loot>();
        loot.GridWidth = width;
        loot.GridHeight = height;

        RectTransform rt = ls.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50 * width, 50 * height);
    }

    static List<ItemModChart> ModsOfLevel(ItemModChart[] allMods, int iLevel)
    {
        List<ItemModChart> ims = new List<ItemModChart>();
        foreach (ItemModChart im in allMods)
        {
            if(im.iLvl <= iLevel)
            {
                ims.Add(im);
            }
        }

        return ims;
    }
}