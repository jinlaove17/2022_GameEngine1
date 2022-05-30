using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private Image skillIcon;
    private Image skillFilter;
    private Text skillCoolTimeText;
    private Image[] skillLevelImages;

    private bool isEmpty = true;
    private bool isUsable;

    private SKILL_TYPE skillType;
    private float skillCoolTime;
    private float currentCoolTime;

    private void Awake()
    {
        skillIcon = transform.GetComponent<Image>();
        skillFilter = transform.GetChild(0).GetComponent<Image>();
        skillCoolTimeText = transform.GetChild(0).GetComponentInChildren<Text>(true);
        skillLevelImages = transform.GetChild(3).GetComponentsInChildren<Image>();
    }

    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }
    }

    public bool IsUsable
    {
        get
        {
            return isUsable;
        }
    }

    public SKILL_TYPE SkillType
    {
        get
        {
            return skillType;
        }
    }

    public void RegisterSkill(SKILL_TYPE newSkillType)
    {
        if (isEmpty)
        {
            SkillPrefab skill = PoolingManager.Instance.skillDB.skillPrefabs[(int)newSkillType];

            isEmpty = false;
            isUsable = true;

            skillType = newSkillType;
            skillCoolTime = skill.skillCoolTime;
            skillIcon.sprite = skill.skillIcon;
        }
    }

    public void IncreaseSkillLevel(int skillLevel)
    {
        skillLevelImages[skillLevel].sprite = SkillManager.Instance.skillLevelImage;
    }

    public IEnumerator CalculateCoolTime()
    {
        isUsable = false;
        skillCoolTimeText.gameObject.SetActive(true);
        currentCoolTime = skillCoolTime;

        // 스킬 버튼을 가린다.
        skillFilter.fillAmount = 1.0f;

        while (skillFilter.fillAmount > 0.0f)
        {
            float deltaTime = Time.smoothDeltaTime / skillCoolTime;

            skillFilter.fillAmount -= deltaTime;
            currentCoolTime -= Time.smoothDeltaTime;
            skillCoolTimeText.text = string.Format("{0:0.0}", currentCoolTime);

            yield return null;
        }

        // 스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 변경한다.
        isUsable = true;
        currentCoolTime = 0.0f;
        skillCoolTimeText.gameObject.SetActive(false);
    }
}
