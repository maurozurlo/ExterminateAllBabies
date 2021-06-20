using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public enum powerUpActivated
    {
        none, flash, bomb, lsd, ice
    }
    public powerUpActivated powerUp;
    public GameObject lsdCamera;
    //Power ups
    public float powerUpTime = 2;
    public AudioClip bombSound, snifSound;
    public float frozenMouseSpeed, flashMouseSpeed;
    // Start is called before the first frame update
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
}
