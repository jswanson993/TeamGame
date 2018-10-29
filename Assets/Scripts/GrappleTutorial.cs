using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTutorial : MonoBehaviour {
    float time;
	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.enabled == true)
        {
            time += Time.deltaTime;
        }
        if(time >= 7)
        {
            Destroy(gameObject);
        }
	}

    public void enableTutorial()
    {
        //this.gameObject.SetActive(true);
    }
}
