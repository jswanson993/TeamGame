using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunTrigger : MonoBehaviour {

    [SerializeField] float WallRunBoost;
    public float MaxWallRunSpeed;
    Vector3 VelocitySnapshot;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger Entered");
        Debug.Log(col.name);
        if (col.transform.parent.GetComponent<Player_Controller>() || col.transform.tag == "Player")
        {
            col.transform.parent.GetComponent<Player_Controller>().jState = Player_Controller.JumpState.Wallrunning;
            Rigidbody r = col.transform.parent.GetComponent<Player_Controller>().Rigid;
            VelocitySnapshot = new Vector3(r.velocity.x, 0, r.velocity.z);
            r.velocity = new Vector3(r.velocity.x, 0, r.velocity.z);
            r.AddForce(Vector3.Scale(transform.forward, new Vector3(r.velocity.x, 0, r.velocity.z)));
            r.useGravity = false;
            
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.transform.parent.GetComponent<Player_Controller>() || col.transform.tag == "Player")
        {
            col.transform.parent.GetComponent<Player_Controller>().jState = Player_Controller.JumpState.Wallrunning;
            Rigidbody r = col.transform.parent.GetComponent<Player_Controller>().Rigid;
            r.velocity = new Vector3(r.velocity.x, 0, r.velocity.z);
            r.AddForce( VelocitySnapshot * WallRunBoost);
            //Vector3.Scale(transform.forward * .2f, new Vector3(r.velocity.x, 0, r.velocity.z)) +
            r.velocity = Vector3.ClampMagnitude(r.velocity, MaxWallRunSpeed);

        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Trigger Left");
        Debug.Log(col.name);
        if (col.transform.parent.GetComponent<Player_Controller>() || col.transform.tag == "Player")
        {
            col.transform.parent.GetComponent<Player_Controller>().isWallLeaping = true;
            col.transform.parent.GetComponent<Player_Controller>().jState = Player_Controller.JumpState.InAir;
            Rigidbody r = col.transform.parent.GetComponent<Player_Controller>().Rigid;
            r.useGravity = true;
        }
    }
}
