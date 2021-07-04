using UnityEngine;

public class cleanUp : MonoBehaviour
{

    public enum typeOfScreen
    {
        menu, win, lost
    }
    public typeOfScreen myType;
    bool restartwithEnter, activateRestart;

    void Awake()
    {
        Time.timeScale = 1.0f;
        LevelController.control.AddNewUnlockedLevel();

        switch (myType)
        {
            case typeOfScreen.win:
            case typeOfScreen.lost:
                activateRestart = true;
                break;
        }
    }

    // ??? no idea
    void Update()
    {
        if (activateRestart)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!restartwithEnter)
                {
                    if (myType == typeOfScreen.win)
                    {
                        //this.gameObject.GetComponent<MainMenu> ().ButtonClick ("nextLevel");
                    }
                    if (myType == typeOfScreen.lost)
                    {
                        //this.gameObject.GetComponent<MainMenu> ().ButtonClick ("levelRestart");
                    }
                    restartwithEnter = true;
                }
            }
        }
    }

}
