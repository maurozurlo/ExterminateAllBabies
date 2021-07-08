using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swatter : MonoBehaviour {
	PlayerMovement playerMovement;

    private void Start()
    {
		playerMovement = gameObject.transform.parent.GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider col){
		if (playerMovement.hitting) {
			if (col.name == "babyPlane") {
				col.gameObject.GetComponent<BabyAnimation> ().Hit ();
				col.gameObject.GetComponentInParent<Baby> ().kill ();
			}
			if (col.CompareTag("PowerUp")) {
				col.gameObject.GetComponent<PowerUp>().KillWithSwatter();
			}
		}
	}
}
