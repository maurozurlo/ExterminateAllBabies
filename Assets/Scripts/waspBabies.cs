using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waspBabies : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if (gameController.control.currentState == gameController.states.hitting) {
			if (col.name == "babyPlane") {
				col.gameObject.GetComponent<BabyAnimation> ().Hit ();
				col.gameObject.GetComponentInParent<Baby> ().kill ();
			}


			if (col.name == "powerUp") {
				col.gameObject.GetComponent<genericPowerUp> ().killWithWasp ();
			}
		}
	}
}
