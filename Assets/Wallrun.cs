using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Controller))]
public class Wallrun : MonoBehaviour {

    // Use this for initialization
    Player_Controller p_controller;

	void Start () {
        p_controller = GetComponent<Player_Controller>();	
	}
	
	// Update is called once per frame
	void Update () {
		
        if(p_controller.jState == Player_Controller.JumpState.InAir && Input.GetButtonDown("Jump"))
        {
            tryWallRun();
        }

	}

    private void tryWallRun()
    {
        int layerMask = 1 << 11;

        layerMask = ~layerMask;

        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Right Side");
            GetComponent<Rigidbody>().AddForce(-Camera.main.transform.right * 100);
        }

        else if(Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            Debug.DrawRay(Camera.main.transform.position, -Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Left Side");
        }

        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("WwallRun failed");
        }
    }
}
