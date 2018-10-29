using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour {

    // Use this for initialization
    public bool isAlerted;
    public Transform shotOrgin;
    public GameObject player;
    public float shotCooldown = 1;
    private float cooldownTimer;
    public int damagePerShot = 5;
    public LineRenderer LRend;
	void Start () {
        LRend = GetComponent<LineRenderer>();
        LRend.enabled = false;
        cooldownTimer = 0;
        

    }
	
	// Update is called once per frame
	void Update () {
        cooldownTimer += Time.deltaTime;
        if (isAlerted)
        {
            TryShoot();
        }
	}

    private void TryShoot()
    {
        if (GameObject.FindObjectOfType<Player_Controller>())
        {
            player = GameObject.FindObjectOfType<Player_Controller>().gameObject;
        }
        else
            player = GameObject.FindObjectOfType<Player_Controller>().gameObject;
        RaycastHit hit;
        if(Physics.Raycast(shotOrgin.position, player.transform.position - shotOrgin.position, out hit, Mathf.Infinity) && cooldownTimer > shotCooldown)
        {
            Debug.DrawRay(shotOrgin.position, player.transform.position - shotOrgin.position);
            if(hit.collider.tag == "Player")
                Shoot(hit);
        }
    }

    private void Shoot(RaycastHit hit)
    {
        LRend.SetPositions(new Vector3[] {shotOrgin.position, hit.point });
        LRend.enabled = true;
        Invoke("ClearLine", .2f);
        cooldownTimer = 0;
        player.GetComponent<PlayerHealth>().takeDamage(damagePerShot);
    }

    void ClearLine()
    {
        LRend.enabled = false;
    }
}
