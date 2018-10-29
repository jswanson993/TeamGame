using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    private string lastPosition;
    public bool is3d;

	// Use this for initialization
	void Start () {
        lastPosition = GetComponent<ScenePositionLoader>().getLastHeading();
        Debug.Log("Last Position:" + lastPosition);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void reset() {
        GetComponent<ScenePositionLoader>().LoadRequestedScene(SceneManager.GetActiveScene().buildIndex, lastPosition, is3d);
    }
}
