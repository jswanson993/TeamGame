using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarker : MonoBehaviour {

	// Use this for initialization
    public bool isStart;//should be only one per scene
    public int sceneIndexToLoad;
    public bool isNextScene3D;
    private GameObject SPLoader;
	void Start () {
        SPLoader = GameObject.Find("ScenePositionLoader");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            SPLoader.GetComponent<ScenePositionLoader>().LoadRequestedScene(sceneIndexToLoad, gameObject.name, isNextScene3D);
        }
    }
}
