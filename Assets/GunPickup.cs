using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.GetComponent<Player_Controller>().getPickup("Gun Pickup");
        }
    }
}
