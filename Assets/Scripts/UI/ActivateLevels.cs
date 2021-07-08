using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLevels : MonoBehaviour {
	public GameObject[] levels;
	LevelController levelController;
	// Use this for initialization
	void Start () {
		levelController = LevelController.control;

			if (levelController.GetUnlockedLevel() <= levels.Length) {
			int lenght = levelController.GetUnlockedLevel();
				for (int i = 0; i < lenght; i++) {
					levels [i].GetComponent<UnityEngine.UI.Button> ().interactable = true;
				}
			}
	}
}
