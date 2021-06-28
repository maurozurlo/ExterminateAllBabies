using System.Collections;
using UnityEngine;

public class SpawnBabies : MonoBehaviour {
	public GameObject[] baby;
	public float minTime,maxTime;

	public Vector2Int spawnedBabies;
	public int probOfbaby1,probOfbaby2,probOfbaby3; // TODO: Use a weighted table instead...

	GameController gameController;
	public static SpawnBabies control;
	
	void Start () {
		// Singleton
		if (!control) control = this;
		else Destroy(this);

		gameController = GameController.control;
		StartCoroutine ("SpawnBaby");
	}

	public void IncreaseSpeed(float speedMul)
	{
		if (maxTime > minTime) maxTime *= speedMul;
	}

	IEnumerator SpawnBaby () {
		// Check if all babies have been spawned already
		if (spawnedBabies.x == spawnedBabies.y) yield break;
		// Only spawn babies if we're in play state, otherwise just wait and try again in x seconds
		if (gameController.GetGameState() == GameController.GameState.play)
        {
			// Pick a random baby
			GameObject Baby = Instantiate(baby[PickABaby()], transform.position, Quaternion.identity);
			Baby.tag = "BabyEnemy";
			spawnedBabies = new Vector2Int(spawnedBabies.x++, spawnedBabies.y);
		}
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
}
