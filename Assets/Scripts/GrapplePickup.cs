using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePickup : MonoBehaviour {


    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {

            other.gameObject.transform.parent.GetComponent<Grapple>().enableGrapple();
            Destroy(gameObject);
        }
    }
}
