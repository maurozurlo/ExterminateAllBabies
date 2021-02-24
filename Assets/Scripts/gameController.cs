using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {
	public enum states
	{
		juststarted, idle, hitting, dead, paused
	};
	public states currentState;
	public Text scoreText;
	public Text babyText;
	public Text pauseText, cuentaRegresiva;
	public Image damageScreen;
	public Button menuBut;

	public bool paused;
	public int babySpeedForThisLevel = 3;
	public int scoreInt;
	public int babyInt;
	public int totalBabies;
	public float speedMultiplier;
	public static gameController control;
	public GameObject babyControl;
	public float delay;
	public int howManyInARow;
	AudioSource AS;
	public AudioClip[] hurtSound;
	public AudioClip youWon, bombSound, snifSound;
	public bool upsideDownWorld;



	[Header("Power Up Stuff")]

	public float powerUpTime = 2;
	public enum powerUpActivated
	{
		none, flash, bomb, lsd, ice
	}
	public powerUpActivated powerUp;
	public GameObject lsdCamera;
	GameObject player, music;
	float orgSpeedForMouse;
	public float frozenMouseSpeed, flashMouseSpeed;

	// Use this for initialization
	void Awake () {
		if (control == null) {
			control = this;
		} else {
			Destroy (this.gameObject);
		}
		this.currentState = states.juststarted;
		UpdateUI ();
		StartCoroutine ("SetUpStuff");
		AS = this.gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control != null) {
			AS.volume = accountForLevels.control.fxVolume;
		}

		player = GameObject.Find ("Player");
		music = GameObject.Find ("Music");
		orgSpeedForMouse = player.GetComponent<MouseMove> ().speed;
		Time.timeScale = 1.0f;
	}

	void Update(){
		if (currentState != states.juststarted) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (!paused) {
					Pause ();
					paused = true;
					//cambiar UI
					pauseText.gameObject.SetActive (true);
					menuBut.gameObject.SetActive (true);
				} else {
					Resume ();
					paused = false;
					pauseText.gameObject.SetActive (false);
					menuBut.gameObject.SetActive (false);
					//cambiar UI
				}
			}
		}
	}

	// Update is called once per frame
	public void UpdateUI () {
		babyText.text = "/ " + babyInt.ToString ();
		scoreText.text = " = " + scoreInt.ToString ();
	}


	IEnumerator SetUpStuff(){
		for (int i = 0; i < delay; i++) {
			cuentaRegresiva.text = (delay - i).ToString ();
			yield return new WaitForSeconds (1);
		}
		cuentaRegresiva.text = 0.ToString();
		yield return new WaitForSeconds (1);
		cuentaRegresiva.text = "";
		babyControl.GetComponent<spawnBabies> ().enabled = true;
		babyControl.GetComponent<spawnPowerUps> ().enabled = true;
		currentState = states.idle;
		InvokeRepeating ("IncreaseSpeed", 0, 5);
	}

	public void Pause(){
		currentState = states.paused;
		babyControl.GetComponent<spawnBabies> ().halt ();
		babyControl.GetComponent<spawnPowerUps> ().halt ();

		GameObject[] babiesNow = GameObject.FindGameObjectsWithTag ("BabyEnemy");
		foreach (var item in babiesNow) {
			item.GetComponent<Baby> ().kill ();
			item.GetComponentInChildren<BabyAnimation>().alive = false;
		}

		GameObject[] pUps = GameObject.FindGameObjectsWithTag ("PowerUp");
		foreach (var item in pUps) {
			item.GetComponent<genericPowerUp> ().PausePU ();
		}

	}

	public void Resume(){
		currentState = states.idle;
		babyControl.GetComponent<spawnBabies> ().resume ();
		GameObject[] babiesNow = GameObject.FindGameObjectsWithTag ("BabyEnemy");
		foreach (var item in babiesNow) {
			item.GetComponent<Baby> ().revive ();
			item.GetComponentInChildren<BabyAnimation>().alive = true;
		}

		babyControl.GetComponent<spawnPowerUps> ().resume ();
		GameObject[] pUps = GameObject.FindGameObjectsWithTag ("PowerUp");
		foreach (var item in pUps) {
			item.GetComponent<genericPowerUp> ().revive ();
		}

	}

	public IEnumerator takeDamage(int dmgToTake, Color col){
		scoreInt = scoreInt - dmgToTake;
		damageScreen.color = col;

		if (scoreInt > -1) {
			UpdateUI();
			howManyInARow = 0;
			if (!AS.isPlaying) {
				AS.PlayOneShot (hurtSound [Random.Range(0, hurtSound.Length)]);
			}
			damageScreen.gameObject.SetActive (true);
			damageScreen.GetComponent<ImgHandlers> ().replayAnim ();
			yield return new WaitForSeconds (1);
			damageScreen.gameObject.SetActive (false);
		} else {
			//game over
			currentState = states.dead;
			SceneManager.LoadScene("GameOver");
		}
	}


	public void WeWon(){
		StartCoroutine("WinRoutine");
	}

	IEnumerator WinRoutine(){
		cuentaRegresiva.text = "YOU WIN";
		AS.PlayOneShot (youWon);
		yield return new WaitForSeconds (youWon.length + 1);
		SceneManager.LoadScene ("YouWon");
	}

	public void StopPowerUps(){
		babyControl.GetComponent<spawnPowerUps> ().halt ();
	}

	public void IncreaseSpeed(){
		babyControl.GetComponent<spawnBabies> ().IncreaseSpeed (speedMultiplier);
	}


	public void activatePowerUp(powerUpActivated pw, int myDamage){
		int damageForThisLevel;


		if (accountForLevels.control != null) {
			damageForThisLevel = myDamage * accountForLevels.control.currentLevel;
		} else {
			damageForThisLevel = myDamage;
		}

		if (pw != powerUpActivated.flash) {
			

		}

		switch (pw) {
		case powerUpActivated.bomb:
			Debug.Log ("got hit by bomb");
			StartCoroutine (gameController.control.takeDamage (damageForThisLevel,Color.red));
			AS.PlayOneShot (bombSound);
			break;
		case powerUpActivated.flash:
			Debug.Log ("got hit by flash");
			StartCoroutine (gameController.control.takeDamage (damageForThisLevel,Color.yellow));
			player.GetComponent<MouseMove> ().speed = flashMouseSpeed;
			player.GetComponent<MouseMove> ().changeVisuals("flash");
			music.GetComponent<AudioSource> ().pitch = 2;
			StartCoroutine ("returnPlayerToNormal");
			break;
		case powerUpActivated.ice:
			StartCoroutine (gameController.control.takeDamage (damageForThisLevel,Color.cyan));
			player.GetComponent<MouseMove> ().speed = frozenMouseSpeed;
			player.GetComponent<MouseMove> ().changeVisuals("ice");
			music.GetComponent<AudioSource> ().pitch = .5f;
			Debug.Log ("got hit by ice");
			StartCoroutine ("returnPlayerToNormal");
			break;
		case powerUpActivated.lsd:
			Debug.Log ("got hit by lsd");
			StartCoroutine (gameController.control.takeDamage (damageForThisLevel, Color.magenta));
			Instantiate (lsdCamera, new Vector3 (0, 4.2f, -0.24f), Quaternion.identity);
			music.GetComponent<AudioSource> ().pitch = 5;
			AS.PlayOneShot (snifSound);
			break;
		}
	}


	IEnumerator returnPlayerToNormal()
	{
		yield return new WaitForSeconds (powerUpTime);
		music.GetComponent<AudioSource> ().pitch = 1;
		player.GetComponent<MouseMove>().speed = orgSpeedForMouse;
		player.GetComponent<MouseMove> ().returnVisualsToNormal ();
	}

	public void  returnMusicToNormal(){
		music.GetComponent<AudioSource> ().pitch = 1;
	}


}
