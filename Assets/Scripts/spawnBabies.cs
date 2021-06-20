using System.Collections;
using UnityEngine;

public class spawnBabies : MonoBehaviour {
	public GameObject[] baby;
	public float minTime,maxTime; // Should be in gameController?

	public int spawnedBabies;
	public int probOfbaby1,probOfbaby2,probOfbaby3; // TODO: Use a weighted table instead...

	gameController GameController;
	
	void Start () {
		StartCoroutine ("SpawnBaby");
		GameController = gameController.control;
	}

	public void IncreaseSpeed(float speedMul)
	{
		if (maxTime > minTime) maxTime *= speedMul;
	}

	IEnumerator SpawnBaby () {
		// Check if we can spawn babies
		if (GameController.gameState != gameController.GameState.play) yield return null;
		// Check if all babies have been spawned already
		if (spawnedBabies == GameController.totalBabies) yield return null;

		// Pick a random baby
		GameObject Baby = Instantiate (baby[PickABaby()], transform.position, Quaternion.identity);
		Baby.tag = "BabyEnemy";
		spawnedBabies++;
		GameController.BabyWasSpawned(spawnedBabies);

		yield return new WaitForSeconds (Random.Range (minTime, maxTime));
		StartCoroutine ("SpawnBaby");
	}

	int PickABaby(){
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
		//	if (checkForEndOfGame) {
		//	if (GameObject.FindGameObjectsWithTag ("BabyEnemy").Length == 0) {
		//		gameController.control.WeWon();
		//		checkForEndOfGame = false;
		//	}
		//}
}
