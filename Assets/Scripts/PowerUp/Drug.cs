using UnityEngine;
using System.Collections;

public class Drug : PowerUp
{
    GameObject overlay;
    public Color explosionColor;
    public float pitchOverride;
    public float duration;
    float originalFOV;
    public float FOV = 17;
    public override void ActivateEffect()
    {
        overlay = GameObject.FindGameObjectWithTag("LSDFX");
        ChangeOverlayState(true);
        originalFOV = Camera.main.fieldOfView;
        Camera.main.fieldOfView = FOV;
        GetMusicController().ChangePitch(pitchOverride);
        StartCoroutine(RemoveEffect());
        Destroy(gameObject, duration + .5f);
    }
    IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(duration);
        ChangeOverlayState(false);
        Camera.main.fieldOfView = originalFOV;
        GetMusicController().ReturnPitchToNormal();
    }

    public void ChangeOverlayState(bool isActive)
    {
        overlay.GetComponent<UnityEngine.UI.Image>().enabled = isActive;
        overlay.GetComponent<LSDFX>().enabled = isActive;
    }
}
