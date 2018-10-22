using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour {

    public string takesDamage;
    public string damageFunction = "takeDamage";
    public int damageDealt = 20;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(takesDamage)){
            other.SendMessage(damageFunction, damageDealt, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerStay(Collider other) {

        if (other.CompareTag(takesDamage)) {
            other.SendMessage(damageFunction, damageDealt, SendMessageOptions.DontRequireReceiver);
        }
     
    }
}
