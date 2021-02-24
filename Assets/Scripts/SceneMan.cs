using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneMan : MonoBehaviour {
	public bool startAfterXSeconds;
	public float delay;
	public GameObject[] buttns;
	public Toggle tauntToggle;
	public Slider musicSlid,fxSlid;
	public GameObject selectLevel, optionsPanel, helpPanel, creditsPanel;
	public AudioClip click;
	AudioSource AS;
	// Use this for initialization
	void Start () {
		if (startAfterXSeconds) {
			StartCoroutine (startInXSeconds(delay));
		}
		AS = this.gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control != null && tauntToggle != null) {

			if (PlayerPrefs.HasKey ("taunt") != false) {
				switch(PlayerPrefs.GetInt("taunt")){
					case 0:
					tauntToggle.isOn = false;
					accountForLevels.control.taunt = false;
					break;
				case 1:
					tauntToggle.isOn = true;
					accountForLevels.control.taunt = true;
					break;
				}
			}


			if (PlayerPrefs.HasKey ("fxVol") != false) {
				fxSlid.value = PlayerPrefs.GetFloat ("fxVol");
				accountForLevels.control.fxVolume = fxSlid.value;
			}

			if (PlayerPrefs.HasKey ("musicVol") != false) {
				musicSlid.value = PlayerPrefs.GetFloat ("musicVol");
				accountForLevels.control.musicVolume = musicSlid.value;
			}

			AS.volume = accountForLevels.control.fxVolume;

		}

	}
	
	// Update is called once per frame
	public void ButtonClick (string levelTo) {
		if (!AS.isPlaying && click != null) {
			AS.PlayOneShot (click);
		}
		StartCoroutine (delayButton(levelTo));
	}

	public IEnumerator delayButton(string levelTo){
		if (click != null) {
			yield return new WaitForSeconds (click.length);
		}

		switch (levelTo) {
		case "quit":
			Application.Quit ();
			break;
		case "tutorial":
			accountForLevels.control.currentLevel = 0;
			SceneManager.LoadScene (levelTo);
			break;
		case "level1":
			accountForLevels.control.currentLevel = 1;
			SceneManager.LoadScene (levelTo);
			break;
		case "level2":
			accountForLevels.control.currentLevel = 2;
			SceneManager.LoadScene (levelTo);
			break;
		case "level3":
			accountForLevels.control.currentLevel = 3;
			SceneManager.LoadScene (levelTo);
			break;
		case "level4":
			accountForLevels.control.currentLevel = 4;
			SceneManager.LoadScene (levelTo);
			break;
		case "level5":
			accountForLevels.control.currentLevel = 5;
			SceneManager.LoadScene (levelTo);
			break;
		case "menu":
			SceneManager.LoadScene (levelTo);
			break;
		case "nextLevel":
			string levelToLoad = calculateLevel ();
			SceneManager.LoadScene (levelToLoad);
			break;
		case "levelRestart":
			string levelToRestart;
			if (accountForLevels.control != null) {
				levelToRestart = "level" + accountForLevels.control.currentLevel.ToString ();
			} else {
				levelToRestart = "level1";
			}

			SceneManager.LoadScene (levelToRestart);
			break;
		case "select":
			turnOffAllButtons ();
			selectLevel.SetActive (true);
			break;
		case "options":
			turnOffAllButtons ();
			optionsPanel.SetActive (true);
			break;
		case "help":
			turnOffAllButtons ();
			helpPanel.SetActive (true);
			break;
		case "credits":
			turnOffAllButtons ();
			creditsPanel.SetActive (true);
			break;
		case "goBackToMenu":
			turnOffAllButtons ();
			selectLevel.SetActive (false);
			creditsPanel.SetActive (false);
			helpPanel.SetActive (false);
			optionsPanel.SetActive (false);
			StartCoroutine(startInXSeconds(0));
			break;
		case "saveOptions":
			if (accountForLevels.control != null) {
				accountForLevels.control.fxVolume = fxSlid.value;
				accountForLevels.control.musicVolume = musicSlid.value;
				accountForLevels.control.taunt = tauntToggle.isOn;
				PlayerPrefs.SetFloat ("fxVol", fxSlid.value);
				PlayerPrefs.SetFloat ("musicVol", musicSlid.value);
				if (tauntToggle.isOn) {
					PlayerPrefs.SetInt ("taunt", 1);
				} else {
					PlayerPrefs.SetInt ("taunt", 0);
				}
				optionsPanel.SetActive (false);
				StartCoroutine (startInXSeconds (0));
			}
			break;
		default:
			SceneManager.LoadScene ("Menu");
			break;
		}
	}



	void turnOffAllButtons(){
		foreach (var item in buttns) {
			item.SetActive (false);
		}
	}


	IEnumerator startInXSeconds(float myDelay){
		if (myDelay != 0) {
			yield return new WaitForSeconds (myDelay);
		}

		foreach (var item in buttns) {
			item.SetActive (true);
		}
	}

	string calculateLevel(){
		if (accountForLevels.control.currentLevel < accountForLevels.control.maxLevels) {
			accountForLevels.control.currentLevel++;
			string b = "level" + accountForLevels.control.currentLevel.ToString ();
			return b;
		} else {
			return "endGame";
		}
	}
}


