using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmoothingKernel
{
    protected double Scaling;
    protected double scalar;

    protected double SmoothingLength;
    protected double SmoothingLengthSq;
    protected double SmoothingLengthCb;

    protected double lengthOfDistance;
    protected double lengthOfDistanceSq;
    protected double lengthOfDistanceCb;

    protected double h2minusr2;

    public double SmoothingLengthH
    {
        get
        {
            return SmoothingLength;
        }

        set
        {
            SmoothingLength = value;
            SmoothingLengthSq = SmoothingLength * SmoothingLength;
            SmoothingLengthSq = SmoothingLength * SmoothingLength * SmoothingLength;
        }
    }

    public SmoothingKernel()
    {
        SmoothingLength = 1.0d;
    }

    public SmoothingKernel(float smoothingLength)
    {
        SmoothingLength = smoothingLength;
    }

    public abstract double Calculate(ref Vector3 distance);

    public abstract Vector3 CalculateGradient(ref Vector3 distance);

    public abstract double CalculateLaplacian(ref Vector3 distance);
}
