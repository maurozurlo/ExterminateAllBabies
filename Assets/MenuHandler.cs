using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public Toggle tauntToggle;
    public Slider musicSlid, fxSlid;
    public AudioClip click;
    AudioSource AS;
    // Start is called before the first frame update
    public GameObject[] panels;

    private void Awake()
    {
        AS = GetComponent<AudioSource>();
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
        foreach (GameObject panel in panels)
        {
            if (panel.name == panelToEnable) panel.SetActive(true);
        }
    }

    public void LoadLevel(int levelToLoad)
    {
        ButtonClick();
        DisplayPanel("Loading");
        // Do loading magic
        // Finally load level
    }

    public void ButtonClick()
    {
        if (!AS.isPlaying && click != null)
        {
            AS.PlayOneShot(click);
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }

}
