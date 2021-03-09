using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBabies : MonoBehaviour {
	public GameObject[] baby;
	public float minTime,maxTime;
	public int amountOfBabies;
	public bool checkForEndOfGame;
	public int probOfbaby1,probOfbaby2,probOfbaby3;	

	void Start () {
		StartCoroutine ("SpawnBaby");
	}

	IEnumerator SpawnBaby () {
		int pickBaby = pickABaby ();

		if (amountOfBabies < gameController.control.totalBabies) {
			GameObject Baby = Instantiate (baby[pickBaby], transform.position, Quaternion.identity);
			Baby.tag = "BabyEnemy";
			amountOfBabies++;
			gameController.control.babyInt = gameController.control.totalBabies - amountOfBabies;
			gameController.control.UpdateUI ();
			yield return new WaitForSeconds (Random.Range (minTime, maxTime));
			StartCoroutine ("SpawnBaby");
		} else {
			checkForEndOfGame = true;
		}


		if (amountOfBabies == gameController.control.totalBabies--) {
			gameController.control.StopPowerUps ();
		}
	}

	int pickABaby(){
		int ourRandomNumber = Random.Range (0, 100);
		int ourNumber = 0;
		if (ourRandomNumber < probOfbaby1) {
			ourNumber = 0;
		}
		if (ourRandomNumber > probOfbaby1 && ourRandomNumber < probOfbaby3) {
			ourNumber = 1;
		}
		if (ourRandomNumber > probOfbaby3) {
			ourNumber = 2;
		}
		return ourNumber;
	}


	//TODO AAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHAHHAHAHAHAHHAAJHADJHASDJASHDFJASHDASJFAJF NO.
	void Update(){
		if (checkForEndOfGame) {
			if (GameObject.FindGameObjectsWithTag ("BabyEnemy").Length == 0) {
				gameController.control.WeWon ();
				checkForEndOfGame = false;
			}
		}
	}

	public void IncreaseSpeed(float speedMul){
		if (maxTime > minTime) {
			maxTime *= speedMul;
		}
	}

	public void halt(){
		CancelInvoke ();
	}

	public void resume(){
		InvokeRepeating("SpawnBaby",0,Random.Range(minTime,maxTime));
	}
}
