﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    //Game states
    public enum states
    {
        juststarted, idle, hitting, dead, paused
    };
    public states currentState;
    public bool paused;

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
    public AudioClip youWon, bombSound, snifSound;
    public bool upsideDownWorld;


    //Power ups
    public float powerUpTime = 2;
    public enum powerUpActivated
    {
        none, flash, bomb, lsd, ice
    }
    public powerUpActivated powerUp;
    public GameObject lsdCamera;
    GameObject player, music;
    float orgSpeedForMouse;
    public float frozenMouseSpeed, flashMouseSpeed;

    void Awake()
    {
        if (control == null)
        {
            control = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        this.currentState = states.juststarted;
        UpdateUI();
        StartCoroutine("SetUpStuff");
        AS = this.gameObject.AddComponent<AudioSource>();
        if (accountForLevels.control != null)
        {
            AS.volume = accountForLevels.control.fxVolume;
        }

        player = GameObject.Find("Player");
        music = GameObject.Find("Music");
        orgSpeedForMouse = player.GetComponent<PlayerMovement>().speed;
        Time.timeScale = 1.0f;

        cam = Camera.main.GetComponent<CameraFunctions>();
        if (!cam) Debug.LogError("Camera functions not set up");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseRoutine();
    }



    //TODO: Review this function...
    public void PauseRoutine()
    {
        if (currentState != states.juststarted)
        {
            if (!paused)
            {
                Pause();
                paused = true;
                pauseText.gameObject.SetActive(true);
                menuBut.gameObject.SetActive(true);
            }
            else
            {
                Resume();
                paused = false;
                pauseText.gameObject.SetActive(false);
                menuBut.gameObject.SetActive(false);
            }
        }
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

    public void Pause()
    {
        currentState = states.paused;
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

    }

    public void Resume()
    {
        currentState = states.idle;
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


    public void activatePowerUp(powerUpActivated pw, int myDamage)
    {
        int damageForThisLevel;

        if (accountForLevels.control != null)
        {
            damageForThisLevel = myDamage * accountForLevels.control.currentLevel;
        }
        else
        {
            damageForThisLevel = myDamage;
        }

        switch (pw)
        {
            case powerUpActivated.bomb:
                Debug.Log("got hit by bomb");
                takeDamage(damageForThisLevel, Color.red);
                AS.PlayOneShot(bombSound);
                break;
            case powerUpActivated.flash:
                Debug.Log("got hit by flash");
                takeDamage(damageForThisLevel, Color.yellow);
                player.GetComponent<PlayerMovement>().speed = flashMouseSpeed;
                player.GetComponent<PlayerMovement>().changeVisuals("flash");
                music.GetComponent<AudioSource>().pitch = 2;
                StartCoroutine("returnPlayerToNormal");
                break;
            case powerUpActivated.ice:
                takeDamage(damageForThisLevel, Color.cyan);
                player.GetComponent<PlayerMovement>().speed = frozenMouseSpeed;
                player.GetComponent<PlayerMovement>().changeVisuals("ice");
                music.GetComponent<AudioSource>().pitch = .5f;
                Debug.Log("got hit by ice");
                StartCoroutine("returnPlayerToNormal");
                break;
            case powerUpActivated.lsd:
                Debug.Log("got hit by lsd");
                takeDamage(damageForThisLevel, Color.magenta);
                Instantiate(lsdCamera, new Vector3(0, 4.2f, -0.24f), Quaternion.identity);
                music.GetComponent<AudioSource>().pitch = 5;
                AS.PlayOneShot(snifSound);
                break;
        }
    }


    IEnumerator returnPlayerToNormal()
    {
        yield return new WaitForSeconds(powerUpTime);
        music.GetComponent<AudioSource>().pitch = 1;
        player.GetComponent<PlayerMovement>().speed = orgSpeedForMouse;
        player.GetComponent<PlayerMovement>().ReturnVisualsToNormal();
    }

    public void returnMusicToNormal()
    {
        music.GetComponent<AudioSource>().pitch = 1;
    }


}
