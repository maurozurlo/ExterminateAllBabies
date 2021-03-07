using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accountForLevels : MonoBehaviour {


	public static accountForLevels control;
	public int currentLevel;
	public int maxLevels = 5;
	public int UnlockedLevel;
	public bool taunt = true;
	public float fxVolume;
	public float musicVolume;

	// Use this for initialization
	void Awake () {
		if (control == null) {
			control = this;
			DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (this.gameObject);
		}

		if (PlayerPrefs.HasKey ("unlockedLevels")) {
			UnlockedLevel = PlayerPrefs.GetInt ("unlockedLevels");
		}
	}

	public void addNewUnlockedLevel(){
		int tryTo = currentLevel + 2;
		if (tryTo > UnlockedLevel) {
			PlayerPrefs.SetInt ("unlockedLevels", tryTo);
			UnlockedLevel = PlayerPrefs.GetInt ("unlockedLevels");
		}
	}
}
