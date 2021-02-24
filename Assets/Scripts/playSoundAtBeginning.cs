using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundAtBeginning : MonoBehaviour {
	public AudioClip exterminateBabies;
	public AudioClip music;
	public bool playMusicAfterwards;
	public float delay;
	private AudioSource AS;

	// Use this for initialization
	void Start () {
		StartCoroutine ("playSound");
		AS = gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control != null) {
			AS.volume = accountForLevels.control.fxVolume;
		}
	}


	IEnumerator playSound(){
		yield return new WaitForSeconds (delay);
			AS.PlayOneShot(exterminateBabies);
		if (playMusicAfterwards) {
			yield return new WaitForSeconds (exterminateBabies.length + delay);
			PlayMusic ();
		}
		}

	void PlayMusic(){
		if (accountForLevels.control != null) {
			AS.volume = accountForLevels.control.musicVolume;
		}
		AS.loop = true;
		AS.clip = music;
		AS.Play ();
	}
}
