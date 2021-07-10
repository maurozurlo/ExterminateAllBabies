using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour {
    public GameConfig config;
    public GameObject Grass;

    public static GameInit control;

    void Awake() {
        //Singleton Pattern
        if (!control) control = this;
        else DestroyImmediate(this);
        // Probably some sort of fade
        //public Material grass;
        //public Material skybox;
        //public Vector3 cameraPosition;
        //public bool isUpsideDown;

        Grass.GetComponent<MeshRenderer>().material = config.grass;
        RenderSettings.skybox = config.skybox;
        Camera.main.transform.position = config.cameraPosition;
        if (config.isUpsideDown) UpsideDownWorld();
    }

    void UpsideDownWorld() {
        Camera.main.transform.Rotate(new Vector3(0, 0, -180));
    }
}
