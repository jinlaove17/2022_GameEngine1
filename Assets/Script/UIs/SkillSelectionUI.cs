using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    public Text[] skillUINames = null;
    public Text[] skillUIInfo = null;

    private int[] selectedIndices = new int[3];

    public void UpdateSkillSelectionUI()
    {
        transform.gameObject.SetActive(true);

        List<int> indexList = new List<int>();
        SkillDB skillDB = PoolingManager.Instance.skillDB;

        for (int i = 0; i < 3;)
        {
            int randomIndex = Random.Range(0, skillDB.skillPrefabs.Length);
            BaseSkill skill = skillDB.skillPrefabs[randomIndex].prefab.GetComponent<BaseSkill>();

            if (!indexList.Contains(randomIndex))
            {
                indexList.Add(randomIndex);

                selectedIndices[i] = randomIndex;
                skillUINames[i].text = skillDB.skillPrefabs[randomIndex].prefabName + " (LV." + skill.skillLevel + ")";
                skillUIInfo[i].text = skill.skillInfo;
                ++i;
            }
        }
    }

    public void OnClickSelectButton(int index)
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (SkillManager.Instance.HasSkill(selectedIndices[index]))
        {
            // 이미 등록된 스킬이라면 레벨을 증가시킨다.
            SkillManager.Instance.IncreaseSkillLevel(selectedIndices[index]);
        }
        else
        {
            SkillManager.Instance.InsertSkill(selectedIndices[index]);
        }

        transform.gameObject.SetActive(false);
    }
}
