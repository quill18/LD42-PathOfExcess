using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatEnemy : MonoBehaviour {

    void Start()
    {
        statManager = GameObject.FindObjectOfType<StatManager>();
        combatQueue = GameObject.FindObjectOfType<CombatQueue>();

        healthBar = this.transform.Find("HealthBarFrame/HealthBar").GetComponent<Image>();
    }

    float attackCooldown = 1;
    StatManager statManager;
    CombatQueue combatQueue;

    public AudioClip[] DeathSounds;

    int hitpoints = 10;
    int maxHitpoints = 10;
    int Level = 1;
    int Bossiness = 1;  // 1 = normal -- more than that is more epic

    float deathTimer = 0.5f;

    DamageType damageType = DamageType.Physical;

    Image healthBar;

    public void SetupEnemy(int level, int bossiness, Sprite sprite)
    {
        Level = level;
        Bossiness = bossiness;

        hitpoints = 8 * level * bossiness;
        maxHitpoints = hitpoints;

        GetComponent<Image>().sprite = sprite;

        if(bossiness > 1)
        {
            // Elites and bosses get assigned a random element!
            // ....maybe tint them?
            // TODO: Element symbol on their icon
            System.Array vals = System.Enum.GetValues(typeof(DamageType));
            System.Random random = new System.Random();

            damageType = (DamageType)vals.GetValue( random.Next(vals.Length) );
        }
    }

    // Update is called once per frame
    void Update () {
        if (PauseManager.isPaused || CombatPlayer.isDead)
            return;

        if(hitpoints <= 0)
        {
            deathTimer -= Time.deltaTime;

            if(deathTimer <= 0)
            {
                Destroy(gameObject);
                return;
            }

            GetComponent<CanvasGroup>().alpha = deathTimer / 0.5f;
            this.transform.position = this.transform.position + new Vector3(10, -25) * Time.deltaTime;

            return;
        }

        if(hitpoints >= maxHitpoints)
        {
            healthBar.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            healthBar.transform.parent.gameObject.SetActive(true);
            healthBar.fillAmount = (float)hitpoints / (float)maxHitpoints;
        }

        // Am I at the front of the queue?
        if (combatQueue.FirstEnemy() == this)
        {
            attackCooldown -= Time.deltaTime;

            if(attackCooldown <= 0)
            {
                DoAttack();
                attackCooldown = 1.25f;
            }
        }

	}

    void DoAttack()
    {
        CombatPlayer player = GameObject.FindObjectOfType<CombatPlayer>();
        if (player == null)
            return;

        int hitSkill = 50 + Level * 10;

        if (Random.Range(0, hitSkill) < Random.Range(0, player.Dodge()))
        {
            CombatBubbleSpawner.SpawnBubble(
                    player.transform.position,
                    "Dodged!",
                    CombatBubbleSpawner.BubbleTypes.Round,
                    Color.green
                );
            return;
        }

        int maxDamage = ((Bossiness-1)*2 + Level) * (1 + Difficulty.DiffLevel );

        int damage = Random.Range(maxDamage / 2, maxDamage);

        player.TakeDamage(damage, damageType);

        

    }

    public int Dodge()
    {
        return 50 + Level * 10;
    }

    public void TakeDamage(int amount)
    {
        hitpoints -= amount;

        if(hitpoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // TODO: Death animation
        LootArea lootArea = GameObject.FindObjectOfType<LootArea>();
        lootArea.SpawnLoot( this.transform.position, Level + (Bossiness*2) - 1, 100*(Bossiness*2-1));

        this.transform.SetParent( GetComponentInParent<Canvas>().transform );
        //Destroy(gameObject);

        SoundsManager.PlayRandomClip(DeathSounds);
    }
}
