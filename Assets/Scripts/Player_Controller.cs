﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {


    #region "Variables"
    public Rigidbody Rigid;
    public float MouseSensitivity;
    public float MoveSpeed;
    public float JumpForce;
    public float groundCheckDistance;
    #endregion
    static bool playerCanJump;
    Vector2 rotation = new Vector2(0, 0);
    Transform shotPoint;

    // Use this for initialization
    void Start () {

        Rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        playerCanJump = true;
        shotPoint = transform.Find("Camera/FP_Gun/Gun/FirePoint");

    }
	
	// Update is called once per frame
	void Update () {

        processMovementInput();
        processButtonInput();
	}

    private void processButtonInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
        
    }

    private void shoot()
    {
        Vector3 endpoint;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, transform.TransformDirection(Camera.main.transform.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            endpoint = hit.point;
            ////*
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = endpoint;
            Destroy(cube, 1);
            ///*/

        }
        else
        {
            Debug.DrawRay(Camera.main.transform.position, transform.TransformDirection(Camera.main.transform.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            endpoint = transform.TransformDirection(Camera.main.transform.forward) * 1000;
        }
        
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] {shotPoint.position, endpoint});
        Invoke("RemoveTrail", .06f);
        ///*
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = endpoint;
        Destroy(cube2, 1);
        ///*/

    }

    private void RemoveTrail()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    private void processMovementInput()
    {
        TestRotation();
        Vector3 VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed;
        Vector3 HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed;
        Rigid.MovePosition(transform.position + VShift + HShift);
        if (Input.GetKeyDown("space") && groundTest())
            Rigid.AddForce(transform.up * JumpForce);
    }

    private bool groundTest()
    {
        if (!playerCanJump)
        {
            return false;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.up), out hit, Mathf.Infinity))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(-transform.up) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                if(hit.distance < groundCheckDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }

    private void TestRotation()
    {
        rotation.x += -Input.GetAxis("Mouse Y") * MouseSensitivity;
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);
        rotation.y += Input.GetAxis("Mouse X") * MouseSensitivity;
        //rotation.y = Mathf.Clamp(rotation.y, 0, 90);
        Camera.main.transform.eulerAngles = (Vector2)rotation * MouseSensitivity;
    }
}
