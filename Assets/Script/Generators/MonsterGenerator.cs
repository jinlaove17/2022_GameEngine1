using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] monsterPrefabs = null;

    // 오로지 생성된 자식을 담기 위해 사용한다.
    public Transform    monsters = null;

    // 몬스터의 생성 위치는 현재 스테이지의 가장자리이다.
    // x: XMin, y: XMax, z: ZMin, w: ZMax
    public Vector4[]    genLocation = null;

    private int         monsterCount = 0;
    private const int   maxMonsterCount = 30;

    private const float genPeriod = 3.0f;

    public IEnumerator CreateMonster()
    {
        WaitForSeconds genTime = new WaitForSeconds(genPeriod);

        while (!GameManager.Instance.IsGameOver)
        {
            if (GameManager.Instance.Stage > 0)
            {
                if (monsterCount < maxMonsterCount)
                {
                    int genCountPerFrame = Random.Range(5, 10 + 1);

                    for (int i = 0; i < genCountPerFrame; ++i)
                    {
                        int locationIndex = Random.Range(0, 3 + 1);
                        float genX = Random.Range(genLocation[locationIndex].x, genLocation[locationIndex].y);
                        float genY = Random.Range(4.0f, 6.0f);
                        float genZ = Random.Range(genLocation[locationIndex].z, genLocation[locationIndex].w);

                        Vector3 genPosition = new Vector3(genX, genY, genZ + 108.0f * (GameManager.Instance.Stage - 1));
                        Quaternion genRotation = Quaternion.identity;

                        switch (locationIndex)
                        {
                            case 0:
                                genRotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
                                break;
                            case 1:
                                genRotation = Quaternion.Euler(new Vector3(0.0f, -90.0f, 0.0f));
                                break;
                            case 3:
                                genRotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
                                break;
                        }

                        int monsterType = Random.Range(0, 1 + 1);

                        GameObject newMonster = Instantiate(monsterPrefabs[monsterType], genPosition, genRotation);
                        newMonster.transform.parent = monsters;

                        monsterCount += 1;
                        GameManager.Instance.RestMonsterCount += 1;

                        if (monsterCount >= maxMonsterCount)
                        {
                            break;
                        }
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

    public void PrepareNextStage()
    {
        monsterCount = 0;
    }
}
