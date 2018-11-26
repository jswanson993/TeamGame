using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour {
    bool isPaused = false;
    public Canvas pauseCanvas;

    private void Start(){
        Time.timeScale = 1f;
        pauseCanvas.enabled = false;
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
           isPaused = pauseGame();
        }
	}
    
    

    public void Resume(){
        isPaused = pauseGame();
    }

    public void Quit(){
        SceneManager.LoadScene(0);
    }



    private bool pauseGame() {
        Debug.Log("Paused");
        if(isPaused) {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseCanvas.enabled = false;
            return false;
        } else {
            pauseCanvas.enabled = true;
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return true;
        }
    }
}
