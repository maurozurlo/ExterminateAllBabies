using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{

    public float displacementMax, displacementMin;
    public float maxX, minX, maxY, minY;
    public float speed;
    public bool hitting;
    public bool dead;
    public float secondsToWait;
    public Sprite frozenMaterial, flashMaterial;
    Sprite spriteOrg;
    public GameObject wasp;
    AudioSource AS;
    public AudioClip swat;

    void Start()
    {
        AS = this.gameObject.AddComponent<AudioSource>();
        if (accountForLevels.control != null)
        {
            AS.volume = accountForLevels.control.fxVolume;
        }
        spriteOrg = this.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    void Update()
    {
        //TODO find out if this works on mobile	
        //hor/ver should be CrossPlatformInputManager.GetAxis ("Horizontal")?
        float hor = -CrossPlatformInputManager.VirtualAxisReference("Horizontal").GetValue;
        float ver = CrossPlatformInputManager.VirtualAxisReference("Vertical").GetValue;

        Vector3 movementX = new Vector3(hor, 0, 0) * Time.deltaTime * speed;
        Vector3 movementY = new Vector3(0, ver, 0) * Time.deltaTime * speed;
        if (gameController.control.currentState != gameController.states.paused)
        {
            if (!dead)
            {
                if (isWithinRange(transform.position.x + movementX.x, minX, maxX))
                {
                    this.transform.position += movementX;
                }

                if (isWithinRange(transform.position.y + movementY.y, minY, maxY))
                {
                    this.transform.position += movementY;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
					StartHitting();
                }
            }
        }
    }

    public void StartHitting()
    {
        if (!hitting)
        StartCoroutine("Hit");
    }

    public bool isWithinRange(float val, float min, float max)
    {
        if (val >= min && val <= max)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator Hit()
    {
        hitting = true;
        gameController.control.currentState = gameController.states.hitting;
        wasp.GetComponent<Animator>().SetBool("hit", true);
        if (!AS.isPlaying)
        {
            AS.PlayOneShot(swat);
        }
        yield return new WaitForSeconds(secondsToWait);
        wasp.GetComponent<Animator>().SetBool("hit", false);
        yield return new WaitForSeconds(secondsToWait);
        hitting = false;
        gameController.control.currentState = gameController.states.idle;
    }


    public void Die()
    {
        dead = true;
        gameController.control.currentState = gameController.states.dead;
        //Mostrarte la screen de q te moriste
    }


    public void returnVisualsToNormal()
    {
        this.GetComponentInChildren<SpriteRenderer>().sprite = spriteOrg;
    }

    public void changeVisuals(string mySkin)
    {
        switch (mySkin)
        {
            case "ice":
                this.GetComponentInChildren<SpriteRenderer>().sprite = frozenMaterial;
                break;
            case "flash":
                this.GetComponentInChildren<SpriteRenderer>().sprite = flashMaterial;
                break;
        }
    }
}
