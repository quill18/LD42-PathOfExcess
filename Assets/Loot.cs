using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Loot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // Use this for initialization
    void Start() {
        lootStats = GetComponent<LootStats>();
    }

    public AudioClip[] PickupSounds;
    public AudioClip[] DropSounds;
    public AudioClip[] PotionDrinkSounds;

    LootStats lootStats;

    public int GridWidth = 1;
    public int GridHeight = 1;

    static Loot DraggedLoot;

    BagSlot MySlot;

    public bool IsInBackpack()
    {
        if (MySlot != null && MySlot.Backpack != null)
            return true;

        return false;
    }

    public void RemoveFromSlot()
    {
        if(MySlot == null)
        {
            Debug.LogError("Why are we calling RemoveFromSlot when we have no slot?.");
            return;
        }

        // TODO: Remove from all slots
        if(MySlot.Backpack != null)
        {
            BagSlot[] slots = MySlot.Backpack.GetSlotsAt(MySlot.PosX, MySlot.PosY, GridWidth, GridHeight);

            foreach (BagSlot bs in slots)
            {
                bs.MyLoot = null;
            }

        }
        else
        {
            MySlot.MyLoot = null;

            // We need to remove the item stats to our character
            this.GetComponent<LootStats>().RemoveStats();

        }

        MySlot = null;
    }

    public void AddToSlot(BagSlot s)
    {
        MySlot = s;

        if (MySlot.Backpack == null)
        {
            // This is an inventory slot -- check for type match?
            //       Was already done in an earlier step
            MySlot.MyLoot = this;

            if( lootStats.itemType== ItemTypes.Potion)
            {
                SoundsManager.PlayRandomClip(PotionDrinkSounds);
            }

            // We need to apply the item stats to our character
            this.GetComponent<LootStats>().ApplyStats();
            return;
        }

        // Add to all slots
        BagSlot[] slots = MySlot.Backpack.GetSlotsAt(MySlot.PosX, MySlot.PosY, GridWidth, GridHeight);

        foreach(BagSlot bs in slots)
        {
            bs.MyLoot = this;
        }

    }

    static bool alreadyDidButton = false;

    // Update is called once per frame
    void Update() {

        if(CombatPlayer.isDead)
        {
            StopDrag();
            return;
        }

        if (DraggedLoot != this)
            return;

        RectTransform rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(GetComponentInParent<Canvas>().GetComponent<RectTransform>(),
            Input.mousePosition, Camera.main, out globalMousePos))
        {
            rt.position = globalMousePos + 
                (GetComponentInParent<Canvas>().scaleFactor * new Vector3(-25, 25, 0));
        }

        if(Input.GetMouseButtonUp(0) && alreadyDidButton == false)
        {
            //Debug.Log("----------------------------------");
            alreadyDidButton = true;
            StopDrag();
        }


    }

    void LateUpdate()
    {
        alreadyDidButton = false;
    }

    public void StartDrag(bool isSwap = false)
    {
        //Debug.Log("OnBeginDrag: " + gameObject.name);

        if (CombatPlayer.isDead)
            return;

        DraggedLoot = this;

        if (MySlot != null)
        {
            RemoveFromSlot();
        }

        if(isSwap == false)
        {
            SoundsManager.PlayRandomClip(PickupSounds);
        }

        //GetComponent<CanvasGroup>().interactable = false;
        //GetComponent<CanvasGroup>().blocksRaycasts = false;

        this.transform.SetParent(GetComponentInParent<Canvas>().transform);
        this.transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartDrag(false);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //StopDrag();
    }

    void StopDrag()
    {
        if (DraggedLoot != this)
            return;

        //Debug.Log("StopDrag " + gameObject.name);
        DraggedLoot = null;

        SoundsManager.PlayRandomClip(DropSounds);


        //GetComponent<CanvasGroup>().interactable = true;
        //GetComponent<CanvasGroup>().blocksRaycasts = true;


        // Do a raycast to see what's under the mouse and find stuff that
        // implements OnDrop

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach(RaycastResult rr in results)
        {
            BagSlot bs = rr.gameObject.GetComponent<BagSlot>();
            if(bs != null)
            {
                bs = bs.CanDropHere(this);

                // bs might be changed or could be null now

                if (bs != null )
                {
                    bs.DroppedLoot(this);
                }
                else
                {
                    ResetToLootArea();
                }

                return;
            }
        }



        // If we get here, we didn't hit a slot
        ResetToLootArea();
    }

    void ResetToLootArea()
    {
        this.transform.SetParent( GameObject.FindObjectOfType<LootArea>().transform );
    }

}
