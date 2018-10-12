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

    // Use this for initialization
    void Start () {
        //Change this after done implementing
        hasGrapple = true;

        is3D = GetComponent<Player_Controller>().is3D;
        shotPoint = GetComponent<Player_Controller>().shotPoint;
    }
	
	// Update is called once per frame
	void Update () {
        //forwardPos = shotPoint.forward;
        //Debug.Log("shot point transform updated");
        if (Input.GetButtonDown("Fire2") && hasGrapple) {
            if (!fired) {
                if (is3D) {
                    grapple();
                } else {
                    grapple2D();
                }
                //Checks to see if the point the player is trying to grapple to is close enough.
                if (Math.Abs(endpoint.x - currentPos.x) <= grappleDistance && Math.Abs(endpoint.y - currentPos.y) <= grappleDistance && Math.Abs(endpoint.z - currentPos.z) <= grappleDistance) {
                    fired = true;
                    if (is3D) {
                        forwardPos = Camera.main.transform.forward;
                    } else {
                        
                        forwardPos = shotPoint.forward;
                        
                    }
                    GetComponent<Rigidbody>().useGravity = false;
                    
                }
            } else {
                fired = false;
            }

        }

        Debug.DrawRay(transform.position, forwardPos);

        if (fired) {
            if (is3D) {  
                //Moves the player to the point of where they grappled
                this.transform.Translate(forwardPos * Time.deltaTime * grappleSpeed);
                StartCoroutine(checkSameSurface());
                //Checks to see if you fired at the same surface you are on
                
                
            } else {
                this.transform.Translate(forwardPos * Time.deltaTime * grappleSpeed, Space.World);
                Debug.DrawRay(transform.position, forwardPos * 4, Color.cyan, 1f);
            }
        } else {
            GetComponent<Rigidbody>().useGravity = true;
        }

    }

    IEnumerator checkSameSurface() {;
        yield return new WaitForSeconds(.05f);
        if (!leftSurface) {
            Debug.Log("Checking");
            fired = false;
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
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        Vector3 endpoint;
        RaycastHit hit;
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out hit, Mathf.Infinity, layerMask)) {
            Debug.DrawRay(shotPoint.position, shotPoint.forward * hit.distance, Color.red, 1f);
            endpoint = hit.point;

        } else {
            Debug.DrawRay(shotPoint.position, transform.TransformDirection(shotPoint.forward) * 1000, Color.white);
            endpoint = transform.TransformDirection(shotPoint.forward) * 1000;
            Debug.Log("RayMissed");
        }

        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        Invoke("RemoveTrail", .06f);
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
