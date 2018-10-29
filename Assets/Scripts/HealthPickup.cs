using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {
    public int healthAmount = 20;
    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {

            other.gameObject.transform.parent.GetComponent<PlayerHealth>().gainHealth(20);
            Destroy(gameObject);
        }
    }
}
