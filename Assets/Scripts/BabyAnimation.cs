using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAnimation : MonoBehaviour {

	public Texture2D[] frames;
	public Texture2D[] hitFrame;
	public TextMesh comboText;
	public TextMesh multiplierText, shadowText;
	public float period = 0.1f;
	public float time= 0;
	public int imgWereAt = 0;
	public bool alive = true;
	public bool shake;
	public float speed = 1.0f;
	public float amount = 1.0f;
	public AudioClip combo1,combo2,combo3,combo4;
	private Vector3 orgPos;
	AudioSource AS;
	public AudioClip[] cry;
	int scoreToAdd;
	public GameObject exploPrefab, pivotPoint;

	// Use this for initialization
	void Update() {
		if (alive) {
			time += Time.deltaTime;

			if (time >= period) {
				time = time - period;
				if (imgWereAt < frames.Length - 1) {
					imgWereAt++;
					this.GetComponent<MeshRenderer> ().material.mainTexture = frames [imgWereAt];
				} else {
					imgWereAt = 0;
					this.GetComponent<MeshRenderer> ().material.mainTexture = frames [imgWereAt];
				}
			}
		}

		if (shake) {
			float shk = Mathf.Sin (Time.deltaTime * speed) * amount;
			transform.position = orgPos + new Vector3 (shk, shk, 0);
		}
	}
	
	// Update is called once per frame
	public void Hit () {
		alive = false;
		this.GetComponent<MeshRenderer> ().material.mainTexture = hitFrame[Random.Range(0,hitFrame.Length)];
		AS = this.gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control != null) {
			AS.volume = accountForLevels.control.fxVolume;
		}
		AS.PlayOneShot (cry [Random.Range (0, cry.Length)]);
		orgPos = this.transform.position;
		shake = true;
		StartCoroutine("KillMe");
	}


	void Start(){
		if (GameController.control.upsideDownWorld) {
			comboText.gameObject.transform.Rotate(new Vector3(0,0,-180));
			shadowText.gameObject.transform.Rotate(new Vector3(0,0,-180));
			multiplierText.gameObject.transform.Rotate(new Vector3(0,0,-180));
		}
	}

	public void reachDestination(){
		alive = false;
		this.GetComponent<MeshRenderer> ().enabled = false;
		this.GetComponent<BoxCollider> ().enabled = false;
		//algun coso rojo que te diga q perdiste puntos
		Destroy (this.transform.parent.gameObject,2);
	}

	IEnumerator KillMe(){
		if (exploPrefab != null && pivotPoint != null) {
			Instantiate (exploPrefab, pivotPoint.transform.position, Quaternion.identity);
		}

		this.GetComponent<BoxCollider> ().enabled = false;
		AddPoints (GameController.control.howManyInARow);
		yield return new WaitForSeconds (2);
		Destroy (this.transform.parent.gameObject);
	}


	public void AddPoints(int typeOfCombo){
		AudioClip clipToPlay = null;
		if (typeOfCombo < 11) {
			switch (typeOfCombo) {
			default:
				comboText.text = "";
				shadowText.text = "";
				scoreToAdd = 1;
				break;
			case 2:
				comboText.color = Color.magenta;
				comboText.text = "EXCELLENT";
				shadowText.text = "EXCELLENT";
				scoreToAdd = 2;
				multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
				clipToPlay = combo1;
				break;
			case 3:
			case 4:
				comboText.color = Color.yellow;
				comboText.text = "OUTSTANDING";
				shadowText.text = "OUTSTANDING";
				scoreToAdd = 3;
				multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
				clipToPlay = combo2;
				break;
			case 5:
				comboText.color = Color.blue;
				comboText.text = "COMBO";
				shadowText.text = "COMBO";
				scoreToAdd = 4;
				multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
				clipToPlay = combo3;
				break;
			case 6:
			case 7:
			case 8:
			case 9:
				comboText.color = Color.blue;
				comboText.text = "COMBO";
				shadowText.text = "COMBO";
				scoreToAdd = 4;
				multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
				clipToPlay = null;
				break;
			case 10:
				comboText.color = Color.red;
				comboText.text = "ULTRA COMBO";
				shadowText.text = "ULTRA COMBO";
				scoreToAdd = 5;
				multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
				clipToPlay = combo4;
				break;
			}
		} else {
			comboText.color = Color.red;
			comboText.text = "ULTRA COMBO";
			shadowText.text = "ULTRA COMBO";
			scoreToAdd = 5;
			multiplierText.text = "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd-1).ToString() + ")";
			clipToPlay = null;
		}

		AudioSource AS2 = this.gameObject.AddComponent<AudioSource> ();
		if (accountForLevels.control != null) {
			AS2.volume = accountForLevels.control.fxVolume;
		}
		if(clipToPlay != null)
			AS2.PlayOneShot (clipToPlay);
		GameController.control.scoreInt += scoreToAdd;
		GameController.control.howManyInARow++;
		GameController.control.UpdateUI();
	}

}
