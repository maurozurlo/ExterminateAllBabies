using System.Collections;
using UnityEngine;

public class DeadZone : MonoBehaviour {
	public AudioClip[] adrielTaunts;
	private AudioSource AS;
	public int probabilidad;
	public int lastTauntPlayed;
	public int prob;
	int damageForThisLevel;
	public float delay;
	public int tauntToPlay;
	bool tauntsEnabled = true;
	LevelController levelController;

	void Start()
    {
		levelController = LevelController.control;

		AS = gameObject.AddComponent<AudioSource>();

		tauntsEnabled = levelController.TauntsEnabled();
		AS.volume = levelController.GetVolume("fx");

		probabilidad = tauntsEnabled ? probabilidad : 0;
    }

	void OnTriggerEnter(Collider col){
		// Babies
		if (col.CompareTag("Enemy")) {
			col.gameObject.GetComponent<Baby>().kill();
			col.gameObject.GetComponentInChildren<BabyAnimation> ().ReachDestination ();
			damageForThisLevel = col.gameObject.GetComponentInParent<Baby>().damage * levelController.GetCurrentLevel();
			GameController.control.TakeDamage(damageForThisLevel,Color.red);
		}
		// PowerUps
		if (col.CompareTag("PowerUp")) {
			PowerUp currentPowerUp = col.gameObject.GetComponent<PowerUp>();
			currentPowerUp.ActivatePowerUp();
		}
		///Taunt
		StartCoroutine ("TryAndPlay");
	}

	IEnumerator TryAndPlay(){
		yield return new WaitForSeconds (delay);
		prob = Random.Range (0, 100);
		if (prob < probabilidad) {
			PlayTaunt ();
		}
	}

	void PlayTaunt(){
		if (!AS.isPlaying) {
		tauntToPlay = Random.Range (0, adrielTaunts.Length);
		//para que no se repitan los taunts
		if (tauntToPlay == lastTauntPlayed) {
			if (tauntToPlay + 1 < adrielTaunts.Length) {
				lastTauntPlayed = tauntToPlay + 1;
			} else if (tauntToPlay - 1 >= 0) {
				lastTauntPlayed = tauntToPlay - 1;
			}
		} else {
			lastTauntPlayed = tauntToPlay;
		}
			AS.PlayOneShot (adrielTaunts [lastTauntPlayed]);
		}
	}

}
