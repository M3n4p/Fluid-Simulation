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

    private void Update()
    {
        List<GameObject> objects = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, neighborCheckingRadius, Physics.DefaultRaycastLayers);
        foreach (Collider col in hitColliders)
        {
            if(!col.gameObject.Equals(this.gameObject))
            {
                objects.Add(col.gameObject);
            }
        }

        neighbors = objects;
    }
}
