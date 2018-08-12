using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        statManager = GameObject.FindObjectOfType<StatManager>();
        combatQueue = GameObject.FindObjectOfType<CombatQueue>();
        isDead = false;
    }

    public GameObject GameOverText;

    float attackCooldown = 1;
    StatManager statManager;
    CombatQueue combatQueue;
    float regenCooldown = 1;

    public AudioClip[] HurtSounds;
    public AudioClip[] AttackSounds;
    public AudioClip[] DeathSounds;
    public AudioClip[] MissSounds;


    public static bool isDead = false;

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.D))
        {
            statManager.SetValue(Stats.CurrentHP, 0);
        }

        if (isDead)
            return;

        if (PauseManager.isPaused)
            return;

        if (statManager.GetValue(Stats.CurrentHP) <= 0)
        {
            Die();
            return;
        }

        regenCooldown -= Time.deltaTime;
        if(regenCooldown <= 0)
        {
            regenCooldown += 1;
            statManager.ChangeValue(
                Stats.CurrentHP,
                statManager.GetValue(Stats.MaxHP) * (statManager.GetValueF(Stats.HealthRegen) / 100f)
                );
        }

        attackCooldown -= Time.deltaTime * (100f+statManager.GetValue(Stats.AttackSpeed))/100f;

        if(attackCooldown <= 0 && combatQueue.NumEnemies() > 0)
        {
            attackCooldown = 1;

            DoAttack();
        }
    }

    void Die()
    {
        isDead = true;

        GameOverText.SetActive(true);
        GameOverText.transform.SetAsLastSibling();

        SoundsManager.PlayClip(DeathSounds[Random.Range(0, DeathSounds.Length)]);

        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
        GetComponent<RectTransform>().position =
            GetComponent<RectTransform>().position +
            new Vector3(
                GetComponent<RectTransform>().sizeDelta.x + 25,
                -25) * GetComponentInParent<Canvas>().scaleFactor;
    }

    public void TakeDamage(int amount, DamageType dtype)
    {
        int mitigation = 0;
        switch (dtype)
        {
            case DamageType.Physical:
                mitigation = Random.Range(0, (int)statManager.GetValue(Stats.Armor));
                break;
            case DamageType.Fire:
                mitigation = Random.Range(0, (int)statManager.DerivedResFire());
                break;
            case DamageType.Electrical:
                mitigation = Random.Range(0, (int)statManager.DerivedResElec());
                break;
            case DamageType.Cold:
                mitigation = Random.Range(0, (int)statManager.DerivedResCold());
                break;
            default:
                break;
        }

        mitigation = Mathf.FloorToInt(Mathf.Min(amount * 0.75f, mitigation));

        int actualDamage = amount - mitigation;
        statManager.ChangeValue(Stats.CurrentHP, -(actualDamage));

        Color c = Color.white;
        switch (dtype)
        {
            case DamageType.Physical:
                c = Color.grey;
                break;
            case DamageType.Fire:
                c = Color.red;
                break;
            case DamageType.Electrical:
                c = Color.yellow;
                break;
            case DamageType.Cold:
                c = Color.blue;
                break;
            default:
                break;
        }

        if(actualDamage > 0)
        {
            SoundsManager.PlayClip( HurtSounds[ Random.Range(0, HurtSounds.Length) ]  );
        }

        CombatBubbleSpawner.SpawnBubble(
            this.transform.position,
            "-" + actualDamage,
            CombatBubbleSpawner.BubbleTypes.Round,
            c
        );
    }

    public int Dodge()
    {
        return statManager.GetValue(Stats.Dodge);
    }

    void DoAttack()
    {
        CombatEnemy enemy = combatQueue.FirstEnemy();
        if (enemy == null)
            return;

        int hitSkill = (int)statManager.DerivedPrecision();

        if( Random.Range(0, hitSkill) < Random.Range(0, enemy.Dodge()) )
        {
            CombatBubbleSpawner.SpawnBubble(
                enemy.transform.position,
                "Missed!",
                CombatBubbleSpawner.BubbleTypes.Round,
                Color.gray
            );

            SoundsManager.PlayClip(MissSounds[Random.Range(0, MissSounds.Length)]);

            return;
        }

        int maxDamage = (int)statManager.DerivedDamage();

        int damage = Random.Range(maxDamage / 2, maxDamage);

        bool isCrit = Random.Range(0, 100) < statManager.GetValue(Stats.Critical);
        if (isCrit)
        {
            // TODO: Crit event
            damage *= 2;
        }

        CombatBubbleSpawner.SpawnBubble(
            enemy.transform.position,
            damage.ToString(),
            (isCrit ? CombatBubbleSpawner.BubbleTypes.Spiky : CombatBubbleSpawner.BubbleTypes.Round),
            (isCrit ? Color.red : Color.white)
        );

        SoundsManager.PlayClip(AttackSounds[Random.Range(0, AttackSounds.Length)]);

        enemy.TakeDamage(damage);
    }
}
