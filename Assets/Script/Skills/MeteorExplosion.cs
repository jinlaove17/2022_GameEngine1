using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    public ParticleSystem explosionParicleSystem;
    public MeshRenderer meteorMesh;
    
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
            explosionParicleSystem.Play();

            Player player = GameManager.Instance.player;

            if (player.IsAlive && !player.IsHit)
            {
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 2.0f)
                {
                    // 공격 애니메이션이 끝날 때쯤 플레이어에게 피해를 주어야 하기 때문에, 애니메이션 이벤트를 활용한다.
                    StartCoroutine(player.DecreaseHealth(10));

                    // 화면 전체에 블러드 이펙트 애니메이션을 활성화한다.
                    GameManager.Instance.systemUI.ShowBloodEffect();

                    SoundManager.Instance.PlaySFX("Hit");
                }
            }
        }
    }
}
