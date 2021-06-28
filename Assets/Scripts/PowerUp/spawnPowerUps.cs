using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPowerUps : MonoBehaviour {
	public GameObject[] powerUpGo;
	public float minTime,maxTime;
	public int amountOfPowerUps;
	public int actualAmountOfPowerUps;
	// Use this for initialization

	void Start () {
		StartCoroutine ("firstSpawn");
	}


	IEnumerator firstSpawn(){
		yield return new WaitForSeconds (minTime);
		StartCoroutine ("spawnPowerUp");
	}


	// Update is called once per frame
	IEnumerator spawnPowerUp () {
		int powerUpInt = pickAPowerUp ();

		if (amountOfPowerUps < actualAmountOfPowerUps) {
			GameObject powerUp = Instantiate (powerUpGo [powerUpInt], transform.position, Quaternion.identity) as GameObject;
			powerUp.name = "powerUp";
			powerUp.tag = "PowerUp";
			amountOfPowerUps++;
			yield return new WaitForSeconds (Random.Range (minTime, maxTime));
			StartCoroutine ("spawnPowerUp");
		} else {
			halt ();
		}
	}

	int pickAPowerUp(){
		int ourNumber = Random.Range(0,powerUpGo.Length);

		return ourNumber;
	}

	public void halt(){
		CancelInvoke ();
	}

	public void resume(){
		InvokeRepeating("spawnPowerUp",0,Random.Range(minTime,maxTime));
	}
}
