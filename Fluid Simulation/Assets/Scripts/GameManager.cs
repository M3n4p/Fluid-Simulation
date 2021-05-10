using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public int ParticleResolution = 15;
    public float ParticleSize = 1.0f;
    public Material ParticleMaterial;

    void Awake()
    {
        if(manager != null)
        {
            GameObject.Destroy(manager);
        }
        else
        {
            manager = this;
        }
        DontDestroyOnLoad(this);
    }
}
