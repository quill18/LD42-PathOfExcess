using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatQueue : MonoBehaviour {

	// Use this for initialization
	void Start () {
        velocity = new Vector2[maxQueue];
	}

    int maxQueue = 5;
    Vector2[] velocity;

    public GameObject EnemyPrefab;  // TODO: MOAR ENEMIES

    public Sprite[] EnemySpritesTier1;
    public Sprite[] EnemySpritesTier2;
    public Sprite[] EnemySpritesTier3;

    int waveCounter = 0;

    [System.NonSerialized]
    public int Level = 0;
    [System.NonSerialized]
    public int MaxLevel = 0;

    int bossWaveID = 3;

    public void LeaveDungeon()
    {
        // wipe out the enemies
        while(this.transform.childCount > 0)
        {
            Transform t = this.transform.GetChild(0);
            t.SetParent(null);  // BECOME BATMAN
            Destroy(t.gameObject);
        }

        Level = (Level) / 2;
        waveCounter = 0;

    }

    public void ReturnFromTown()
    {
    }

    void SpawnWave()
    {
        Level += 1; // Level up each wave

        if (Level > MaxLevel)
            MaxLevel = Level;

        SpawnEnemy(1);
        SpawnEnemy(1);
        SpawnEnemy(1);
        SpawnEnemy(1);

        if (waveCounter % bossWaveID == (bossWaveID-1))
        {
            SpawnEnemy(3);
        }
        else
        {
            SpawnEnemy(2);
        }

        waveCounter++;
    }

    // Update is called once per frame
    void Update () {
        if (PauseManager.isPaused)
            return;


        // Move enemies to their right position (visually)

        if (this.transform.childCount <= 0)
        {
            // TODO: Trigger item sell-off

            //if (waveCounter % bossWaveID == 0)
            //{
            //    GameObject.FindObjectOfType<Backpack>().SellOffLoot();
            //}

            SpawnWave();
            return;
        }
            

        float widthSoFar = 0;
        float padding = 8;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            RectTransform rt = this.transform.GetChild(i).GetComponent<RectTransform>();

            float pixelWidth = rt.sizeDelta.x;

            Vector2 desiredPos = new Vector2(widthSoFar, 0);

            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, desiredPos, ref velocity[i], 1.0f);

            widthSoFar += pixelWidth + padding;
        }

    }

    public int NumEnemies()
    {
        return transform.childCount;
    }

    public CombatEnemy FirstEnemy()
    {
        if (NumEnemies() <= 0)
            return null;

        return this.transform.GetChild(0).GetComponent<CombatEnemy>();
    }

    void SpawnEnemy(int enemyTier)
    {
        GameObject enemyGO = Instantiate(EnemyPrefab, this.transform);
        enemyGO.GetComponent<RectTransform>().anchoredPosition =
            new Vector2( 
                this.GetComponent<RectTransform>().rect.center.x,
                0
                );

        CombatEnemy enemy = enemyGO.GetComponent<CombatEnemy>();

        Sprite sprite = EnemySpritesTier1[Random.Range(0, EnemySpritesTier1.Length)];
        switch (enemyTier)
        {
            case 2:
                sprite = EnemySpritesTier2[Random.Range(0, EnemySpritesTier2.Length)];
                break;
            case 3:
                sprite = EnemySpritesTier3[Random.Range(0, EnemySpritesTier3.Length)];
                break;
            default:
                break;
        }

        enemy.SetupEnemy(Level, enemyTier, sprite);
    }
}
