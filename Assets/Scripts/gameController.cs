using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    //Game states
    // New implementation
    public enum GameState {
        idle, play, pause, end
    };
    public GameState gameState;

    //UI
    CameraFunctions cam;
    public Text scoreText;
    public Text babyText;
    public Text pauseText, cuentaRegresiva;
    public Image damageScreen;
    public Button menuBut;

    //Game config
    public int babySpeedForThisLevel = 3;
    public int scoreInt;
    public int babyInt;
    public int totalBabies;
    public float speedMultiplier;
    public float delay;
    public int howManyInARow;

    //Singleton
    public static gameController control;
    public GameObject babyControl;

    //Audio
    AudioSource AS;
    public AudioClip[] hurtSound;
    public AudioClip youWon;
    public bool upsideDownWorld;

    void Awake()
    {
        // Singleton Pattern
        if (control == null)
            control = this;
        else
            Destroy(gameObject);
        // Initialize Level
        AS = gameObject.AddComponent<AudioSource>();
        if (accountForLevels.control != null)
            AS.volume = accountForLevels.control.fxVolume;
        cam = Camera.main.GetComponent<CameraFunctions>();
        if (!cam) Debug.LogError("Camera functions not set up");
        // Countdown + GameState
        gameState = GameState.idle;
        UpdateUI();
        StartCoroutine("SetUpStuff");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseRoutine();
    }



    //TODO: Review this function...
    public void PauseRoutine()
    {
        if (gameState == GameState.play)
        {
            Pause();
            return;
            
        }
        if(gameState == GameState.pause)
        {
            Resume();
            return;
        }
    }

    public void Pause()
    {
        gameState = GameState.pause;
        babyControl.GetComponent<spawnBabies>().halt();
        babyControl.GetComponent<spawnPowerUps>().halt();

        GameObject[] babiesNow = GameObject.FindGameObjectsWithTag("BabyEnemy");
        foreach (var item in babiesNow)
        {
            item.GetComponent<Baby>().kill();
            item.GetComponentInChildren<BabyAnimation>().alive = false;
        }

        GameObject[] pUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (var item in pUps)
        {
            item.GetComponent<genericPowerUp>().PausePU();
        }
        pauseText.gameObject.SetActive(true);
        menuBut.gameObject.SetActive(true);
    }

    public void Resume()
    {
        gameState = GameState.play;
        babyControl.GetComponent<spawnBabies>().resume();
        GameObject[] babiesNow = GameObject.FindGameObjectsWithTag("BabyEnemy");
        foreach (var item in babiesNow)
        {
            item.GetComponent<Baby>().revive();
            item.GetComponentInChildren<BabyAnimation>().alive = true;
        }

        babyControl.GetComponent<spawnPowerUps>().resume();
        GameObject[] pUps = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (var item in pUps)
        {
            item.GetComponent<genericPowerUp>().revive();
        }
        pauseText.gameObject.SetActive(false);
        menuBut.gameObject.SetActive(false);

    }


    IEnumerator SetUpStuff()
    {
        for (int i = 0; i < delay; i++)
        {
            cuentaRegresiva.text = (delay - i).ToString();
            yield return new WaitForSeconds(1);
        }
        cuentaRegresiva.text = 0.ToString();
        yield return new WaitForSeconds(1);
        cuentaRegresiva.text = "";
        babyControl.GetComponent<spawnBabies>().enabled = true;
        babyControl.GetComponent<spawnPowerUps>().enabled = true;
        currentState = states.idle;
        InvokeRepeating("IncreaseSpeed", 0, 5);
    }

    public void UpdateUI()
    {
        babyText.text = "/ " + babyInt.ToString();
        scoreText.text = "= " + scoreInt.ToString();
    }
    public void takeDamage(int dmgToTake, Color col)
    {
        scoreInt -= dmgToTake;
        UpdateUI();
        // Check if dead
        if (scoreInt <= -1)
        {
            currentState = states.dead;
            SceneManager.LoadScene("GameOver");
            return;
        }
        howManyInARow = 0;
        if (!AS.isPlaying){
            AS.PlayOneShot(hurtSound[Random.Range(0, hurtSound.Length)]);
        }
        // Camera shake
        if(dmgToTake > 0) StartCoroutine(cam.Shake(.4f, .2f));
        // Damage Screen
        StartCoroutine(DamageScreen(col));
    }

    IEnumerator DamageScreen(Color col)
    {
        damageScreen.color = col;
        damageScreen.gameObject.SetActive(true);
        damageScreen.GetComponent<ImgHandlers>().replayAnim();
        yield return new WaitForSeconds(1);
        damageScreen.gameObject.SetActive(false); 
    }


    public void WeWon()
    {
        StartCoroutine("WinRoutine");
    }

    IEnumerator WinRoutine()
    {
        cuentaRegresiva.text = "YOU WIN";
        AS.PlayOneShot(youWon);
        yield return new WaitForSeconds(youWon.length + 1);
        SceneManager.LoadScene("YouWon");
    }

    public void StopPowerUps()
    {
        babyControl.GetComponent<spawnPowerUps>().halt();
    }

    public void IncreaseSpeed()
    {
        babyControl.GetComponent<spawnBabies>().IncreaseSpeed(speedMultiplier);
    }

    public void BabyWasSpawned(int spawnedBabies)
    {
        babyInt = totalBabies - spawnedBabies;
        UpdateUI();
    }
    

    public void returnMusicToNormal()
    {
        music.GetComponent<AudioSource>().pitch = 1;
    }


}
