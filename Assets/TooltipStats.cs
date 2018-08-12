using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipStats : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        tooltip = GetComponent<Tooltip>();
        lootTooltip = GameObject.FindObjectOfType<TooltipLoot>().GetComponent<Tooltip>();
    }

    Tooltip tooltip;
    Tooltip lootTooltip;

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isPaused)
        {
            return;
        }

        if(lootTooltip.isShowing)
        {
            tooltip.Hide();
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
            StatTooltipInfo sti = rr.gameObject.GetComponentInParent<StatTooltipInfo>();

            if (sti != null)
            {
                ShowToolTip(sti);
                return;

            }
        }

        tooltip.Hide();
    }

    void ShowToolTip(StatTooltipInfo sti)
    {
        Text nameTxt = transform.Find("Item Name").GetComponent<Text>();
        Text statsTxt = transform.Find("Stats").GetComponent<Text>();

        nameTxt.text = sti.Stat.ToString();

        statsTxt.text = sti.StatDescription();

        this.transform.SetAsLastSibling();
        tooltip.Show();
    }
}
