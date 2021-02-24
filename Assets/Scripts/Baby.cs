using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour {


	public float toZ, toY;
	public float babySpeed;
	public float maxX,minX,maxZ,minZ;
	public float minTime,maxTime;
	float disp;
	AudioSource AS;
	public AudioClip[] randSounds;
	public float intervalBetweenSounds;
	public int damage = 1;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (Random.Range (minX, maxX), -3.53f,Random.Range (minZ, maxZ));
		AS = this.gameObject.AddComponent<AudioSource> ();
		StartCoroutine("RiseUp");
		StartCoroutine("PlayRandSound");
		babySpeed = gameController.control.babySpeedForThisLevel;

		if (accountForLevels.control != null) {
			AS.volume = accountForLevels.control.fxVolume;
		}
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
			transform.position += new Vector3 (0, 0, babySpeed/100);
			yield return new WaitForEndOfFrame();
		}
	}

	public void kill(){
		StopAllCoroutines ();
	}


	public void revive(){
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
