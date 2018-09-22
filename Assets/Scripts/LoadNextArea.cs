using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;


public class LoadNextArea : MonoBehaviour {

    public string scene;
    public bool isActive = false;
    

    private void OnCollisionEnter(Collision collision) {
        if (isActive) {
            EditorSceneManager.LoadScene(scene);
        }
    }
}
