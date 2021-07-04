using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public Toggle tauntToggle;
    public Slider musicSlid, fxSlid;
    public AudioClip click;
    public GameObject subtitle, HelpButton;
    AudioSource AS;
    // Start is called before the first frame update
    public GameObject[] panels;
    LevelController levelController;

    private void Awake()
    {
        levelController = LevelController.control;
        AS = GetComponent<AudioSource>();
        // Setup
        if (LevelController.control && tauntToggle)
        {
            // Taunts
            bool taunt = !PlayerPrefs.HasKey("taunt") || System.Convert.ToBoolean(PlayerPrefs.GetInt("taunt"));
            tauntToggle.isOn = taunt;
            levelController.SetTaunts(taunt);
            // FX
            fxSlid.value = PlayerPrefs.HasKey("fxVol") ? PlayerPrefs.GetFloat("fxVol") : 1;
            levelController.SetVolume("fx", fxSlid.value);
            AS.volume = fxSlid.value;
            musicSlid.value = PlayerPrefs.HasKey("musicVol") ? PlayerPrefs.GetFloat("musicVol") : 1;
            // Music
            levelController.SetVolume("music", musicSlid.value);
        }
    }

    void HideAllPanels()
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }


    public void DisplayPanel(string panelToEnable)
    {
        ButtonClick();
        HideAllPanels();
        GameObject panel = GetPanel(panelToEnable);
        panel.SetActive(true);
    }

    public GameObject GetPanel(string panelToEnable)
    {
        GameObject selectedPanel = null;
        foreach (GameObject panel in panels)
        {
            if (panel.name == panelToEnable)
            {
                selectedPanel = panel;
            }
        }
        return selectedPanel;
    }

    public void LoadLevel(int levelToLoad)
    {
        ButtonClick();
        HideAllPanels();
        GameObject panel = GetPanel("Loading");
        panel.SetActive(true);
        panel.GetComponent<LoadSceneWithProgress>().LoadSceneNow($"level{levelToLoad}");
    }

    public void ButtonClick()
    {
        if (!AS.isPlaying && click != null)
        {
            AS.PlayOneShot(click);
        }
    }

    public void SaveConfig()
    {
        levelController.SetVolume("fx", fxSlid.value);
        levelController.SetVolume("music", musicSlid.value);
        levelController.SetTaunts(tauntToggle.isOn);
        PlayerPrefs.SetFloat("fxVol", fxSlid.value);
        PlayerPrefs.SetFloat("musicVol", musicSlid.value);
        PlayerPrefs.SetInt("taunt", tauntToggle.isOn ? 1 : 0);
        HideAllPanels();
        DisplayPanel("Main");
    }

    public void ActivateMenu()
    {
        subtitle.SetActive(true);
        DisplayPanel("Main");
        HelpButton.SetActive(true);
    }
    
    string CalculateLevel(){
        return levelController.GetNextLevel();
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}
