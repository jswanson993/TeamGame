using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour {

    public int startingHP = 100;

    public UnityEvent damageEvent;
    public UnityEvent deathEvent;

    private int currentHP;

    private void Start() {
        currentHP = startingHP;
    }

    public void takeDamage(int damage) {
        currentHP -= damage;

        if(currentHP <= 0) {
            currentHP = 0;
            playerDeath();
        }
    }

    public void playerDeath() {
        deathEvent.Invoke();

    }
}
