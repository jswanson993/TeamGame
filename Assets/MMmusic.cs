using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMmusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameObject.Find("MusicManager") != null)
        {
            GameObject.Find("MusicManager").GetComponent<AudioSource>().Stop();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
