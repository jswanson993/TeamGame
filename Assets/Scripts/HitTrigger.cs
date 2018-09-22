using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitTrigger : MonoBehaviour {

    public UnityEvent triggerEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hitTrigger() {
        InvokeTrigger();
    }

    public void InvokeTrigger() {
        triggerEffect.Invoke();
    }
}
