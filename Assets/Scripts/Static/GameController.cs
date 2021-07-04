using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
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
    public float speedMultiplier;
    public float delay;
    public int howManyInARow;

    //Singleton
    public static GameController control;

    //Audio
    AudioSource AS;
    public AudioClip[] hurtSound;
    public AudioClip youWon;
    public bool upsideDownWorld;

    void Awake()
    {
        // Singleton Pattern
        if (!control) control = this;
        else Destroy(gameObject);
        // Initialize Level
        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = LevelController.control.GetVolume("fx");
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
        Time.timeScale = 0;
        pauseText.gameObject.SetActive(true);
        menuBut.gameObject.SetActive(true);
    }

    public void Resume()
    {
        gameState = GameState.play;
        Time.timeScale = 1;
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
        cuentaRegresiva.text = "DALEE!!!";
        yield return new WaitForSeconds(1);
        cuentaRegresiva.text = "";
        gameState = GameState.play;
        InvokeRepeating("IncreaseSpeed", 0, 5);
    }

    public void UpdateUI()
    {
        babyText.text = "/ " + babyInt.ToString();
        scoreText.text = "= " + scoreInt.ToString();
    }
    public void TakeDamage(int dmgToTake, Color col)
    {
        scoreInt -= dmgToTake;
        UpdateUI();
        // Check if dead
        if (scoreInt <= -1)
        {
            gameState = GameState.end;
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

    public void IncreaseSpeed()
    {
        SpawnBabies.control.IncreaseSpeed(speedMultiplier);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

}
