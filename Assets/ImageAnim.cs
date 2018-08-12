using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}

    Image image;

    public Sprite[] Frames;
    int FPS = 3;
    float timer = 0;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * FPS;
        int frame = (int)timer % Frames.Length;
        image.sprite = Frames[frame];
	}
}
