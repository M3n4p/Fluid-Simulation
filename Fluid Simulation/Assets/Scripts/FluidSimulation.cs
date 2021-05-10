using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulation : MonoBehaviour
{
    public int numberOfParticles = 100;
    public float particleXDistance = 1;
    public float particleZDistance = 1;
    public float particleYDistance = 1;
    public int verticalLayers = 2;
    public int horizontalLayers = 5;
    public float particleMass = 2;
    public float neighborCheckingRadius = 2;

    public GameObject particle;

    public Material particleMaterial;

    private GameObject[] particles;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = this.transform.position;

        int lineLength = (numberOfParticles / verticalLayers) / horizontalLayers;

        particles = new GameObject[numberOfParticles];
        for(int i = 0; i < verticalLayers; i++)
        {
            for (int j = 0; j < horizontalLayers; j++)
            {
                for (int k = 0; k < lineLength; k++)
                {
                    Vector3 particlePosition = new Vector3(position.x + k * particleXDistance, position.y - i * particleYDistance, position.z + j * particleZDistance);
                    GameObject p = Instantiate(particle, particlePosition, Quaternion.identity);
                    p.transform.parent = this.gameObject.transform;
                    p.GetComponent<Particle>().neighborCheckingRadius = neighborCheckingRadius;
                    particles[i] = p;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(particles[0].GetComponent<Particle>().neighbors.Count);
        }
    }
}
