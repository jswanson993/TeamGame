using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePositionLoader : MonoBehaviour {

    // Use this for initialization
    GameObject[] PositionMarkers;
    public GameObject playerPrefab;
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        PositionMarkers = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject item in PositionMarkers)
        {
            if (item.GetComponent<PositionMarker>().isStart)
            {
                Instantiate(playerPrefab, item.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = item.transform.GetChild(0).rotation;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
