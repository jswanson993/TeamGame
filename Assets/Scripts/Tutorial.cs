using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    float time;
	// Use this for initialization
	void Start () {
        time = Time.deltaTime;
   
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time >= 7){
            Destroy(gameObject);
        }
    }
}
