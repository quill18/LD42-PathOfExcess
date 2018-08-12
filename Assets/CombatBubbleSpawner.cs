using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatBubbleSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        instance = this;
    }

    public GameObject CombatBubblePrefab;

    static CombatBubbleSpawner instance;

    public enum BubbleTypes { Round, Spiky };

    public Sprite[] BubbleSprites;

    public static void SpawnBubble(Vector2 pos, string text, BubbleTypes type, Color color)
    {
        GameObject go = Instantiate(instance.CombatBubblePrefab, instance.transform);
        go.transform.position = (Vector3)(pos);
        go.GetComponentInChildren<Text>().text = text;

        Image img = go.GetComponentInChildren<Image>();
        img.sprite = instance.BubbleSprites[(int)type];
        img.color = color;
    }

}
