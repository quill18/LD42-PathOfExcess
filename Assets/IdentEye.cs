using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentEye : MonoBehaviour {

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        lootstats = GetComponentInParent<LootStats>();
	}

    Image image;
    LootStats lootstats;

    int dir = 1;
	
	// Update is called once per frame
	void Update () {

        if(lootstats.IdentInProgress() == false)
        {
            image.fillAmount = 0;
            return;
        }

        image.fillAmount += Time.deltaTime * dir;

        if (dir > 0 && image.fillAmount >= 1)
        {
            dir = -1;
            image.fillClockwise = !image.fillClockwise;
        }
        else if (dir < 0 && image.fillAmount <= 0)
        {
            dir = 1;
            image.fillClockwise = !image.fillClockwise;
        }

    }
}
