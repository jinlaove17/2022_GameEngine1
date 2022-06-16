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

            Player player = GameManager.Instance.player;

            if (player.IsAlive && !player.IsHit)
            {
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 2.0f)
                {
                    // ���� �ִϸ��̼��� ���� ���� �÷��̾�� ���ظ� �־�� �ϱ� ������, �ִϸ��̼� �̺�Ʈ�� Ȱ���Ѵ�.
                    StartCoroutine(player.DecreaseHealth(10));

                    // ȭ�� ��ü�� ���� ����Ʈ �ִϸ��̼��� Ȱ��ȭ�Ѵ�.
                    GameManager.Instance.systemUI.ShowBloodEffect();
                }
            }
        }
        
    }
}
