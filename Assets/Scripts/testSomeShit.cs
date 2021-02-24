using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class testSomeShit : MonoBehaviour {
	public PostProcessingProfile PBheaviour;
	ColorGradingModel.Settings thing;
	public float val, min, max, speed,t;
	// Use this for initialization
	void Start () {
		thing = PBheaviour.colorGrading.settings;
	}

	void Update(){
		float actualVal = Mathf.Lerp (min, max, t);
		t += speed * Time.deltaTime;
		thing.basic.hueShift = actualVal;
		PBheaviour.colorGrading.settings = thing;

		if (t > 1.0f) {
			float temp = max;
			max = min;
			min = temp;
			t = 0;
		}

	}

}
