using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBubble : MonoBehaviour {

	// Use this for initialization
	void Start () {
        speed = new Vector2( Random.Range(-10, 10) , 50);

    }

    float lifespan = 0.75f;
    Vector2 speed;
	
	// Update is called once per frame
	void Update () {
        lifespan -= Time.deltaTime;
        if(lifespan <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if(lifespan < 0.25f)
        {
            this.GetComponent<CanvasGroup>().alpha = lifespan / 0.25f;
        }

        this.transform.position = this.transform.position + (Vector3)speed * Time.deltaTime;
	}

}
