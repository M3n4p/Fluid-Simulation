using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    public float Mass;
    public float Size;
    public float checkSize;
    public Vector3 Position;
    public Vector3 PreviousPosition;
    public Vector3 Velocity;
    public Vector3 Force;
    public float Density;
    public float Pressure;
    public float Viscosity;
    public float GasConstant;
    public float DensityOffset;

    public Particle()
    {
        Mass = 2.99f * Mathf.Pow(10f, -23f);
        Size = 1.0f;
        checkSize = 2.0f;
        Viscosity = 1.0f;
        Position = Vector3.zero;
        PreviousPosition = Position;
        Velocity = Vector3.zero;
        Force = Vector3.zero;
        Density = 1000f;
        GasConstant = 8.3145f;
        DensityOffset = 100.0f;
    }

    public void UpdatePressure()
    {
        Pressure = GasConstant * (Density - DensityOffset);
    }

    public void Update(float dTime)
    {
        Integrate(ref Position, ref PreviousPosition, ref Velocity, Force, Mass, dTime);
    }

    public void Integrate(ref Vector3 position, ref Vector3 previousPosition, ref Vector3 velocity, Vector3 force, float mass, float timeStep)
    {
        previousPosition = position;
        Vector3 acceleration = force / mass;
        velocity = velocity + acceleration * timeStep;
        position = position + velocity * timeStep;
    }
}
