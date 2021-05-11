using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulation
{
    public List<Particle> particles;

    public ComputeShader ComputeShader;

    public SmoothingKernel Poly6;
    public SmoothingKernel Spiky;
    public SmoothingKernel Viscosity;

    public Particle particle;

    public Vector3 FSForce;
    public Vector3 dist;
    public Vector3 FSVelocity;

    public float maxDist;
    public float maxDistSq;
    public float scalar;
    public float FSPressureSum;
    public float distLen;

    public FluidSimulation(ComputeShader computeShader)
    {
        particles = new List<Particle>();
        particle = new Particle();
        Poly6 = new Poly6(1.0f);
        Spiky = new Spiky(1.0f);
        Viscosity = new Viscosity(1.0f);

        ComputeShader = computeShader;

        maxDist = particle.Size * 0.5f;
        maxDistSq = maxDist * maxDist;
    }

    public void ConceptualParticle(Vector3 position)
    {
        particle = new Particle();
        particle.Position = position;
        particles.Add(particle);
    }

    public void CalculateDensities(int index)
    {
        particles[index].Density = 0.0f;
        particles[index].Density += particles[index].Mass * (float)Poly6.Calculate(ref dist);
    }


    public void CalculateFSForces(int indexPart, int indexOther)
    {
        if(particles[indexOther].Density > Mathf.Epsilon && particles[indexPart] != particles[indexOther])
        {
            dist = particles[indexPart].Position - particles[indexOther].Position;
            distLen = dist.sqrMagnitude;
            if (distLen < maxDist)
            {
                // Calculate pressure forces
                FSForce = CalculateFSPressure(particles[indexOther].Mass, particles[indexPart].Pressure, particles[indexOther].Pressure, particles[indexOther].Density, ref dist);
                particles[indexPart].Force -= FSForce;
                particles[indexOther].Force -= FSForce;

                // Calculate Viscosity forces
                FSForce = CalculateFSViscosity(particles[indexOther].Mass, particles[indexPart].Velocity, particles[indexOther].Velocity, particles[indexOther].Density, ref dist);
                particles[indexPart].Force += FSForce;
                particles[indexOther].Force += FSForce;
            }
        }
    }

    public Vector3 CalculateFSPressure(float mass, float iPressure, float jPressure, float density, ref Vector3 distance)
    {
        FSPressureSum = (iPressure + jPressure) / (2.0f * density);
        FSForce = Spiky.CalculateGradient(ref distance);
        scalar = mass * FSPressureSum;
        FSForce *= scalar;

        return FSForce;
    }

    public Vector3 CalculateFSViscosity(float mass, Vector3 iVelocity, Vector3 jVelocity, float density, ref Vector3 distance)
    {
        FSVelocity = (jVelocity - iVelocity) / density;
        FSVelocity = FSVelocity * mass * (float)Viscosity.CalculateLaplacian(ref distance);
        FSForce = Vector3.Scale(FSForce, FSVelocity);

        return FSForce;
    }
}
