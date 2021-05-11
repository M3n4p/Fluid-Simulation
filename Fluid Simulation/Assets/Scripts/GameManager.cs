using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public int NumberOfParticlesX = 10;
    public int NumberOfParticlesZ = 10;
    public int NumberOfParticlesY = 2;
    public Vector3 ParticleVelocity = Vector3.zero;
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
