using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource AS;
    public static MusicController control;
    // Start is called before the first frame update
    void Start()
    {
        if (!control) control = this;
        else Destroy(this);

        AS = GetComponent<AudioSource>();
    }

    public void ChangePitch(float pitch)
    {
        AS.pitch = pitch;
    }

    public void ReturnPitchToNormal()
    {
        AS.pitch = 1;
    }
}
