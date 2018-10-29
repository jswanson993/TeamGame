using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {

    // Use this for initialization
    public int StartingHealth = 10;
    public GameObject particlePrefab;
    private int currentHealth;
    public float despawnTimer = 5f;
	void Start () {
        currentHealth = StartingHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int DamageTaken)
    {
        currentHealth = currentHealth - DamageTaken;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (transform.parent.GetComponent<ShootingEnemy>()!=null)
        {
            transform.parent.GetComponent<ShootingEnemy>().LRend.enabled = false;
            transform.parent.GetComponent<ShootingEnemy>().enabled = false;
        }
        if(GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        if (transform.parent.GetComponent<Collider>() != null)
        {
            transform.parent.GetComponent<Collider>().enabled = false;
        }
        if(particlePrefab != null)
        {
            Instantiate(particlePrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, despawnTimer);
    }
}
