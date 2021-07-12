using UnityEngine;
using System.Collections;

public abstract class PowerUp : MovingTarget {
    public AudioClip loopSound;
    public AudioClip deathSound;
    public GameObject explo;
    // Access Singleton
    GameController gameController;
    PlayerMovement playerMovement;
    MusicController musicController;

    public override void Init() {
        gameController = GameController.control;
        playerMovement = PlayerMovement.control;
        musicController = MusicController.control;
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
        PlayOneShot(deathSound,true);
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

    public override IEnumerator PlaySound() {
        PlaySoundInLoop(loopSound);
        yield break;
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