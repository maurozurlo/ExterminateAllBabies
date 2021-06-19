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

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
        // Old Code
        // Setup
        if (accountForLevels.control && tauntToggle)
        {
            // Taunts
            bool taunt = !PlayerPrefs.HasKey("taunt") || System.Convert.ToBoolean(PlayerPrefs.GetInt("taunt"));
            tauntToggle.isOn = taunt;
            accountForLevels.control.taunt = taunt;
            // FX
            fxSlid.value = PlayerPrefs.HasKey("fxVol") ? PlayerPrefs.GetFloat("fxVol") : 1;
            accountForLevels.control.fxVolume = fxSlid.value;
            AS.volume = accountForLevels.control.fxVolume;
            musicSlid.value = PlayerPrefs.HasKey("musicVol") ? PlayerPrefs.GetFloat("musicVol") : 1;
            // Music
            accountForLevels.control.musicVolume = musicSlid.value;
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
        if (accountForLevels.control != null)
        {
            accountForLevels.control.fxVolume = fxSlid.value;
            accountForLevels.control.musicVolume = musicSlid.value;
            accountForLevels.control.taunt = tauntToggle.isOn;
            PlayerPrefs.SetFloat("fxVol", fxSlid.value);
            PlayerPrefs.SetFloat("musicVol", musicSlid.value);
            if (tauntToggle.isOn)
            {
                PlayerPrefs.SetInt("taunt", 1);
            }
            else
            {
                PlayerPrefs.SetInt("taunt", 0);
            }
            
        }
        HideAllPanels();
        DisplayPanel("Main");
    }

    public void ActivateMenu()
    {
        subtitle.SetActive(true);
        DisplayPanel("Main");
        HelpButton.SetActive(true);
    }
    // Ni idea que hace esto
    string calculateLevel()
    {
        if (accountForLevels.control.currentLevel < accountForLevels.control.maxLevels)
        {
            accountForLevels.control.currentLevel++;
            string b = "level" + accountForLevels.control.currentLevel.ToString();
            return b;
        }
        else
        {
            return "endGame";
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}
