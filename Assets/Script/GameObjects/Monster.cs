using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    private Animator animator = null;
    private List<Material> materials = new List<Material>();

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        
        // 해당 객체가 가지고 있는 모든 메터리얼을 캐싱 해놓는다.
        // 이때, 이름이 중복된다면 하나만 저장하도록 한다.
        SkinnedMeshRenderer[] skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<string> isIncluded = new List<string>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (Material material in skinnedMeshRenderer.materials)
            {
                if (!isIncluded.Contains(material.name))
                {
                    materials.Add(material);
                    isIncluded.Add(material.name);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsAlive)
        {
            // collision.gameObject.tag로 하면 충돌이 일어난 프레임의 최상단을 가져오기 때문에 collider.tag로 해야함!
            if (collision.collider.tag == "Floor")
            {
                animator.SetTrigger("Land");
            }
            else if (collision.collider.tag == "Weapon")
            {
                if (!IsHit)
                {
                    Health -= 50;

                    StartCoroutine(HitEffect());

                    if (!IsAlive)
                    {
                        animator.SetTrigger("Die");
                        GameManager.Instance.IncreasePlayerExp(100.0f);
                    }
                }
            }
        }
    }

    IEnumerator HitEffect()
    {
        IsHit = true;

        foreach (Material material in materials)
        {
            material.color = new Color(1.0f, 0.6f, 0.6f, 1.0f);
        }

        yield return new WaitForSeconds(0.15f);

        IsHit = false;

        foreach (Material material in materials)
        {
            material.color = Color.white;
        }
    }
}
