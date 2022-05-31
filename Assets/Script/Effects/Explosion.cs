using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int skillType;
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
        if (skillType == 1)
            yield return new WaitForSeconds(1.0f);
        else if(skillType == 2)
            yield return new WaitForSeconds(3.5f);
        else if(skillType == 3)
            yield return new WaitForSeconds(1.2f);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        explosionParicleSystem.Play();
    }
}
