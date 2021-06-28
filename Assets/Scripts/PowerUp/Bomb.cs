using UnityEngine;

public class Bomb : PowerUp
{
    public AudioClip bombSound;
    public int damage;
    public override void ActivateEffect()
    {
        int damageForThisLevel;
        if (accountForLevels.control != null)
        {
            damageForThisLevel = damage * accountForLevels.control.currentLevel;
        }
        else
        {
            damageForThisLevel = damage;
        }
        DoDamage(damageForThisLevel, Color.red);
        PlayOneShot(bombSound);
        // Dissapear
        Destroy(gameObject,bombSound.length);
    }
}
