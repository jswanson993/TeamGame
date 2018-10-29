using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    private string lastPosition;
    private int currentHealth;
    public bool is3d;
    public GameObject SceneLoader;
    public GameObject canvas;
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
        Text tutorial = canvas.AddComponent<Text>();
        tutorial.text = "Use WASD to move, Space to jump, Left Click to shoot\n You can Press Space against a wall to wall run";
        Vector3 textTrans =  tutorial.rectTransform.position;
        textTrans.x = 0;
        textTrans.y = 100;
        textTrans.z = 0;
        
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
