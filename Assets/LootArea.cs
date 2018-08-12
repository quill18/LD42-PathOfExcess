using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
        currentVelocity = new Dictionary<RectTransform, Vector2>();
        statManager = GameObject.FindObjectOfType<StatManager>();
    }

    StatManager statManager;

    float rot;
    float rotationSpeed = 60;

    float distFromCenter = 125;

    int maxItems = 6;

    Dictionary<RectTransform, Vector2> currentVelocity;

    public GameObject LootPrefab;

    public void ClearLoot()
    {
        while (this.transform.childCount > 0)
        {
            Transform t = this.transform.GetChild(0);
            t.SetParent(null); // I'M BATMAN
            Destroy(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {

        if (this.transform.childCount > maxItems)
        {
            Transform t = this.transform.GetChild(0);
            t.SetParent(null); // I'M BATMAN
            Destroy(t.gameObject);
        }


        //rot += Time.deltaTime * rotationSpeed;


        int numChildren = transform.childCount;

        float anglePerChild = 360f / numChildren;

        for (int i = 0; i < numChildren; i++)
        {
            RectTransform t = transform.GetChild(i).GetComponent<RectTransform>();

            float a = rot + anglePerChild * i;

            Vector2 offset = Quaternion.Euler(0, 0, a) * new Vector3(distFromCenter, 0, 0);

            Vector2 desiredPosition = GetComponent<RectTransform>().rect.center 
                + offset
                + ( new Vector2(-t.rect.size.x/2, t.rect.size.y/2) )
                ;


            Vector2 vel = Vector2.zero;
            
            if(currentVelocity.ContainsKey(t))
                vel = currentVelocity[t];

            t.anchoredPosition = Vector2.SmoothDamp(t.anchoredPosition, desiredPosition, ref vel, 0.5f);

            currentVelocity[t] = vel;
        }

	}

    public void SpawnLoot( Vector2 pos, int iLevel, int dropRate )
    {
        //Debug.Log("SpawnLoot");
        int luck = statManager.GetValue(Stats.Luck);
        int numItems = Mathf.CeilToInt( (Random.Range(0, dropRate+ luck)) / 100f );

        for (int i = 0; i < numItems; i++)
        {

            GameObject lootGO = Instantiate(LootPrefab);
            LootStats ls = lootGO.GetComponent<LootStats>();
            ls.iLvl = iLevel;

            int rarityRoll = Random.Range(Mathf.Min(luck,50), 101 + (luck/5) );

            if (rarityRoll < 70)
            {
                ls.Rarity = 1;
            }
            else if (rarityRoll < 80)
            {
                ls.Rarity = 2;
            }
            else if (rarityRoll < 95)
            {
                ls.Rarity = 3;
            }
            else if (rarityRoll < 97)
            {
                ls.Rarity = 4;
            }
            else if (rarityRoll < 99)
            {
                ls.Rarity = 5;
            }
            else
            {
                ls.Rarity = 6;
            }

            lootGO.transform.SetParent(this.transform);
            lootGO.transform.position = (Vector3)pos;
        }
    }
}
