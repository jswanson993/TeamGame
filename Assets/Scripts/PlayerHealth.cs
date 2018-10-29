using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHP = 100;
    private Text healthUI;
    public UnityEvent damageEvent;
    //public UnityEvent deathEvent;
    public GameObject canvas;

    private int currentHP;

    private void Start() {
        currentHP = startingHP;
        GameObject healthGO = GameObject.Find("HealthTracker");
        healthUI = healthGO.GetComponent<Text>();
        healthUI.text = "100";
    } 


    public void takeDamage(int damage) {
        currentHP -= damage;
        healthUI.text = currentHP + "";
        //GM.GetComponent<Manager>().setCurrentHealth();
        if (currentHP <= 0) {
            currentHP = 0;
            healthUI.text = currentHP + "";
            //GetComponent<Manager>().setCurrentHealth();
            playerDeath();
        }
    }

    public void playerDeath() {
        //deathEvent.Invoke();
        //GM.GetComponent<Manager>().reset();
        GameObject.Find("ScenePositionLoader").GetComponent<ScenePositionLoader>().LoadRequestedScene(SceneManager.GetActiveScene().buildIndex, GameObject.Find("ScenePositionLoader").GetComponent<ScenePositionLoader>().getLastHeading(), GameObject.Find("ScenePositionLoader").GetComponent<ScenePositionLoader>().wasLastHeading3D);

    }

    private void setPlayerHealth() {
        //GM.GetComponent<Manager>().getCurrentHealth();
        healthUI.text = currentHP + "";
    }

    public int getCurrentHealth() {
        return currentHP;
    }

    public void gainHealth(int health) {
        currentHP += health;
        //GM.GetComponent<Manager>().setCurrentHealth();
        healthUI.text = currentHP + "";
    }
}
