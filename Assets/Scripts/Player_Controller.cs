﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [Header("Player Movement")] [Space(15)]
    public float MouseSensitivity;
    public float MoveSpeed;
    public float JumpForce;
    public float groundCheckDistance;
    public float wallRunSnapDistance;
    public float MaxMoveSpeed;
    static bool playerCanJump;

    private Rigidbody Rigid;
    private Vector2 rotation = new Vector2(0, 0);
    private Transform shotPoint;
    private Vector3 wallRunVector;
    private Rigidbody p_rigidbody;

    public static bool is3D;

    public enum JumpState {Grounded, InAir, Wallrunning};
    JumpState jState;

    // Use this for initialization
    void Start () {

        p_rigidbody = GetComponent<Rigidbody>(); 
        jState = JumpState.Grounded;
        Rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        playerCanJump = true;
        shotPoint = transform.Find("Camera/FP_Gun/Gun/FirePoint");
        is3D = true;

    }
	
	// Update is called once per frame
	void Update () {

        
        processButtonInput();

        /*
        if(jState == JumpState.Wallrunning)
        {
            wallRun();
        }
        else
        {
            groundTest();
        }
        */
        groundTest();
        Debug.Log(jState.ToString());
	}

    void FixedUpdate()
    {
        if (is3D) { processFPMovementInput(); }
        else { process2DMovementInput(); }
    }

    private void wallRun()
    {
        RaycastHit filteredHit;
        Vector3 rayDirection;

        RaycastHit hitR;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hitR, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("RightSide WallRunHit");
            
        }

        RaycastHit hitL;
        if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hitL, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("LeftSide WallRunHit");
            
        }

        if(hitL.distance > hitR.distance)
        {
            filteredHit = hitR;
            rayDirection = Camera.main.transform.right;
        }
        else
        {
            filteredHit = hitL;
            rayDirection = -Camera.main.transform.right;
        }

        if (Physics.Raycast(Camera.main.transform.position, rayDirection, wallRunSnapDistance))
        {
            

            //Vector3 surfaceParallel = Vector3.ProjectOnPlane(rayDirection, filteredHit.normal);
            Vector3 surfaceParallel = Quaternion.Euler(0, -45, 0) * filteredHit.normal;
            Debug.DrawRay(Camera.main.transform.position, surfaceParallel * 20, Color.red, 1f);

            if (filteredHit.distance > wallRunSnapDistance)
            {
                jState = JumpState.InAir;
                p_rigidbody.useGravity = true;
            }

        }



        
    }

    private void processButtonInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }

        if (Input.GetButtonDown("Jump") && groundTest())
        {
            Rigid.AddForce(Vector3.up * JumpForce);
            jState = JumpState.InAir;
        }

        /*
        if (Input.GetButtonDown("Jump") && jState == JumpState.InAir && CanWallRun() && jState != JumpState.Wallrunning)
        {
            jState = JumpState.Wallrunning;
            //Debug.Log(p_rigidbody.velocity.to)
            p_rigidbody.velocity = new Vector3(p_rigidbody.velocity.x, 0, p_rigidbody.velocity.z);
            //p_rigidbody.useGravity = false;
        }
        */

    }

    private bool CanWallRun()
    {
        RaycastHit hitR;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.right, out hitR, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("RightSide WallRunHit");
            return true;
        }

        RaycastHit hitL;
        if (Physics.Raycast(Camera.main.transform.position, -Camera.main.transform.right, out hitL, wallRunSnapDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.right) * hitR.distance, Color.yellow);
            Debug.Log("LeftSide WallRunHit");
            return true;
        }

        return false;
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

    private void processFPMovementInput()
    {
        TestRotation();

        if (jState != JumpState.Wallrunning)
        {
            Vector3 VShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1))) * Input.GetAxis("Vertical") * MoveSpeed;
            Vector3 HShift = Vector3.Normalize(Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1))) * Input.GetAxis("Horizontal") * MoveSpeed;


            //Rigid.MovePosition(transform.position + VShift + HShift);
            //Rigid.AddForce(Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * MoveSpeed);
            //Rigid.velocity = new Vector3(Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).x, Rigid.velocity.y, Vector3.ClampMagnitude(Rigid.velocity, MaxMoveSpeed).z);
            Vector3 moveVec = (Vector3.ClampMagnitude((transform.position + VShift + HShift) - transform.position, 4f) * MoveSpeed);
            Debug.Log(moveVec.ToString());
            Rigid.velocity = new Vector3(moveVec.x, Rigid.velocity.y, moveVec.z);

        }

        //Invoke("groundTest", .3f);
        
        

    }

    private void process2DMovementInput()
    {
        if (jState != JumpState.Wallrunning)
        {
            float Shift = Input.GetAxis("Vertical") * MoveSpeed;
            

            
            Debug.Log(Shift.ToString());
            Rigid.velocity = new Vector3(Shift, Rigid.velocity.y, 0);

        }
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
                //Debug.Log("Did Hit");
                if(hit.distance < groundCheckDistance)
                {
                    jState = JumpState.Grounded;
                    return true;
                }
                else
                {
                    jState = JumpState.InAir;
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
        Camera.main.transform.eulerAngles = (Vector2)rotation;
    }
}
