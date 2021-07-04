using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waspBabies : MonoBehaviour {
	PlayerMovement playerMovement;
	public GameObject player;

    private void Start()
    {
		playerMovement = player.GetComponent<PlayerMovement>();
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
