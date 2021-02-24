using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanUp : MonoBehaviour {

	public enum typeOfScreen{
		menu, win, lost
	}
	public typeOfScreen myType;
	bool restartwithEnter, activateRestart;

	void Awake () {
		Time.timeScale = 1.0f;

		if (accountForLevels.control != null) {
			accountForLevels.control.addNewUnlockedLevel ();

			switch (myType) {
			case typeOfScreen.win:
			case typeOfScreen.lost:
				activateRestart = true;
				break;
			}
		}
	}


	void Update(){
		if (activateRestart) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				if (!restartwithEnter) {
					if(myType == typeOfScreen.win){
						this.gameObject.GetComponent<SceneMan> ().ButtonClick ("nextLevel");
					}
					if(myType == typeOfScreen.lost){
						this.gameObject.GetComponent<SceneMan> ().ButtonClick ("levelRestart");
					}
					restartwithEnter = true;
				}
			}
		}
	}

}
