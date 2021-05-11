using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulationSystem : MonoBehaviour
{
    public List<GameObject> drawParticleList = new List<GameObject>();

    private GameObject drawParticle;
    private FluidSimulation fs;
    private Particle tempParticle;

    private static float UpdateTime = 0.05f;
    private int particleNumberX, particleNumberY, particleNumberZ;
    private Vector3 particleVelocity;

    public FluidSimulationSystem()
    {
        tempParticle = new Particle();
        tempParticle.Velocity = particleVelocity;
        fs = new FluidSimulation();
    }

    // Start is called before the first frame update
    void Start()
    {
        tempParticle.Size = GameManager.manager.ParticleSize;
        particleNumberX = GameManager.manager.NumberOfParticlesX;
        particleNumberY = GameManager.manager.NumberOfParticlesY;
        particleNumberZ = GameManager.manager.NumberOfParticlesZ;
        particleVelocity = GameManager.manager.ParticleVelocity;
        CreateParticle();
        StartCoroutine("CalculateFS");
    }

    private void CreateParticle()
    {
        for(int i = 0; i < particleNumberX; i++)
        {
            for(int j = 0; j < particleNumberY; j++)
            {
                for (int k = 0; k < particleNumberZ; k++)
                {
                    drawParticle = VisualParticle(i, j, k);
                    drawParticleList.Add(drawParticle);
                    fs.ConceptualParticle(drawParticle.transform.position);
                }
            }
        }
    }

    private GameObject VisualParticle(int loopIndex1, int loopIndex2, int loopIndex3)
    {
        float particleNumberXHalf = particleNumberX / 2.0f;
        float particleNumberYHalf = particleNumberY / 2.0f;
        float particleNumberZHalf = particleNumberZ / 2.0f;
        drawParticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = drawParticle.AddComponent<Rigidbody>();
        drawParticle.transform.position = new Vector3(loopIndex1 - particleNumberXHalf + transform.position.x, loopIndex2 - particleNumberYHalf + transform.position.y, loopIndex3 - particleNumberZHalf + transform.position.z);
        drawParticle.transform.localScale = new Vector3(tempParticle.Size, tempParticle.Size, tempParticle.Size);
        rb.mass = tempParticle.Mass;
        rb.velocity = tempParticle.Velocity;
        //rb.useGravity = false;
        drawParticle.GetComponent<MeshRenderer>().material = GameManager.manager.ParticleMaterial;
        drawParticle.name = "Particle";
        drawParticle.tag = "fluid";

        return drawParticle; 
    }

    public void Calculate()
    {
        for (int i = 0; i < fs.particles.Count; i++)
        {
            fs.particles[i].Position = drawParticleList[i].transform.position;
            fs.particles[i].Update(UpdateTime);

            for (int j = 0; j < fs.particles.Count; j++)
            {
                fs.particles[i].UpdatePressure();
                fs.CalculateDensities(i);

                fs.particles[j].Position = drawParticleList[j].transform.position;
                fs.distLen = Vector3.Distance(fs.particles[i].Position, fs.particles[j].Position);

                if (fs.distLen <= fs.particles[i].checkSize)
                {
                    fs.CalculateFSForces(i, j);
                    drawParticleList[j].transform.position = fs.particles[j].Position;
                }
            }
            drawParticleList[i].transform.position = fs.particles[i].Position;
        }
    }

    IEnumerator CalculateFS()
    {
        for(; ; )
        {
            Calculate();
            yield return new WaitForSeconds(UpdateTime);
        }
    }
}
