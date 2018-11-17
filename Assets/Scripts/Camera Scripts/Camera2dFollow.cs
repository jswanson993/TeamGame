using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2dFollow : MonoBehaviour {
   
    //public float followSpeed = .5f;
    //public float delay = .5f;
    public int camEdgeDistance = 1;

    
    //private GameObject player = GameObject.Find("PlayerFP");
    public Transform target;
    public float followSpeed = 8f;
    
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate() {
        moveCamera();
    }
    /**
     * Moves the to the side if the character is far enough in the screen 
     **/
    void moveCamera() {
        Vector3 screenPos = Camera.current.WorldToScreenPoint(target.position);
        //if (screenPos.x > camEdgeDistance || screenPos.x < -camEdgeDistance) {
        //Vector3 currentPos = this.transform.position;
        //currentPos.x = target.position.x;
        //this.transform.position = currentPos;
        //}
        
        //this.transform.position = Vector3.Lerp(this.transform.position, target, Time.fixedDeltaTime * followSpeed);
    }
}
