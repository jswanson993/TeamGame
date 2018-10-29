using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRBInChildren : MonoBehaviour {

    // Use this for initialization
    public Component[] RBs;
    public Transform parent;
    private bool triggered;
	void Start () {
        RBs = parent.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody RB in RBs)
        {
            RB.isKinematic = true;
            //RB.gameObject.AddComponent<MeshCollider>();
            //RB.gameObject.GetComponent<MeshCollider>().convex = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Trigger");
        if(col.collider.tag == "Player" && !triggered)
        {
            triggered = true;
            foreach (Rigidbody RB in RBs)
            {
                RB.isKinematic = false;
                RB.AddForce(Random.rotation.ToEulerAngles() * 100);
                
            }
            GetComponent<Collider>().enabled = false;
        }
    }
}
