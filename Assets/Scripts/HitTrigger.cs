using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitTrigger : MonoBehaviour {

    public UnityEvent triggerEffect;




    public void hitTrigger() {
        InvokeTrigger();
    }

    public void InvokeTrigger() {
        triggerEffect.Invoke();
    }
}
