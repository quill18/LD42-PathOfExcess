using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipLoot : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Tooltip = GetComponent<Tooltip>();
        rarityColor = GameObject.FindObjectOfType<RarityColor>();
    }

    Tooltip Tooltip;

    RarityColor rarityColor;

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isPaused)
        {
            return;
        }


        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult rr in results)
        {
            LootStats ls = rr.gameObject.GetComponentInParent<LootStats>();

            if(ls != null)
            {
                ShowToolTip(ls);
                return;

            }
        }

        Tooltip.Hide();
    }

    void ShowToolTip(LootStats ls)
    {
        Text nameTxt = transform.Find("Item Name").GetComponent<Text>();
        Text statsTxt = transform.Find("Stats").GetComponent<Text>();

        nameTxt.color = rarityColor.RarityColors[ls.Rarity];

        if (ls.IsIdentified())
        {
            nameTxt.text = ls.name;

            statsTxt.text = "";

            foreach (ItemMod im in ls.itemMods)
            {
                if (statsTxt.text != "")
                    statsTxt.text += "\n";

                statsTxt.text += im.Stat.ToString() + (im.Value >= 0 ? " +" : " ") + im.Value;
            }
        }
        else
        {
            // NOT IDENTIFIED
            nameTxt.text = "UNIDENTIFIED";

            statsTxt.text = ls.IdentTimeLeft().ToString("#.##");

        }

        this.transform.SetAsLastSibling();
        Tooltip.Show();


    }
}
