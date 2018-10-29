using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHP = 100;
    private Text healthUI;
    public UnityEvent damageEvent;
    public UnityEvent deathEvent;

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
        if(currentHP <= 0) {
            currentHP = 0;
            healthUI.text = currentHP + "";
            playerDeath();
        }
    }

    public void playerDeath() {
        deathEvent.Invoke();

    }
}
