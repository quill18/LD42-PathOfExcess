using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BagSlot : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}

    public Loot MyLoot { get; set; }

    public int PosX;
    public int PosY;
    public Backpack Backpack;

    public string BagName()
    {
        return "BagSlot_" + PosX + "_" + PosY;
    }

    // Update is called once per frame
    void Update () {
		
	}


    /// <summary>
    /// Return the "true" bag slot that this item will get placed in,
    /// or null if there's no space here
    /// </summary>
    /// <param name="l"></param>
    /// <returns></returns>
    public BagSlot CanDropHere(Loot l)
    {
        if(Backpack == null)
        {
            // This is char inventory
            LootStats ls = l.GetComponent<LootStats>();
            if ( ls.IsIdentified() == false || ls.itemType.ToString() != this.gameObject.name )
            {
                return null;
            }

            return this;
        }

        int x = PosX;
        int y = PosY;

        // First, correct if we're too close to the edge

        if (x + l.GridWidth > Backpack.BagWidth)
        {
            x = Backpack.BagWidth - l.GridWidth;
        }
        if (y + l.GridHeight > Backpack.BagHeight)
        {
            y = Backpack.BagHeight - l.GridHeight;
        }

        if(x != PosX || y != PosY)
        {
            // position was corrected, so send job to new tile
            return Backpack.GetSlotAt(x, y);
        }

        // Correct bag slot, so check that all slots that will be used for this item
        // have at most one other item
        if( Backpack.GetLootsInArea(x, y, l.GridWidth, l.GridHeight).Length > 1 )
        {
            // Not a valid tile for placement
            return null;
        }

        return this;
    }

    public void DroppedLoot(Loot l)
    {
        //Debug.Log("DroppedLoot: " + l.gameObject.name);

        if (Backpack != null)
        {
            Loot[] loots = Backpack.GetLootsInArea(PosX, PosY, l.GridWidth, l.GridHeight);
            if (loots.Length > 0)
            {
                if (loots.Length > 1)
                {
                    Debug.LogError("MORE THAN ONE LOOT?!?!?!?");
                    return;
                }

                loots[0].RemoveFromSlot();
                loots[0].StartDrag(true);
            }

        }
        else if( MyLoot != null )
        {
            Loot mine = MyLoot;
            mine.RemoveFromSlot();
            mine.StartDrag(true);
        }


        l.AddToSlot(this);

        Vector3 offset = Vector3.zero;

        if(Backpack != null)
        {
            offset = new Vector3(
                this.GetComponent<RectTransform>().sizeDelta.x / 2f,
                -this.GetComponent<RectTransform>().sizeDelta.y / 2f,
                0
                );

        }
        else
        {
            // Center in inventory slot (mostly for weapon)
            offset = new Vector3(
                -this.GetComponent<RectTransform>().sizeDelta.x / 2f +MyLoot.GetComponent<RectTransform>().sizeDelta.x /2f,
                -this.GetComponent<RectTransform>().sizeDelta.y / 2f +
                MyLoot.GetComponent<RectTransform>().sizeDelta.y / 2f
                );
        }

        offset *= GetComponentInParent<Canvas>().scaleFactor;


        MyLoot.GetComponent<RectTransform>().position = 
            this.GetComponent<RectTransform>().position
            - offset;
    }
}
