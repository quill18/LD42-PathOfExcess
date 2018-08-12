using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArtByDiff : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ImageAnim ia = GetComponent<ImageAnim>();

        switch(Difficulty.DiffLevel)
        {
            case 0:
                ia.Frames = Frames0;
                break;
            case 1:
                ia.Frames = Frames1;
                break;
            case 2:
                ia.Frames = Frames2;
                break;
        }
    }

    public Sprite[] Frames0;
    public Sprite[] Frames1;
    public Sprite[] Frames2;

}
