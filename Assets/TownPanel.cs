using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        combatQueue = GameObject.FindObjectOfType<CombatQueue>();
        Hide();
	}

    CombatQueue combatQueue;

    RectTransform rt;
    Vector2 velocity;

    public int TownPortalsUsed = 0;

    public AudioClip PortalSound;
    public AudioClip WalkSound;

    public Text itemNameTxt;
    public Text statGainTxt;

    Vector2 desiredPosition;

    float PortalCooldown = 0;
    float PortalCooldownTime = 30f;

    public Image PortalCooldownImage;
    public Image LowHealthImage;

    float blinkTimer = 0;

    bool lowHealth = false;

	// Update is called once per frame
	void Update () {
        if (PauseManager.isPaused == false)
        {
            PortalCooldown += Time.deltaTime / PortalCooldownTime;
            if (PortalCooldown > 1)
                PortalCooldown = 1;
        }

        blinkTimer += Time.deltaTime * 5f;

        if (StatManager.Instance.GetValue(Stats.CurrentHP) < StatManager.Instance.GetValue(Stats.MaxHP) * 0.25f)
        {
            lowHealth = true;
        }
        else if (StatManager.Instance.GetValue(Stats.CurrentHP) > StatManager.Instance.GetValue(Stats.MaxHP) * 0.30f)
        {
            lowHealth = false;
        }

        bool blinkOn = Mathf.FloorToInt(blinkTimer / 2) % 2 == 0;

        if (lowHealth  && blinkOn)
        {
            if(PortalCooldown >= 1)
                PortalCooldownImage.color = Color.red;

        }
        else
        {
            PortalCooldownImage.color = Color.white;
        }

        LowHealthImage.enabled = lowHealth;


        PortalCooldownImage.fillAmount = PortalCooldown;

        rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, desiredPosition, ref velocity, 0.25f);

    }

    public void Show()
    {
        if (CombatPlayer.isDead)
            return;

        if (PortalCooldown < 1)
            return;

        SoundsManager.PlayClip(PortalSound);

        TownPortalsUsed++;

        PauseManager.isPaused = true;
        PortalCooldown = 0;
        PortalCooldownTime += 10f;// + (Difficulty.DiffLevel * 5);

        combatQueue.LeaveDungeon();
        GameObject.FindObjectOfType<LootArea>().ClearLoot();

        itemNameTxt.text = "";
        statGainTxt.text = "";

        DoSale();

        this.transform.SetAsLastSibling();

        StatManager.Instance.HealPlayer();

        // Slide In
        desiredPosition = new Vector2(0, 0);

    }

    public void Hide()
    {
        PauseManager.isPaused = false;

        combatQueue.ReturnFromTown();

        SoundsManager.PlayClip(WalkSound);

        // Slide Out

        desiredPosition = new Vector2(1280, 0);



    }

    public void DoSale()
    {
        GameObject.FindObjectOfType<Backpack>().SellOffLoot(itemNameTxt, statGainTxt);
    }
}
