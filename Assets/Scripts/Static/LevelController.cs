using UnityEngine;

public class LevelController : MonoBehaviour {


	public static LevelController control;
	public int currentLevel;
	public int maxLevels = 5;
	public int unlockedLevel;
    private bool taunt = true;
    private float fxVolume = 1;
    private float musicVolume = 1;

	// Use this for initialization
	void Awake () {
		if (control == null) {
			control = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}

		if (PlayerPrefs.HasKey ("unlockedLevels")) {
			unlockedLevel = PlayerPrefs.GetInt ("unlockedLevels");
		}
	}

	public void AddNewUnlockedLevel(){ // this should work with a string or something
		int tryTo = currentLevel + 2;
		if (tryTo > unlockedLevel) {
			PlayerPrefs.SetInt ("unlockedLevels", tryTo);
			unlockedLevel = PlayerPrefs.GetInt ("unlockedLevels");
		}
	}

	public float GetVolume(string setting)
    {
		if (setting == "music") return musicVolume;
		return fxVolume;
    }

	public void SetVolume(string setting, float value)
    {
		if (setting == "music") musicVolume = value;
        else{
			fxVolume = value;
		}
	}

	public bool TauntsEnabled()
    {
		return taunt;
    }

	public void SetTaunts(bool value)
    {
		taunt = value;
    }

	public int GetCurrentLevel()
    {
		return currentLevel;
    }

	public int GetUnlockedLevel()
    {
		return unlockedLevel;
	}

	public string GetNextLevel()
    {
		if (currentLevel < maxLevels){
			currentLevel++;
			return $"level {currentLevel}";
		}
		return "endGame";
	}
}
