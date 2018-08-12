using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxLevelLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        combatQueue = GameObject.FindObjectOfType<CombatQueue>();
        townPanel = GameObject.FindObjectOfType<TownPanel>();

    }

    CombatQueue combatQueue;
    TownPanel townPanel;

    // Update is called once per frame
    void Update () {
		GetComponent<Text>().text  = "Current Level: "+ combatQueue .Level + "\nDeepest Level: " + combatQueue.MaxLevel + "\nTown Portals Used: " + townPanel.TownPortalsUsed;
	}
}
