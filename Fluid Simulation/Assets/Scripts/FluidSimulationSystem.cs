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
    private float particleNumber;

    public FluidSimulationSystem()
    {
        tempParticle = new Particle();
        tempParticle.Velocity = new Vector3(10.0f, 0.0f, 0.0f);
        fs = new FluidSimulation();
    }

    // Start is called before the first frame update
    void Start()
    {
        tempParticle.Size = GameManager.manager.ParticleSize;
        particleNumber = GameManager.manager.ParticleResolution;
        CreateParticle();
        StartCoroutine("CalculateFS");
    }

    private void CreateParticle()
    {
        for(int i = 0; i < particleNumber; i++)
        {
            for(int j = 0; j < particleNumber; j++)
            {
                drawParticle = VisualParticle(i, j);
                drawParticleList.Add(drawParticle);
                fs.ConceptualParticle(drawParticle.transform.position);
            }
        }
    }

    private GameObject VisualParticle(int loopIndex1, int loopIndex2)
    {
        drawParticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        drawParticle.AddComponent<Rigidbody>();
        drawParticle.transform.position = new Vector3(loopIndex1 + (transform.position.x - tempParticle.Size * 2.0f), transform.position.y, loopIndex2 + (transform.position.z - tempParticle.Size * 2.0f));
        drawParticle.transform.localScale = new Vector3(tempParticle.Size, tempParticle.Size, tempParticle.Size);
        drawParticle.GetComponent<Rigidbody>().mass = tempParticle.Mass;
        drawParticle.GetComponent<Rigidbody>().velocity = tempParticle.Velocity;
        drawParticle.GetComponent<MeshRenderer>().material = GameManager.manager.ParticleMaterial;
        drawParticle.name = "Particle";
        drawParticle.tag = "fluid";

        return drawParticle; 
    }

    public void Calculate()
    {
        particleNumber = GameManager.manager.ParticleResolution;
        for(int i = 0; i < fs.particles.Count; i++)
        {
            fs.particles[i].Position = drawParticleList[i].transform.position;
            fs.particles[i].Update(UpdateTime);

            for(int j = 0; j < fs.particles.Count; j++)
            {
                fs.particles[i].UpdatePressure();
                fs.CalculateDensities(i);

                fs.particles[j].Position = drawParticleList[j].transform.position;
                fs.distLen = Vector3.Distance(fs.particles[i].Position, fs.particles[j].Position);

                if(fs.distLen <= fs.particles[i].Size)
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
