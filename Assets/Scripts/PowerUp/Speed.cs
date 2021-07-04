using UnityEngine;

public class Speed : PowerUp
{
    public Sprite overrideSprite;
    public float overrideSpeed;
    public Color explosionColor;
    public float pitchOverride;
    public float duration;

    public override void ActivateEffect()
    {
        DoDamage(0, explosionColor);
        PlayerMovement playerMovement = GetPlayerMovement();
        playerMovement.ChangeSpeed(overrideSpeed);
        playerMovement.ChangeVisuals(overrideSprite);
        GetMusicController().ChangePitch(pitchOverride);

        StartCoroutine(ReturnPlayerToNormal(duration));
        Destroy(gameObject, duration + .5f);
    }
}
