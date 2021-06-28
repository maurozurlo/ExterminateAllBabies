using System;
using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public enum PowerUpActivated
    {
        none, flash, bomb, lsd, ice
    }
    public PowerUpActivated powerUp;
    //Power ups
    public float powerUpTime = 2;
    AudioSource AS;
    public AudioClip bombSound, snifSound;
    public float frozenMouseSpeed, flashMouseSpeed;

    public static PowerUpController control;
    GameController GameController;
    public GameObject player;
    PlayerMovement playerMovement;
    MusicController musicController;
    public Camera LSDCam;

    private void Start()
    {
        if (!control) control = this;
        else Destroy(this);

        AS = GetComponent<AudioSource>();
        GameController = GameController.control;
        playerMovement = player.GetComponent<PlayerMovement>();
        musicController = MusicController.control;
    }
    public void ActivatePowerUp(string powerup, int myDamage)
    {
        PowerUpActivated pw = (PowerUpActivated)Enum.Parse(typeof(PowerUpActivated), powerup);
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
            case PowerUpActivated.bomb:
                GameController.TakeDamage(damageForThisLevel, Color.red);
                AS.PlayOneShot(bombSound);
                break;
            case PowerUpActivated.flash:
                GameController.TakeDamage(damageForThisLevel, Color.yellow);
                playerMovement.ChangeSpeed(flashMouseSpeed);
                playerMovement.ChangeVisuals("flash");
                musicController.ChangePitch(2);
                break;
            case PowerUpActivated.ice:
                GameController.TakeDamage(damageForThisLevel, Color.cyan);
                playerMovement.ChangeSpeed(frozenMouseSpeed);
                playerMovement.ChangeVisuals("ice");
                musicController.ChangePitch(.5f);
                break;
            case PowerUpActivated.lsd:
                GameController.TakeDamage(damageForThisLevel, Color.magenta);
                LSDCam.depth = 2;
                musicController.ChangePitch(5);
                AS.PlayOneShot(snifSound);
                break;
        }
        StartCoroutine("ReturnPlayerToNormal");
    }


    IEnumerator ReturnPlayerToNormal()
    {
        yield return new WaitForSeconds(powerUpTime);
        LSDCam.depth = -99;
        musicController.ReturnPitchToNormal();
        playerMovement.ReturnToNormalSpeed();
        playerMovement.ReturnVisualsToNormal();
    }
}
