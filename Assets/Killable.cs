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
    private bool isDead;
    private MeshRenderer meshRenderer;
    Color lerpedColor = Color.white;
    Color baseColor;
    void Start () {
        
        currentHealth = StartingHealth;
        meshRenderer = GetComponent<MeshRenderer>();
        baseColor = meshRenderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int DamageTaken)
    {

        currentHealth = currentHealth - DamageTaken;
        StopAllCoroutines();
        StartCoroutine("HitFlash");

        if (currentHealth <= 0)
        {
            if(!isDead)
                Die();
        }
    }

    private IEnumerator HitFlash()
    {
        //meshRenderer.material.color = lerpedColor;

        for (float f = 0f; f <= 100;f+=.1f)
        {
            lerpedColor = Color.Lerp(lerpedColor, baseColor, Time.deltaTime*3);
            meshRenderer.material.color = lerpedColor;
            yield return new WaitForSeconds(.01f);
        }
    }

    private void Die()
    {
        isDead = true;
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
