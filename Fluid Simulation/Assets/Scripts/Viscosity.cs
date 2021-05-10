using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viscosity : SmoothingKernel
{
    public Viscosity(double kernelSize)
    {
        SmoothingLengthH = kernelSize;
    }

    public override double Calculate (ref Vector3 distance)
    {
        Scaling = 15.0f / (2.0f * Mathf.PI * SmoothingLengthCb);

        lengthOfDistance = distance.sqrMagnitude;
        lengthOfDistanceSq = Mathf.Pow((float)lengthOfDistance, 2.0f);
        lengthOfDistanceCb = Mathf.Pow((float)lengthOfDistance, 3.0f);

        if(lengthOfDistance > SmoothingLength || lengthOfDistance < Mathf.Epsilon)
        {
            return 0.0d;
        }

        scalar = -(lengthOfDistanceCb / (2 * SmoothingLengthCb)) + (lengthOfDistanceSq / SmoothingLengthSq) + (SmoothingLength / 2.0f * lengthOfDistance) - 1;
        return (Scaling * scalar);
    }

    public override Vector3 CalculateGradient(ref Vector3 distance)
    {
        Scaling = 15.0f / 2.0f * Mathf.PI * SmoothingLengthCb;

        lengthOfDistance = distance.sqrMagnitude;
        lengthOfDistanceSq = Mathf.Pow(distance.sqrMagnitude, 2.0f);
        lengthOfDistanceCb = Mathf.Pow(distance.sqrMagnitude, 3.0f);

        if(lengthOfDistance > SmoothingLength || lengthOfDistance <= Mathf.Epsilon)
        {
            return Vector3.zero;
        }

        scalar = -((3 * lengthOfDistance) / (2.0f * SmoothingLengthCb)) + (2.0f / SmoothingLengthSq) - (SmoothingLength / (2.0f * lengthOfDistanceCb));
        Scaling *= scalar;

        return new Vector3(distance.x * (float)Scaling, distance.y * (float)Scaling, distance.z * (float)Scaling);
    }

    public override double CalculateLaplacian(ref Vector3 distance)
    {
        Scaling = 15.0f / Mathf.Pow((float)SmoothingLength, 5.0f);
        lengthOfDistance = distance.sqrMagnitude;

        if(lengthOfDistance > SmoothingLength || lengthOfDistance < Mathf.Epsilon)
        {
            return 0.0f;
        }

        scalar = 1 - (lengthOfDistance / SmoothingLength);
        return Scaling * scalar;
    }
}
