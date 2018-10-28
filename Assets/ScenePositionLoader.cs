using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePositionLoader : MonoBehaviour {

    // Use this for initialization
    GameObject[] PositionMarkers;
    public GameObject playerPrefab;
    public GameObject player2DPrefab;
    public GameObject LoadPos;
    void Start () {

        DontDestroyOnLoad(this.gameObject);
        PositionMarkers = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject item in PositionMarkers)
        {
            if (item.GetComponent<PositionMarker>().isStart)
            {
                Instantiate(playerPrefab, item.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = item.transform.rotation;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadRequestedScene(int SceneBuildIndex, string Heading, bool is3D)
    {
        SceneManager.LoadScene(SceneBuildIndex);
        
        if (is3D)
        {
            StartCoroutine(Example(SceneBuildIndex,Heading, is3D));
            //Instantiate(playerPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = LoadPos.transform.GetChild(0).rotation;
        }
        else
        {
            StartCoroutine(Example(SceneBuildIndex, Heading, is3D));
            //Debug.Log(LoadPos.transform.position);
            //Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = LoadPos.transform.GetChild(0).rotation;
        }
    }

    IEnumerator Example(int buildIndex, string Heading, bool is3D)
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
        LoadPos = GameObject.Find(Heading);
        Debug.Log(LoadPos.name);
        if (is3D)
        {
            //StartCoroutine(Example());
            GameObject G = Instantiate(playerPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity);
            G.GetComponent<Player_Controller>().is3D = true;
            G.GetComponent<Player_Controller>().refreshIs3D();
        }
        else
        {
            //StartCoroutine(Example());
            Debug.Log(LoadPos.transform.position);
            //Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity);
            Transform tempT = Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform;
            Camera.main.GetComponent<SimpleFollow2d>().target = tempT;
            tempT.GetComponent<Player_Controller>().is3D = false;
            tempT.GetComponent<Player_Controller>().refreshIs3D();
            tempT.transform.GetChild(2).rotation = LoadPos.transform.rotation;

        }
    }
}
