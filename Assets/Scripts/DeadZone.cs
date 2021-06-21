using System.Collections;
using System.Collections.Generic;
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
	public bool tauntsEnabled = true;
	PowerUpController powerUpController;
	void Start()
    {
		AS = this.gameObject.AddComponent<AudioSource>();
		if (accountForLevels.control != null)
		{
			tauntsEnabled = accountForLevels.control.taunt;
			AS.volume = accountForLevels.control.fxVolume;
		}

		if (!tauntsEnabled)
		{
			probabilidad = 0;
		}
		powerUpController = PowerUpController.control;
    }

	void OnTriggerEnter(Collider col){
			if (col.name == "babyPlane") {
				col.gameObject.GetComponent<BabyAnimation> ().reachDestination ();
				col.gameObject.GetComponentInParent<Baby> ().kill ();
			if (accountForLevels.control != null) {
				damageForThisLevel = col.gameObject.GetComponentInParent<Baby> ().damage * accountForLevels.control.currentLevel;
			} else {
				damageForThisLevel = col.gameObject.GetComponentInParent<Baby> ().damage;
			}
			GameController.control.TakeDamage(damageForThisLevel,Color.red);
			}
		//powerUps
		if (col.name == "powerUp") {
			genericPowerUp currentPowerUp = col.gameObject.GetComponent<genericPowerUp>();
			currentPowerUp.Kill ();
			powerUpController.ActivatePowerUp(currentPowerUp.myType.ToString(), currentPowerUp.damage);
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
