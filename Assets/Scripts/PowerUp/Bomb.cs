using UnityEngine;

public class Bomb : PowerUp {
    public AudioClip bombSound;
    public int damage;
    public override void ActivateEffect() {
        int damageForThisLevel;

        damageForThisLevel = damage * LevelController.control.GetCurrentLevel();

        DoDamage(damageForThisLevel, Color.red);
        PlayOneShot(bombSound);
        // Dissapear
        Destroy(gameObject, bombSound.length);
    }
}