using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 max, min;
    private float orgSpeed;
    public float speed;
    public bool hitting;
    public float secondsToWait;
    public Sprite frozenMaterial, flashMaterial;
    Sprite spriteOrg;
    SpriteRenderer spriteRenderer;
    public GameObject wasp;
    AudioSource AS;
    public AudioClip swat;
    GameController gameController;
    public bool DEBUG_CONTROLS;

    void Start()
    {
        AS = gameObject.AddComponent<AudioSource>();
        if (accountForLevels.control != null)
        {
            AS.volume = accountForLevels.control.fxVolume;
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteOrg = spriteRenderer.sprite;
        orgSpeed = speed;
        gameController = GameController.control;
    }

    void Update()
    {
        if (gameController.gameState != GameController.GameState.play) return;
        Movement();
        HitRoutine();
    }

    void Movement()
    {
        float hor, ver;

        if (!DEBUG_CONTROLS)
        {
            hor = -CrossPlatformInputManager.VirtualAxisReference("Horizontal").GetValue;
            ver = CrossPlatformInputManager.VirtualAxisReference("Vertical").GetValue;
        }
        else
        {
            hor = -Input.GetAxisRaw("Horizontal");
            ver = Input.GetAxisRaw("Vertical");
        }
        
        Vector3 movementX = new Vector3(hor, 0) * Time.deltaTime * speed;
        Vector3 movementY = new Vector3(0, ver) * Time.deltaTime * speed;
        if (IsWithinRange(transform.position.x + movementX.x, min.x, max.x)) transform.position += movementX;
        if (IsWithinRange(transform.position.y + movementY.y, min.y, max.y)) transform.position += movementY;
    }

    void HitRoutine()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!hitting)
                StartCoroutine("Hit");
        }
    }

    public bool IsWithinRange(float val, float min, float max)
    {
        return val >= min && val <= max;
    }


    IEnumerator Hit()
    {
        hitting = true;
        wasp.GetComponent<Animator>().SetBool("hit", true);
        if (!AS.isPlaying) AS.PlayOneShot(swat);
        yield return new WaitForSeconds(secondsToWait);
        wasp.GetComponent<Animator>().SetBool("hit", false);
        yield return new WaitForSeconds(secondsToWait);
        hitting = false;
    }

    public void ReturnVisualsToNormal()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = spriteOrg;
    }

    public void ChangeVisuals(string mySkin)
    {
        switch (mySkin)
        {
            case "ice":
                GetComponentInChildren<SpriteRenderer>().sprite = frozenMaterial;
                break;
            case "flash":
                GetComponentInChildren<SpriteRenderer>().sprite = flashMaterial;
                break;
        }
    }

    public void ChangeSpeed(float toSpeed)
    {
        speed = toSpeed;
    }

    public void ReturnToNormalSpeed()
    {
        speed = orgSpeed;
    }
}
