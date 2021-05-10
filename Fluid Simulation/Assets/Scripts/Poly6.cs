using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poly6 : SmoothingKernel
{
    public Poly6(double smoothingLength)
    {
        SmoothingLengthH = smoothingLength;
    }

    public override double Calculate(ref Vector3 distance)
    {
        Scaling = 315.0f / (64.0f * Mathf.PI * Mathf.Pow((float)SmoothingLength, 9.0f));
        lengthOfDistanceSq = (float)Mathf.Pow(distance.sqrMagnitude, 2.0f);

        if(lengthOfDistanceSq >= SmoothingLengthSq || lengthOfDistanceSq <= Mathf.Epsilon)
        {
            return 0.0f;
        }

        h2minusr2 = SmoothingLengthSq - lengthOfDistanceSq;

        return Scaling * (h2minusr2 * h2minusr2 * h2minusr2);
    }

    public override Vector3 CalculateGradient(ref Vector3 distance)
    {
        Scaling = 945.0f / (32.0f * Mathf.PI * Mathf.Pow((float)SmoothingLength, 9.0f));
        lengthOfDistanceSq = Mathf.Pow(distance.sqrMagnitude, 2.0f);

        if(lengthOfDistanceSq > SmoothingLengthSq || lengthOfDistanceSq <= Mathf.Epsilon)
        {
            return Vector3.zero;
        }

        h2minusr2 = SmoothingLengthSq - lengthOfDistanceSq;
        scalar = Scaling * (h2minusr2 * h2minusr2);

        return new Vector3(-distance.x * (float)scalar, -distance.y * (float)scalar, -distance.z * (float)scalar);
    }

    public override double CalculateLaplacian(ref Vector3 distance)
    {
        this.Scaling = 945.0f / (8.0f * Mathf.PI * (float)Mathf.Pow((float)SmoothingLength, 9.0f));
        lengthOfDistanceSq = Mathf.Pow(distance.sqrMagnitude, 2.0f);

        if(lengthOfDistanceSq > SmoothingLengthSq || lengthOfDistanceSq <= Mathf.Epsilon)
        {
            return 0.0f;
        }

        h2minusr2 = SmoothingLengthSq - lengthOfDistanceSq;

        distance = Vector3.Scale(distance, distance);
        return this.Scaling * h2minusr2 * (distance.sqrMagnitude - ((3.0f * h2minusr2) / 4.0f));
    }
}
