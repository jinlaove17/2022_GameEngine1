using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    public ParticleSystem explosionParicleSystem;
    private MeshRenderer meteorMesh;
    
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        meteorMesh = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            meteorMesh.enabled = false;
            explosionParicleSystem.Play();
        }
        
    }
}
