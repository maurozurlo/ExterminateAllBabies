using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateLevels : MonoBehaviour {
	public GameObject[] levels;
	// Use this for initialization
	void Start () {
		if (accountForLevels.control != null) {
			if (accountForLevels.control.UnlockedLevel <= levels.Length) {
				for (int i = 0; i < accountForLevels.control.UnlockedLevel; i++) {
					levels [i].GetComponent<UnityEngine.UI.Button> ().interactable = true;
				}
			}
		}
	}
}
