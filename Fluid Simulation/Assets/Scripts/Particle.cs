using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    private SphereCollider checkingCollider;

    public List<GameObject> neighbors;
    public float mass { get; set; }
    public Vector3 velocity { get; set; }
    public float neighborCheckingRadius { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        neighbors = new List<GameObject>();
        checkingCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!neighbors.Contains(other.gameObject) && other.gameObject.layer != LayerMask.NameToLayer("Walls"))
            neighbors.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        neighbors.Remove(other.gameObject);
    }
}
