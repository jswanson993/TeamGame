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
    private static bool loadedFirst;
    private string lastHeading;
    public bool wasLastHeading3D;
    void Awake () {
        if (!loadedFirst)
        {
            Debug.Log("Loaded first clone");
            DontDestroyOnLoad(this.gameObject);
            PositionMarkers = GameObject.FindGameObjectsWithTag("Respawn");
            foreach (GameObject item in PositionMarkers)
            {
                if (item.GetComponent<PositionMarker>().isStart)
                {
                    Instantiate(playerPrefab, item.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = item.transform.rotation;
                    loadedFirst = true;
                    lastHeading = item.name;
                    Debug.LogError("Heading = " + item.name);
                    wasLastHeading3D = true; //BAD
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadRequestedScene(int SceneBuildIndex, string Heading, bool is3D)
    {
        lastHeading = Heading;
        wasLastHeading3D = is3D;
        SceneManager.LoadScene(SceneBuildIndex);
        
        if (is3D)
        {
            StartCoroutine(LoadNext(SceneBuildIndex,Heading, is3D));
            //Instantiate(playerPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = LoadPos.transform.GetChild(0).rotation;
        }
        else
        {
            StartCoroutine(LoadNext(SceneBuildIndex, Heading, is3D));
            //Debug.Log(LoadPos.transform.position);
            //Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform.GetChild(2).rotation = LoadPos.transform.GetChild(0).rotation;
        }
    }

    IEnumerator LoadNext(int buildIndex, string Heading, bool is3D)
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
        Debug.LogError("Heading = " + Heading);
        LoadPos = GameObject.Find(Heading);
        Debug.Log(LoadPos.name);
        if (is3D)
        {
            //StartCoroutine(Example());
            GameObject G = Instantiate(playerPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity);
            G.GetComponent<Player_Controller>().is3D = true;
            G.GetComponent<Player_Controller>().refreshIs3D();
            G.transform.GetChild(2).rotation = LoadPos.transform.rotation;
        }
        else
        {
            //StartCoroutine(Example());
            Debug.Log(LoadPos.transform.position);
            //Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity);
            Transform tempT = Instantiate(player2DPrefab, LoadPos.transform.GetChild(0).position, Quaternion.identity).transform;
            Camera.main.GetComponent<SimpleFollow2d>().target = tempT;
            Camera.main.GetComponent<Camera>().cullingMask = -1;
            tempT.GetComponent<Player_Controller>().is3D = false;
            tempT.GetComponent<Player_Controller>().refreshIs3D();
            

        }
    }

    public string getLastHeading() {
        return lastHeading;
    }
}
