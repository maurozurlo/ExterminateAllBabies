using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {
	public bool startAfterXSeconds;
	public float delay;
	public GameObject ButtonPanel;
	public Toggle tauntToggle;
	public Slider musicSlid,fxSlid;
	public GameObject selectLevel, optionsPanel, helpPanel, creditsPanel;
	public GameObject subtitle;
	public AudioClip click;
	AudioSource AS;
	
	void Awake () {
		// Setup
		AS = gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control && tauntToggle) {
			// Taunts
			bool taunt = PlayerPrefs.HasKey("taunt") ? System.Convert.ToBoolean(PlayerPrefs.GetInt("taunt")) : true;
			tauntToggle.isOn = taunt;
			accountForLevels.control.taunt = taunt;
			// FX
			fxSlid.value = PlayerPrefs.HasKey("fxVol") ? PlayerPrefs.GetFloat ("fxVol") : 1;
			accountForLevels.control.fxVolume = fxSlid.value;
			AS.volume = accountForLevels.control.fxVolume;
			musicSlid.value = PlayerPrefs.HasKey("musicVol") ? PlayerPrefs.GetFloat ("musicVol") : 1;
			// Music
			accountForLevels.control.musicVolume = musicSlid.value;	
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
		//TODO AAAAAAAAAAAAAAAAAAAAAAAAAAA
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
			ActivateMenu();
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
			}
			break;
		default:
			SceneManager.LoadScene ("Menu");
			break;
		}
	}

	void turnOffAllButtons(){
		ButtonPanel.SetActive(false);
	}

	public void ActivateMenu(){
		ButtonPanel.SetActive(true);
		subtitle.SetActive(true);
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


