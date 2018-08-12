using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlayClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

    public static void PlayRandomClip(AudioClip[] clips)
    {
        PlayClip(clips[Random.Range(0, clips.Length)]);
    }
}
