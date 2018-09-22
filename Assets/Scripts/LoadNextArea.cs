using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadNextArea : MonoBehaviour {

    public string scene;
<<<<<<< HEAD



    void OnTriggerEnter (Collider collision) {
        //if (collision.collider.gameObject.tag == "Player") {
            SceneManager.LoadScene(scene);
           
        //}
=======



    void OnTriggerEnter (Collider col) {
        if (col.gameObject.tag == "Player") {
            SceneManager.LoadScene(scene);
           
        }
>>>>>>> 8fe63884966738f0152d6e9d57a490298e045022
    }


}
