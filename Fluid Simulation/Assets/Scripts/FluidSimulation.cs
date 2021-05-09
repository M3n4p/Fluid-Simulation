using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulation : MonoBehaviour
{
    //public class Particle : MonoBehaviour
    //{
    //    public Particle(Vector3 position, float mass, Vector3 velocity, Material mat, float checkingRadius)
    //    {
    //        Position = position;
    //        Mass = mass;
    //        Velocity = velocity;
    //        NeighborCheckingRadius = checkingRadius;
    //        Neighbors = new List<GameObject>();

    //        GObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        GObject.name = "Particle";
    //        Rigidbody rb = GObject.AddComponent<Rigidbody>();
    //        rb.useGravity = false;
    //        rb.constraints = RigidbodyConstraints.FreezeRotation;
    //        GObject.GetComponent<MeshRenderer>().material = mat;
    //        CheckingCollider = GObject.AddComponent<SphereCollider>();
    //        CheckingCollider.radius = checkingRadius;
    //        CheckingCollider.isTrigger = true;
    //        GObject.transform.position = Position;
    //    }

    //    public void UpdateParticlePosition(Vector3 newPosition)
    //    {
    //        Position = newPosition;
    //        GObject.transform.position = newPosition;
    //    }

    //    private void OnTriggerStay(Collider other)
    //    {
    //        Neighbors.Add(other.gameObject);
    //    }

    //    private void OnTriggerExit(Collider other)
    //    {
    //        Neighbors.Remove(other.gameObject);
    //    }

    //    private SphereCollider CheckingCollider;

    //    public readonly List<GameObject> Neighbors;
    //    public GameObject GObject { get;  set; }
    //    public Vector3 Position { get; set; }
    //    public float Mass { get; set; }
    //    public Vector3 Velocity { get; set; }
    //    public float NeighborCheckingRadius { get; set; }
    //}

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
                    foreach (SphereCollider col in p.GetComponents(typeof(SphereCollider)))
                    {
                        if(col.isTrigger == true)
                        {
                            col.radius = neighborCheckingRadius;
                        }
                    }
                    particles[i] = p;
                }
            }
        }
    }

    private float DensityEstimation()
    {
        return 0;
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
