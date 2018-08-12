using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour {

	// Use this for initialization
	void Start () {

        RectTransform rt = BagSlotPrefab.GetComponent<RectTransform>();

        float pixelWidth = rt.sizeDelta.x;
        float pixelHeight = rt.sizeDelta.y;

        bagSlots = new BagSlot[BagWidth,BagHeight];

        for (int x = 0; x < BagWidth; x++)
        {
            for (int y = 0; y < BagHeight; y++)
            {
                GameObject go = Instantiate(BagSlotPrefab, this.transform);
                BagSlot bs = go.GetComponent<BagSlot>();
                bagSlots[x,y] = bs;
                bs.PosX = x;
                bs.PosY = y;
                bs.Backpack = this;
                go.name = bs.BagName();

                rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(
                         (pixelWidth + Padding)  * x + Padding + rt.sizeDelta.x/2,
                         (pixelHeight + Padding) * -y + Padding - rt.sizeDelta.y/2 );

                Image img = bs.GetComponent<Image>();
                img.sprite = BagSlotImages[ Random.Range(0, BagSlotImages.Length) ];

                //rt.pivot = new Vector2(.5f, .5f);
                rt.rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 4));
                //rt.pivot = new Vector2(0, 0);

            }
        }
	}

    public Sprite[] BagSlotImages;

    BagSlot[,] bagSlots;

    public GameObject BagSlotPrefab;

    public int BagWidth = 12;
    public int BagHeight = 4;

    int Padding = 0;

	// Update is called once per frame
	void Update () {
		
	}

    public BagSlot GetSlotAt(int x, int y)
    {
        return bagSlots[x, y];
    }

    public BagSlot[] GetSlotsAt(int x, int y, int w, int h)
    {
        if (x < 0 || y < 0 || x + w > BagWidth || y + h > BagHeight)
        {
            Debug.Log("GetLootsInArea -- out of bounds");
            return null;
        }

        // returns all lots in an area of the bag
        List<BagSlot> slots = new List<BagSlot>();

        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                slots.Add(GetSlotAt(i, j));
            }
        }

        return slots.ToArray();
    }


    public Loot[] GetLootsInArea(int x, int y, int w, int h)
    {
        if (x < 0 || y < 0 || x + w > BagWidth || y + h > BagHeight)
        {
            Debug.Log("GetLootsInArea -- out of bounds");
            return null;
        }

        // returns all lots in an area of the bag
        List<Loot> loots = new List<Loot>();

        for (int i = x; i < x+w; i++)
        {
            for (int j = y; j < y+h; j++)
            {
                BagSlot bs = GetSlotAt(i, j);
                if(bs.MyLoot != null && loots.IndexOf(bs.MyLoot) == -1)
                {
                    loots.Add(bs.MyLoot);
                }
            }
        }

        return loots.ToArray();
    }

    public void SellOffLoot(Text itemNameTxt, Text statGainTxt)
    {
        Dictionary<Stats, float> statGains = new Dictionary<Stats, float>();

        itemNameTxt.text = "Sold Items:\n\n";

        for (int x = 0; x < BagWidth; x++)
        {
            for (int y = 0; y < BagHeight; y++)
            {
                Loot l = bagSlots[x, y].MyLoot;
                if (l == null)
                    continue;

                LootStats ls = l.GetComponent<LootStats>();

                itemNameTxt.text += ls.name + "\n";

                float perc = 0.1f;
                if (ls.itemType == ItemTypes.Potion)
                {
                    perc = 0.01f * (ls.PotionDuration / 10f);
                }
                    

                l.RemoveFromSlot();
                foreach(ItemMod im in ls.itemMods)
                {
                    float gain = im.Value * perc * (100f+StatManager.Instance.GetValueF(Stats.Charisma))/100f;

                    if(statGains.ContainsKey(im.Stat))
                    {
                        statGains[im.Stat] += gain;
                    }
                    else
                    {
                        statGains[im.Stat] = gain;
                    }

                    StatManager.Instance.ChangeValue(im.Stat, gain);
                }

                Destroy(ls.gameObject);
            }
        }

        statGainTxt.text = "Total Stats Gained:\n\n";

        foreach(Stats s in statGains.Keys)
        {
            if (statGains[s] == 0)
                continue;

            statGainTxt.text += s.ToString() + (statGains[s] >= 0 ? " +" : " ") + statGains[s].ToString("0.#") + "\n";
        }
    }
}
