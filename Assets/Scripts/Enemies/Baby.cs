﻿using System.Collections;
using UnityEngine;

public class Baby : MovingTarget {

	public AudioClip[] randSounds;
	public float intervalBetweenSounds;
	public int damage = 1;
	BabyAnimation animator;
	
	public override void Init () {
		ChangeGround(-2.02f);
		animator = GetComponentInChildren<BabyAnimation>();
	}

	public void KillWithSwatter() {
		StopMoving();
		RemoveParent();
		animator.Hit();
	}

	public void ReachDestination() {
		RemoveParent();
		animator.HideAndDestroy();
	}

	public override IEnumerator PlaySound(){
		PlayOneShot (randSounds [Random.Range (0, randSounds.Length)]);
		yield return new WaitForSeconds (Random.Range (0, intervalBetweenSounds));
		StartCoroutine ("PlaySound");
	}

	void RemoveParent() {
		gameObject.transform.SetParent(null);
    }

}
