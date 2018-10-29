using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    private string lastPosition;
    private int currentHealth;
    public bool is3d;
    public GameObject SceneLoader;

	// Use this for initialization
	void Start () {
        
        Debug.Log("Last Position:" + lastPosition);
        SceneLoader = GameObject.Find("ScenePositionLoader");
        if (SceneLoader != null)
        {
            lastPosition = SceneLoader.GetComponent<ScenePositionLoader>().getLastHeading();
            Debug.LogError("Heading LP= " + lastPosition);
        }
        //currentHealth = 100;
        //setCurrentHealth();

        
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void reset() {
        if (SceneLoader != null)
        {
            SceneLoader.GetComponent<ScenePositionLoader>().LoadRequestedScene(SceneManager.GetActiveScene().buildIndex, lastPosition, is3d);
            //SPLoader.GetComponent<ScenePositionLoader>().LoadRequestedScene(sceneIndexToLoad, gameObject.name, isNextScene3D);
            //GetComponent<ScenePositionLoader>().LoadRequestedScene(SceneManager.GetActiveScene().buildIndex, lastPosition, is3d);
        }
        else
        {
            //SceneLoader.GetComponent<ScenePositionLoader>().LoadRequestedScene(SceneManager.GetActiveScene().buildIndex, lastPosition, is3d);
            throw new Exception("no scene loader");
        }
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    public void setCurrentHealth() {
        //currentHealth = GetComponent<PlayerHealth>().getCurrentHealth();
    }
}
