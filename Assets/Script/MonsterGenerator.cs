using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject monsterPrefab = null;

    // 오로지 생성된 자식을 담기 위해 사용한다.
    public Transform monsters = null;

    private int monsterCount = 0;
    private int maxMonsterCount = 50;

    private float genPeriod = 3.0f;

    public IEnumerator CreateMonster()
    {
        WaitForSeconds genTime = new WaitForSeconds(genPeriod);

        int genCountPerFrame = 0;

        while (!GameManager.Instance.isGameOver)
        {
            genCountPerFrame = Random.Range(5, 10);

            if (monsterCount < maxMonsterCount)
            {
                for (int i = 0; i < genCountPerFrame; ++i)
                {
                    Vector3 genPosition = new Vector3(Random.Range(-30.0f, 30.0f), Random.Range(4.0f, 6.0f), 230.0f);
                    Quaternion genRotation = new Quaternion(0.0f, 180.0f, 0.0f, 1.0f);

                    GameObject newMonster = Instantiate(monsterPrefab, genPosition, genRotation);
                    newMonster.transform.parent = monsters;

                    monsterCount += 1;

                    if (monsterCount >= maxMonsterCount)
                    {
                        break;
                    }
                }

                yield return genTime;
            }
            else
            {
                yield return null;
            }
        }
    }
}
