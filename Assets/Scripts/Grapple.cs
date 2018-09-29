using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

    public static bool hasGrapple;
    public float grappleDistance = 100;
    public float grappleSpeed = 10;
    bool is3D;
    private bool fired = false;
    public Transform shotPoint;
    private Vector3 forwardPos;
    private bool collided;
    private Vector3 endpoint;
    private Vector3 currentPos;
    private bool leftSurface;
    private int count = 0;

    // Use this for initialization
    void Start () {
        //Change this after done implementing
        hasGrapple = true;

        is3D = GetComponent<Player_Controller>().is3D;
        shotPoint = GetComponent<Player_Controller>().shotPoint;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2") && hasGrapple) {
            if (!fired) {
                grapple();
                //Checks to see if the point the player is trying to grapple to is close enough.
                if (Math.Abs(endpoint.x - currentPos.x) <= grappleDistance && Math.Abs(endpoint.y - currentPos.y) <= grappleDistance && Math.Abs(endpoint.z - currentPos.z) <= grappleDistance) {
                    fired = true;
                    forwardPos = Camera.main.transform.forward;
                    GetComponent<Rigidbody>().useGravity = false;
                    
                }
            } else {
                fired = false;
            }

        }

        if (fired) {
            if (is3D) {  
                //Moves the player to the point of where they grappled
                this.transform.Translate(forwardPos * Time.deltaTime * grappleSpeed);
                count++;
                //Checks to see if you fired at the same surface you are on
                if (count == 3) {
                    if (!leftSurface) {
                        fired = false;
                    }
                    count = 0;
                }
                
            } else {
                //grapple2D();
            }
        } else {
            GetComponent<Rigidbody>().useGravity = true;
        }

    }

    /**
     * Sets the location of where to grapple to
     * */
    private void grapple() {  
        RaycastHit hit;
        currentPos = Camera.main.transform.position;
        if (Physics.Raycast(Camera.main.transform.position, transform.TransformDirection(Camera.main.transform.forward), out hit, Mathf.Infinity)) {
            endpoint = hit.point;
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward) * hit.distance, Color.yellow);
        } else {
            Debug.DrawRay(Camera.main.transform.position, transform.TransformDirection(Camera.main.transform.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            endpoint = transform.TransformDirection(Camera.main.transform.forward) * 1000;
        }
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        Invoke("RemoveTrail", .06f);

    }

    private void grapple2D() {

    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.collider.name);
        if (fired) {
            fired = false;
        }
        leftSurface = false;
    }
    private void OnCollisionExit(Collision collision) {
        leftSurface = true;
    }

}
