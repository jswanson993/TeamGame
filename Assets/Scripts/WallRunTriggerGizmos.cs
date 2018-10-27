using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallRunTriggerGizmos : MonoBehaviour {

    void Awake()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward * 5, Color.red);
        Debug.DrawRay(this.transform.position, this.transform.right * 5, Color.yellow);
        Debug.DrawRay(this.transform.position, this.transform.up * 5, Color.cyan);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(this.transform.position, this.transform.forward * 5, Color.red);
        Debug.DrawRay(this.transform.position, this.transform.right * 5, Color.yellow);
        Debug.DrawRay(this.transform.position, this.transform.up * 5, Color.cyan);
    }
}
