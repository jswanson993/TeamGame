using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
           other.gameObject.GetComponent<Player_Controller>().getPickup(activatePickup());
           Destroy(this.gameObject);

        }
    }

    private string activatePickup() {
        return this.name;
    }
}
