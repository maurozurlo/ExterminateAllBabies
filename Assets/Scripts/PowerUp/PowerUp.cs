using UnityEngine;
using System.Collections;

public abstract class PowerUp : MonoBehaviour {
    float toZ, toY, speed;
    Vector2 boundsX;
    Vector2 boundsZ;

    AudioSource AS;
    public AudioClip loopSound;
    public AudioClip deathSound;
    public GameObject explo;
    // Access Singleton
    GameController gameController;
    PlayerMovement playerMovement;
    MusicController musicController;

    public void PlayOneShot(AudioClip clip) {
        AS.PlayOneShot(clip);
    }

    void Start() {
        // References
        gameController = GameController.control;
        playerMovement = PlayerMovement.control;
        musicController = MusicController.control;
        // Get level values
        // Hardcoded for now...
        toZ = 4.5f;
        toY = -2.72f;
        boundsX = new Vector2(0, 3.33f);
        boundsZ = new Vector2(-20, -17);
        speed = 4;


        transform.position = new Vector3(Random.Range(boundsX.x, boundsX.y), -3.53f, Random.Range(boundsZ.x, boundsZ.y));
        AS = GetComponent<AudioSource>();
        StartCoroutine("RiseUp");
        PlayLoopedSound();
        AS.volume = LevelController.control.GetVolume("fx");
    }

    IEnumerator RiseUp() // Replace with an animation...
    {
        while (transform.position.y < toY) {
            transform.position += new Vector3(0, .04f, 0);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine("StartWalking");
    }

    IEnumerator StartWalking() // Get baby speed from game controller
    {
        while (transform.position.z < toZ) {
            transform.position += new Vector3(0, 0, speed / 100);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ActivatePowerUp() {
        StopAllCoroutines();
        DeactivateVisuals();
        ActivateEffect();
    }

    public void KillWithSwatter() {
        StopAllCoroutines();
        Instantiate(explo, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        DeactivateVisuals();
        AS.Stop();
        AS.PlayOneShot(deathSound);
        Destroy(gameObject, deathSound.length);
    }

    void DeactivateVisuals() {
        foreach (Renderer item in GetComponentsInChildren<Renderer>()) {
            item.enabled = false;
        }
        foreach (Collider item in GetComponentsInChildren<Collider>()) {
            item.enabled = false;
        }
    }

    void PlayLoopedSound() {
        AS.loop = true;
        AS.clip = loopSound;
        AS.Play();
    }

    public abstract void ActivateEffect();

    public void DoDamage(int damage, Color color) {
        gameController.TakeDamage(damage, color);
    }

    public IEnumerator ReturnPlayerToNormal(float duration) {
        yield return new WaitForSeconds(duration);
        MusicController.control.ReturnPitchToNormal();
        playerMovement.ReturnToNormalSpeed();
        playerMovement.ReturnVisualsToNormal();
    }

    public PlayerMovement GetPlayerMovement() {
        return playerMovement;
    }

    public MusicController GetMusicController() {
        return musicController;
    }
}