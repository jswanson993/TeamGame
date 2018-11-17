using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour {
    public GameObject Gun;
    private float recoil = 0.0f;

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
		if(recoil != 0){
           //Gun.transform.Translate()
        } else {
            recoil = 0;
        }
	}

    void setRecoil(float recoilAmount) {
        recoil = recoilAmount;
    }
}
