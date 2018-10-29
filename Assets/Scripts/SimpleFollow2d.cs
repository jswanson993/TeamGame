using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow2d : MonoBehaviour {

    // Use this for initialization
    public Transform target;
    [SerializeField] float zDistance;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + 5, -zDistance);
        transform.position = newPos;
    }
}
