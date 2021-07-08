using UnityEngine;

public class VolumeWorkAround : MonoBehaviour {
	public bool musicControl;
	AudioSource audioSource;
	void Start () {
		LevelController levelController = LevelController.control;
		audioSource = GetComponent<AudioSource>();

		if (audioSource)
			audioSource.volume = musicControl ? levelController.GetVolume("music") : levelController.GetVolume("fx");
	}
}
