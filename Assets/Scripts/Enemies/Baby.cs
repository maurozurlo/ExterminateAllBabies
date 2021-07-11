using System.Collections;
using UnityEngine;

public class Baby : MonoBehaviour {


	public float toZ, toY;
	public float speed;
	public float maxX,minX,maxZ,minZ;
	public float minTime,maxTime;
	AudioSource AS;
	public AudioClip[] randSounds;
	public float intervalBetweenSounds;
	public int damage = 1;
	BabyAnimation animator;
	
	void Start () {
		AS = gameObject.AddComponent<AudioSource>();
		animator = GetComponentInChildren<BabyAnimation>();

		AS.volume = LevelController.control.GetVolume("fx");

		transform.position = new Vector3 (Random.Range (minX, maxX), -3.53f,Random.Range (minZ, maxZ));
		StartCoroutine("RiseUp");
		StartCoroutine("PlayRandSound");

		speed = GameController.control.GetEnemySpeed();
	}

	IEnumerator RiseUp(){
		while (transform.position.y < toY) {
			transform.position += new Vector3 (0, .04f, 0);
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine("StartWalking");
	}

	IEnumerator StartWalking(){
		while (transform.position.z < toZ) {
			transform.position += new Vector3 (0, 0, speed / 100);
			yield return new WaitForEndOfFrame();
		}
	}

	public void KillWithSwatter() {
		StopMoving();
		animator.Hit();
	}

	public void ReachDestination() {
		animator.HideAndDestroy();
	}

	public void StopMoving() {
		StopAllCoroutines();
	}

	public void Restart(){
		StartCoroutine("StartWalking");
		StartCoroutine("PlayRandSound");
	}


	IEnumerator PlayRandSound(){
		if (!AS.isPlaying) {
			AS.PlayOneShot (randSounds [Random.Range (0, randSounds.Length)]);
		}
		yield return new WaitForSeconds (Random.Range (0, intervalBetweenSounds));
		StartCoroutine ("PlayRandSound");
	}


}
