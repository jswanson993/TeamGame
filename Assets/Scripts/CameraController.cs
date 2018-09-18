using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Perspectives")]
    public bool is2D = false;
    public Camera camera2D;
    public Camera camera3D;

	// Use this for initialization
	void Start () {
        camera2D.enabled = false;
        camera3D.enabled = true;
	}
	
	// Update is called once per frame
//	void Update () {

//	}

    void switchPerspective() {
        if (is2D) {
            camera3D.enabled = true;
            camera2D.enabled = false;
            is2D = false;
            
        } else {
            camera2D.enabled = true;
            camera3D.enabled = false;
            is2D = true;
        }
    }
}
