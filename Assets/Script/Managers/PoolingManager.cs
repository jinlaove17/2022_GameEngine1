using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance = null;

    public MonsterDB monsterDB = null;

    // 생성된 자식들을 담기 위한 부모객체이다.      
    public Transform monsters = null;

    // 인스턴싱할 프리팝을 저장하기 위한 딕셔너리이다.
    private Dictionary<string, GameObject> prefabDict;

    // 풀링으로 관리되고 있는 객체들을 저장하기 위한 딕셔너리이다.
    private Dictionary<string, List<GameObject>> managedObjects;

    public static PoolingManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<PoolingManager>();

                if (instance == null)
                {
                    GameObject poolingManager = new GameObject(nameof(PoolingManager));

                    instance = poolingManager.AddComponent<PoolingManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        prefabDict = new Dictionary<string, GameObject>();
        managedObjects = new Dictionary<string, List<GameObject>>();

        // 데이터베이스를 딕셔너리로 재구성한다.
        foreach (MonsterData monsterData in monsterDB.monsterBundles)
        {
            prefabDict.Add(monsterData.monsterName, monsterData.prefab);
        }

        foreach (SkillData skillData in SkillManager.Instance.skillDB.skillBundles)
        {
            prefabDict.Add(skillData.skillName, skillData.prefab);
        }
    }

    public GameObject GetMonster(string objectName, Vector3 position, Quaternion quaternion)
    {
        if (!prefabDict.ContainsKey(objectName))
        {
            Debug.Log(objectName + " 프리팝이 존재하지 않습니다.");

            return null;
        }

        if (!managedObjects.ContainsKey(objectName))
        {
            managedObjects.Add(objectName, new List<GameObject>());
        }

        if (managedObjects[objectName].Any(obj => !obj.activeInHierarchy))
        {
            GameObject possibleObject = managedObjects[objectName].FirstOrDefault(obj => !obj.activeInHierarchy);
            
            possibleObject.SetActive(true);
            possibleObject.GetComponent<Monster>().Health = 100;
            possibleObject.transform.position = position;
            possibleObject.transform.rotation = quaternion;

            return possibleObject;
        }

        GameObject newObject = Instantiate(prefabDict[objectName], position, quaternion, monsters);

        managedObjects[objectName].Add(newObject);

        return newObject;
    }

    public GameObject GetSkillEffect(string objectName, Vector3 position, Quaternion quaternion)
    {
        if (!prefabDict.ContainsKey(objectName))
        {
            Debug.Log(objectName + " 프리팝이 존재하지 않습니다.");

            return null;
        }

        if (!managedObjects.ContainsKey(objectName))
        {
            managedObjects.Add(objectName, new List<GameObject>());
        }

        if (managedObjects[objectName].Any(obj => !obj.activeInHierarchy))
        {
            GameObject possibleObject = managedObjects[objectName].FirstOrDefault(obj => !obj.activeInHierarchy);

            possibleObject.SetActive(true);
            possibleObject.transform.position = position;
            possibleObject.transform.rotation = quaternion;

            // 보유한 파티클 시스템을 재시작 하도록 설정해준다.
            foreach (ParticleSystem particleSystem in possibleObject.GetComponentsInChildren<ParticleSystem>())
            {
                particleSystem.Simulate(0.0f, true, true);
                particleSystem.Play();
            }

            return possibleObject;
        }

        GameObject newObject = Instantiate(prefabDict[objectName], position, quaternion);

        managedObjects[objectName].Add(newObject);

        return newObject;
    }
}
