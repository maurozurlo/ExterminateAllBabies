using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericPowerUp : MonoBehaviour {

	public float toZ, toY;
	public float babySpeed;
	public float maxX,minX,maxZ,minZ;
	public float minTime,maxTime;
	public enum TypeOfPowerUp{ 
		flash,ice,bomb,lsd // TODO this could be done some other way...
	}
	public TypeOfPowerUp myType;

	AudioSource AS;
	public AudioClip loopSound;
	public int damage = 5;
	public AudioClip deathSound;
	public GameObject explo;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (Random.Range (minX, maxX), -3.53f,Random.Range (minZ, maxZ));
		AS = this.gameObject.AddComponent<AudioSource> ();
		StartCoroutine("RiseUp");
		PlayLoopedSound();
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

	public void Kill(){
		StopAllCoroutines ();
		foreach (Renderer item in GetComponentsInChildren<Renderer>()) {
			item.enabled = false;
		}

		Destroy (this.gameObject, 2f);
	}

	public void KillWithWasp(){
		StopAllCoroutines ();
		Instantiate (explo, this.transform.position + new Vector3 (0, 0.8f, 0), Quaternion.identity);
		foreach (Renderer item in GetComponentsInChildren<Renderer>()) {
			item.enabled = false;
		}
		foreach (Collider item in GetComponentsInChildren<Collider>()) {
			item.enabled = false;
		}
		AS.Stop ();
		AS.PlayOneShot (deathSound);
		Destroy (this.gameObject, deathSound.length);
	}

	void PlayLoopedSound(){
		AS.loop = true;
		AS.clip = loopSound;
		AS.Play ();
	}
}
