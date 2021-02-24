using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeWorkAround : MonoBehaviour {
	public bool musicControl;

	// Use this for initialization
	void Start () {
		if (accountForLevels.control != null && this.gameObject.GetComponent<AudioSource> () != null) {
			if(musicControl == true){
				this.gameObject.GetComponent<AudioSource> ().volume = accountForLevels.control.musicVolume;
			}else{
				this.gameObject.GetComponent<AudioSource>().volume = accountForLevels.control.fxVolume;
			}
		}
	}
}
