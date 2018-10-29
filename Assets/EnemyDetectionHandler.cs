using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            transform.parent.GetComponent<ShootingEnemy>().isAlerted = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            transform.parent.GetComponent<ShootingEnemy>().isAlerted = false;
        }
    }
}
