using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    public Text[] skillUINames;
    public Image[] skillIcons;
    public Text[] skillUIInfo;
    public GameObject levelUpParticle;

    private Animator animator;
    private SKILL_TYPE[] selectedSkillTypes = new SKILL_TYPE[3];

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void ActivateUI()
    {
        transform.gameObject.SetActive(true);
        animator.Play("Show", -1, 0.0f);

        List<int> typeList = new List<int>();
        SkillDB skillDB = PoolingManager.Instance.skillDB;

        for (int i = 0; i < 3;)
        {
            int randomType = Random.Range(0, skillDB.skillPrefabs.Length);

            if (!typeList.Contains(randomType))
            {
                int skillLevel = SkillManager.Instance.GetSkillLevel((SKILL_TYPE)randomType);

                if (skillLevel < 5)
                {
                    SkillPrefab skill = skillDB.skillPrefabs[randomType];

                    selectedSkillTypes[i] = (SKILL_TYPE)randomType;
                    skillUINames[i].text = skill.skillName + " (LV." + skillLevel + ")";
                    skillIcons[i].sprite = skill.skillIcon;
                    skillUIInfo[i].text = skill.skillInfo;
                    ++i;

                    typeList.Add(randomType);
                }
            }
        }

        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnClickSelectButton(int index)
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (SkillManager.Instance.HasSkill(selectedSkillTypes[index]))
        {
            // 이미 등록된 스킬이라면 레벨을 증가시킨다.
            SkillManager.Instance.IncreaseSkillLevel(selectedSkillTypes[index]);
        }
        else
        {
            SkillManager.Instance.RegisterSkill(selectedSkillTypes[index]);
        }

        transform.gameObject.SetActive(false);
    }

    public void ShowLevelUpEffect()
    {
        Vector3 genPosition = transform.position;

        genPosition.y += 25.0f;

        Instantiate(levelUpParticle, genPosition, Quaternion.identity);
    }
}
