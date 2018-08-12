using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootStats : MonoBehaviour {

    static bool MadeWeapon = false;

	// Use this for initialization
	void Start () {

        if (MadeWeapon)
        {
            ItemGenerator.MakeItem(this, iLvl, Rarity);
        }
        else
        {
            MadeWeapon = true;
            ItemGenerator.MakeItem(this, ItemTypes.Weapon, iLvl, Rarity);
        }

        
        identificationTimer = (Rarity-1) * 3;

        CrunchStats();

        loot = GetComponent<Loot>();

        GetComponent<Image>().color = GameObject.FindObjectOfType<RarityColor>().RarityColors[Rarity];

        transform.localScale = Vector3.one;
    }

    public ItemTypes itemType;
    public ItemMod[] itemMods;
    public int Rarity;
    public int iLvl;

    public float PotionDuration = 10;
    bool isActive = false;

    public float identificationTimer = 999;

    Loot loot;

	// Update is called once per frame
	void Update () {
		
        if(loot.IsInBackpack())
        {
            identificationTimer -= Time.deltaTime * (100f+StatManager.Instance.GetValue(Stats.Intelligence))/100f;
        }

        if(isActive && itemType == ItemTypes.Potion)
        {
            PotionDuration -= Time.deltaTime;
            if(PotionDuration <= 0)
            {
                loot.RemoveFromSlot();
                Destroy(gameObject);
            }
        }

	}

    public bool IdentInProgress()
    {
        return IsIdentified() == false && loot.IsInBackpack();
    }

    public bool IsIdentified()
    {
        return identificationTimer <= 0;
    }

    public float IdentTimeLeft()
    {
        return identificationTimer;
    }

    void CrunchStats()
    {
        List<ItemMod> newMods = new List<ItemMod>();

        foreach(ItemMod im in itemMods)
        {
            // Is this already in the list?
            bool wasFound = false;

            foreach(ItemMod nm in newMods)
            {
                if(nm.Stat == im.Stat)
                {
                    nm.Value += im.Value;
                    wasFound = true;
                    break;
                }
            }

            if(wasFound == false)
            {
                newMods.Add(im);
            }
        }

        itemMods = newMods.ToArray();
    }

    public void ApplyStats( int Adding=1 )
    {
        StatManager sm = GameObject.FindObjectOfType<StatManager>();

        foreach(ItemMod im in itemMods)
        {
            sm.ChangeValue(im.Stat, im.Value * Adding);
        }

        isActive = true;

    }

    public void RemoveStats()
    {
        ApplyStats(-1);
        isActive = false;

    }
}
