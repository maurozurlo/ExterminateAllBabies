using UnityEngine;

public class volumeWorkAround : MonoBehaviour {
	public bool musicControl;
	AudioSource audioSource;
	void Start () {
		audioSource = GetComponent<AudioSource>();
		if (accountForLevels.control && audioSource)
			audioSource.volume = musicControl ? accountForLevels.control.musicVolume : accountForLevels.control.fxVolume;
	}
}
