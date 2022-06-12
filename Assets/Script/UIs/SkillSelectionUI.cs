using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    public Text[] skillUINames;
    public Image[] skillIcons;
    public Sprite blankedSkillIcon;
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

        SkillDB skillDB = SkillManager.Instance.skillDB;

        // 스킬 스롯이 모두 꽉찬 경우에는, 보유한 스킬만 선택되야 한다.
        if (SkillManager.Instance.IsSkillSlotFull())
        {
            int selectedIndex = 0;

            for (int i = 1; i < 4; ++i)
            {
                SKILL_TYPE skillTypeInSlot = SkillManager.Instance.GetSkillTypeInSlot(i);

                if (skillTypeInSlot == SKILL_TYPE.None)
                {
                    break;
                }

                int skillLevel = SkillManager.Instance.GetSkillLevel(skillTypeInSlot);

                if (0 < skillLevel && skillLevel < 5)
                {
                    SkillData skill = skillDB.skillBundles[(int)skillTypeInSlot];

                    selectedSkillTypes[selectedIndex] = skillTypeInSlot;
                    skillUINames[selectedIndex].text = skill.skillName + " (LV." + skillLevel + ")";
                    skillIcons[selectedIndex].sprite = skill.skillIcon;
                    skillUIInfo[selectedIndex].text = skill.skillInfo;

                    selectedIndex += 1;
                }
            }

            if (selectedIndex < 3)
            {
                for (int i = selectedIndex; i < 3; ++i)
                {
                    skillUINames[selectedIndex].text = "";
                    skillIcons[selectedIndex].sprite = blankedSkillIcon;
                    skillUIInfo[selectedIndex].text = "";
                }
            }
        }
        else
        {
            // 선택될 가능성이 있는 스킬을 저장하는 리스트
            List<SKILL_TYPE> candidateSkillTypeList = new List<SKILL_TYPE>();
            int allSkillCount = SkillManager.Instance.skillDB.skillBundles.Length;

            for (int i = 0; i < allSkillCount; ++i)
            {
                // 스킬레벨이 5미만이라면 후보 리스트에 저장한다.
                if (SkillManager.Instance.GetSkillLevel((SKILL_TYPE)i) < 5)
                {
                    candidateSkillTypeList.Add((SKILL_TYPE)i);
                }
            }

            // 3개의 선택창 중 후보 리스트의 크기만큼만 스킬 정보를 불러온다.
            int candidateSkillCount = candidateSkillTypeList.Count;
            List<int> selectedIndexList = new List<int>();

            for (int i = 0; i < 3;)
            {
                int randomIndex = Random.Range(1, candidateSkillCount);

                if (i < candidateSkillCount && !selectedIndexList.Contains(randomIndex))
                {
                    SkillData skill = skillDB.skillBundles[(int)candidateSkillTypeList[randomIndex]];
                    int skillLevel = SkillManager.Instance.GetSkillLevel(candidateSkillTypeList[randomIndex]);

                    selectedSkillTypes[i] = candidateSkillTypeList[randomIndex];
                    skillUINames[i].text = skill.skillName + " (LV." + skillLevel + ")";
                    skillIcons[i].sprite = skill.skillIcon;

                    float currentDamage = skillLevel * skill.skillDamage;
                    float upgradedDamage = (skillLevel + 1) * skill.skillDamage;
                    float damageInreament = upgradedDamage - currentDamage;
                    
                    skillUIInfo[i].text = skill.skillInfo.Replace("O", upgradedDamage + "(+" + damageInreament + ")");

                    selectedIndexList.Add(randomIndex);
                    i += 1;
                }
                else if (i >= candidateSkillCount)
                {
                    skillUINames[i].text = "";
                    skillIcons[i].sprite = blankedSkillIcon;
                    skillUIInfo[i].text = "";

                    i += 1;
                }
            }
        }
        
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
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

        genPosition.y += 20.0f;

        Instantiate(levelUpParticle, genPosition, Quaternion.identity);
    }
}
