using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

    public static bool hasGrapple;
    public float grappleDistance = 100;
    public float grappleSpeed = 10;
    bool is3D;
    public bool fired = false;
    public Transform shotPoint;
    private Vector3 forwardPos;
    private bool collided;
    private Vector3 endpoint;
    private Vector3 currentPos;
    private bool leftSurface;
    private Player_Controller PlayerController;
    private Rigidbody playerRB;
    public GameObject TestPrefab;
    // Use this for initialization
    void Start () {
        //Change this after done implementing
        PlayerController = GetComponent<Player_Controller>();
        is3D = GetComponent<Player_Controller>().is3D;
        shotPoint = GetComponent<Player_Controller>().shotPoint;
        playerRB = GetComponent<Rigidbody>();
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
                if (is3D && Math.Abs(endpoint.x - currentPos.x) <= grappleDistance && Math.Abs(endpoint.y - currentPos.y) <= grappleDistance && Math.Abs(endpoint.z - currentPos.z) <= grappleDistance) {
                    fired = true;
                    if (is3D) {
                        forwardPos = Camera.main.transform.forward;
                    } else {
                        
                        forwardPos = shotPoint.forward;
                        
                    }
                    GetComponent<Rigidbody>().useGravity = false;
                    
                }
                else
                {
                    fired = true;
                    if (is3D)
                    {
                        forwardPos = Camera.main.transform.forward;
                    }
                    else
                    {

                        forwardPos = shotPoint.forward;

                    }
                    GetComponent<Rigidbody>().useGravity = false;
                }
            } else {
                fired = false;
                GetComponent<LineRenderer>().enabled = false;
            }

        }

        Debug.DrawRay(transform.position, forwardPos);

        if (fired) {
            if (is3D) {
                //Moves the player to the point of where they grappled
                //this.transform.Translate(forwardPos * Time.deltaTime * grappleSpeed);
                playerRB.velocity = forwardPos * grappleSpeed;
                StartCoroutine(checkSameSurface());
                playerRB.useGravity = false;
                //Checks to see if you fired at the same surface you are on
                
                
            } else {
                //this.transform.Translate(forwardPos * Time.deltaTime * grappleSpeed, Space.World);
                Debug.LogWarning("TryingShotForward");
                playerRB.velocity = forwardPos * grappleSpeed;
                Debug.DrawRay(transform.position, forwardPos * 4, Color.cyan, 1f);
            }
            GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        } else if(PlayerController.jState != Player_Controller.JumpState.Wallrunning) {
            GetComponent<Rigidbody>().useGravity = true;
        }

    }

    IEnumerator checkSameSurface() {;
        yield return new WaitForSeconds(.05f);
        if (!leftSurface) {
            Debug.Log("Checking");
            //fired = false;
        }

    }

    /**
     * Sets the location of where to grapple to
     * */
    private void grapple() {  
        RaycastHit hit;
        currentPos = Camera.main.transform.position;
        if (Physics.Raycast(Camera.main.transform.position + Camera.main.transform.forward, (Camera.main.transform.forward), out hit, Mathf.Infinity)) {
            endpoint = hit.point;
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward) * hit.distance, Color.yellow);
            playerRB.velocity = Vector3.zero;
        } else {
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward) * 10, Color.white);
            Debug.Log("Did not Hit");
            endpoint = Camera.main.transform.position + Camera.main.transform.forward * 1000;
            Invoke("RemoveTrail", .06f);
        }
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        //Invoke("RemoveTrail", .06f);

    }

    private void grapple2D() {
        int layerMask = 1 << 11;
        layerMask = ~layerMask;
        
        RaycastHit hit;
        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out hit, Mathf.Infinity, layerMask)) {
            Debug.DrawRay(shotPoint.position, shotPoint.forward * hit.distance, Color.red, 1f);
            endpoint = hit.point;
            //Instantiate(TestPrefab, hit.point, Quaternion.identity);

        } else {
            Debug.DrawRay(shotPoint.position, shotPoint.forward * 1000, Color.white);
            endpoint = shotPoint.position + (shotPoint.forward) * 1000;
            Debug.Log("RayMissed");
            Invoke("RemoveTrail", .06f);
        }

        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { shotPoint.position, endpoint });
        
    }

    private void OnCollisionEnter(Collision collision) {
        //Debug.Log(collision.collider.name);
        if (fired) {
            fired = false;
        }
        leftSurface = false;
    }
    private void OnCollisionExit(Collision collision) {
        leftSurface = true;
    }

    void RemoveTrail()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    public void enableGrapple() {
        Debug.Log("is Enabled");
        hasGrapple = true;
        //PlayerPrefs.Save()
    }

}
