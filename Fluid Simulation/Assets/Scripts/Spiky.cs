using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiky : SmoothingKernel
{
    public Spiky(double smoothingLength)
    {
        SmoothingLengthH = smoothingLength;
    }

    public override double Calculate(ref Vector3 distance)
    {
        Scaling = 15.0f / (Mathf.PI * Mathf.Pow((float)SmoothingLength, 6.0f));
        lengthOfDistance = distance.sqrMagnitude;

        if(lengthOfDistance > SmoothingLengthSq || lengthOfDistance < 0)
        {
            return 0.0f;
        }

        scalar = SmoothingLength - lengthOfDistance;
        return Scaling * (scalar * scalar * scalar);
    }

    public override Vector3 CalculateGradient(ref Vector3 distance)
    {
        lengthOfDistance = distance.sqrMagnitude;
        Scaling = 45.0f / (Mathf.PI * Mathf.Pow((float)SmoothingLength, 6.0f) * lengthOfDistance);

        if(lengthOfDistance > SmoothingLengthSq || lengthOfDistance < 0)
        {
            return Vector3.zero;
        }

        h2minusr2 = SmoothingLengthSq - lengthOfDistance;
        scalar = Scaling * (h2minusr2 * h2minusr2);

        return new Vector3(-distance.x * (float)scalar, -distance.y * (float)scalar, -distance.z * (float)scalar);
    }

    // not required
    public override double CalculateLaplacian(ref Vector3 distance)
    {
        throw new System.NotImplementedException();
    }
}
