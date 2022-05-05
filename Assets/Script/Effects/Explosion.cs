using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem explosionParicleSystem;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.0f);

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        explosionParicleSystem.Play();
    }
}
