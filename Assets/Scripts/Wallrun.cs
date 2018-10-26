using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Controller))]
public class Wallrun : MonoBehaviour {

    // Use this for initialization
    Player_Controller p_controller;
    Rigidbody playerRB;
    

	void Start () {
        p_controller = GetComponent<Player_Controller>();
        playerRB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(p_controller.jState == Player_Controller.JumpState.InAir && Input.GetButtonDown("Jump"))
        {
            tryWallRun();
        }
        else if(p_controller.jState == Player_Controller.JumpState.Wallrunning)
        {
            checkWallRelease();
        }

	}

    private bool checkWallRelease()
    {
        RaycastHit hit;
        int layerMask = 1 << 11;

        layerMask = ~layerMask;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            /*
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Right Side");
            GetComponent<Rigidbody>().AddForce(-Camera.main.transform.right * 700*3 + (Camera.main.transform.up*1000) *3);
            */
            
            return false;
        }

        else if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            /*
            Debug.DrawRay(Camera.main.transform.position, -Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Left Side");
            GetComponent<Rigidbody>().AddForce(Camera.main.transform.right * 700 *3 + (Camera.main.transform.up * 1000)*3);
            */
            
            return false;
        }
        else
        {
            playerRB.useGravity = true;
            p_controller.jState = Player_Controller.JumpState.InAir;
            return true;
        }
    }

    private void tryWallRun()
    {
        int layerMask = 1 << 11;

        layerMask = ~layerMask;

        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            /*
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Right Side");
            GetComponent<Rigidbody>().AddForce(-Camera.main.transform.right * 700*3 + (Camera.main.transform.up*1000) *3);
            */
            Debug.Log("GravOFF");
            p_controller.jState = Player_Controller.JumpState.Wallrunning;
            playerRB.velocity = Vector3.Scale(playerRB.velocity, new Vector3(1, 0, 1));
            playerRB.useGravity = false;
        }

        else if(Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hit, p_controller.wallRunSnapDistance, layerMask))
        {
            /*
            Debug.DrawRay(Camera.main.transform.position, -Camera.main.transform.right * hit.distance, Color.yellow);
            Debug.Log("Hit Left Side");
            GetComponent<Rigidbody>().AddForce(Camera.main.transform.right * 700 *3 + (Camera.main.transform.up * 1000)*3);
            */
            Debug.Log("GravOFF");
            p_controller.jState = Player_Controller.JumpState.Wallrunning;
            playerRB.useGravity = false;
        }

        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("WwallRun failed");
        }
    }
}
