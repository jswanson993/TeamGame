using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadNextArea : MonoBehaviour {

    public string scene;



    void OnTriggerEnter (Collision collision) {
        //if (collision.collider.gameObject.tag == "Player") {
            SceneManager.LoadScene(scene);
           
        //}
    }


}
