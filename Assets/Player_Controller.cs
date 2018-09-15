using System;
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Camera.main.transform.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(shotPoint.position, transform.TransformDirection(Camera.main.transform.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            endpoint = hit.point;
           
        }
        else
        {
            Debug.DrawRay(shotPoint.position, transform.TransformDirection(Camera.main.transform.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            endpoint = Camera.main.transform.forward * 100;
        }
        
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] {shotPoint.position, endpoint});
        Invoke("RemoveTrail", .06f);
        
    }

    private void RemoveTrail()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    private void processMovementInput()
    {
        TestRotation();
        Rigid.MovePosition(transform.position + (Camera.main.transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (Camera.main.transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
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
