using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public enum SKILL_TYPE { ThrowFire, DeadExplode, Genesis, EnergyDischarge };

    private static SkillManager instance = null;

    public SkillSelectionUI skillSelectionUI = null;

    public Sprite levelImage = null;

    public Image[] skillFilters = null;
    public Text[] skillCoolTimeText = null;
    public Transform[] skillLevelGroups = null;

    private BaseSkill[] skillSlots = new BaseSkill[5];
    private int skillCount = 0;

    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<SkillManager>();

                if (instance == null)
                {
                    GameObject gameManager = new GameObject(nameof(SkillManager));

                    instance = gameManager.AddComponent<SkillManager>();
                }
            }

            return instance;
        }
    }

    public bool CheckSlotEmpty(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= skillSlots.Length)
        {
            print("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return false;
        }

        return skillSlots[slotIndex] == null;
    }

    public bool HasSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= PoolingManager.Instance.skillDB.skillPrefabs.Length)
        {
            Debug.LogError("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return false;
        }

        for (int i = 0; i < skillCount; ++i)
        {
            if (skillSlots[i].gameObject == PoolingManager.Instance.skillDB.skillPrefabs[skillIndex].prefab)
            {
                return true;
            }
        }

        return false;
    }

    public void InsertSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= PoolingManager.Instance.skillDB.skillPrefabs.Length || skillCount >= skillSlots.Length)
        {
            print("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return;
        }

        BaseSkill skill = PoolingManager.Instance.skillDB.skillPrefabs[skillIndex].prefab.GetComponent<BaseSkill>();

        skillSlots[skillCount] = skill;
        skillLevelGroups[skillCount].GetChild(skillSlots[skillCount].skillLevel).GetComponent<Image>().sprite = levelImage;

        skill.skillLevel += 1;
        skillCount += 1;
    }

    public void IncreaseSkillLevel(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= PoolingManager.Instance.skillDB.skillPrefabs.Length)
        {
            Debug.LogError("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return;
        }

        BaseSkill skill = PoolingManager.Instance.skillDB.skillPrefabs[skillIndex].prefab.GetComponent<BaseSkill>();

        if (skill.skillLevel < 5)
        {
            skill.skillLevel += 1;
            skillLevelGroups[skillCount].GetChild(skillSlots[skillCount].skillLevel).GetComponent<Image>().sprite = levelImage;
        }
    }

    public void UseSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > skillCount)
        {
            print("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return;
        }

        print("SkillManager::UseSkill()");

        if (skillSlots[slotIndex] != null && skillSlots[slotIndex].isUsable)
        {
            // ½ºÅ³ ¹öÆ°À» °¡¸°´Ù.
            skillFilters[slotIndex].fillAmount = 1.0f;

            skillSlots[slotIndex].currentTime = skillSlots[slotIndex].skillCoolTime;
            skillSlots[slotIndex].isUsable = false;
            skillSlots[slotIndex].UseSkill();

            StartCoroutine(CalculateCoolTime(slotIndex));
        }
    }

    public void GenerateEffect(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skillSlots.Length)
        {
            print("ÀÎµ¦½º¸¦ ¹þ¾î³µ½À´Ï´Ù.");

            return;
        }

        switch ((SKILL_TYPE)skillIndex)
        {
            case SKILL_TYPE.ThrowFire:
                Vector3 genPosition = GameManager.Instance.player.rightHand.transform.position;

                GameObject fire = PoolingManager.Instance.GetSkillEffect("ThrowFire", genPosition, Quaternion.identity);

                fire.GetComponent<Rigidbody>().AddForce(15.0f * GameManager.Instance.player.transform.forward, ForceMode.Impulse);
                break;
            case SKILL_TYPE.DeadExplode:
                Transform playerTransform = GameManager.Instance.player.transform;
                Vector3 skillPos = playerTransform.position;

                skillPos += 15.0f * playerTransform.forward;

                PoolingManager.Instance.GetSkillEffect("DeadExplode", skillPos, Quaternion.identity);
                break;
            case SKILL_TYPE.Genesis:
                StartCoroutine(GenerateBeam());
                break;
            case SKILL_TYPE.EnergyDischarge:
                StartCoroutine(Discharge());
                break;
        }
    }

    private IEnumerator GenerateBeam()
    {
        WaitForSeconds spawnTime = new WaitForSeconds(0.3f);
        Transform playerTransform = GameManager.Instance.player.transform;

        for (int i = 0; i < 5; ++i)
        {
            Vector3 skillPos = playerTransform.position;

            skillPos += Random.Range(5.0f, 10.0f) * playerTransform.forward;
            skillPos += Random.Range(-10.0f, 10.0f) * playerTransform.right;

            PoolingManager.Instance.GetSkillEffect("Genesis", skillPos, Quaternion.identity);

            yield return spawnTime;
        }
    }

    private IEnumerator Discharge()
    {
        Transform playerTransform = GameManager.Instance.player.transform;
        Vector3 skillPos = playerTransform.position;

        skillPos.y += 1.1f;

        GameObject skill = PoolingManager.Instance.GetSkillEffect("EnergyDischarge", skillPos, Quaternion.identity);
       
        yield return new WaitForSeconds(2.0f);

        skill.GetComponent<SphereCollider>().enabled = true;
    }

    private IEnumerator CalculateCoolTime(int slotIndex)
    {
        skillCoolTimeText[slotIndex].gameObject.SetActive(true);

        while (skillFilters[slotIndex].fillAmount > 0.0f)
        {
            float deltaTime = Time.smoothDeltaTime / skillSlots[slotIndex].skillCoolTime;

            skillFilters[slotIndex].fillAmount -= deltaTime;
            skillSlots[slotIndex].currentTime -= Time.smoothDeltaTime;
            skillCoolTimeText[slotIndex].text = string.Format("{0:0.0}", skillSlots[slotIndex].currentTime);

            yield return null;
        }

        // ½ºÅ³ ÄðÅ¸ÀÓÀÌ ³¡³ª¸é ½ºÅ³À» »ç¿ëÇÒ ¼ö ÀÖ´Â »óÅÂ·Î ¹Ù²Þ
        skillSlots[slotIndex].isUsable = true;
        skillSlots[slotIndex].currentTime = 0.0f;
        skillCoolTimeText[slotIndex].gameObject.SetActive(false);
    }
}
