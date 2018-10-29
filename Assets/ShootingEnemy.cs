using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour {

    // Use this for initialization
    public bool isAlerted;
    public Transform shotOrgin;
    private GameObject player;
    public float shotCooldown = 1;
    private float cooldownTimer;
    public int damagePerShot = 5;
    private LineRenderer LRend;
	void Start () {
        LRend = GetComponent<LineRenderer>();
        LRend.enabled = false;
        cooldownTimer = 0;
        if (GameObject.Find("PlayerFP(Clone)"))
        {
            player = GameObject.Find("PlayerFP(Clone)");
        }
        else
            GameObject.Find("Player2D(Clone)");

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
