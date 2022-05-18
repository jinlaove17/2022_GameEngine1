using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionUI : MonoBehaviour
{
    public SkillDB skillDB = null;
                   
    public Text[]  skillUINames = null;
    public Text[]  skillUIInfo = null;

    private int[]  selectedIndices = new int[3];

    public void UpdateSkillSelectionUI()
    {
        List<int> indexList = new List<int>();

        for (int i = 0; i < 3;)
        {
            int randomIndex = Random.Range(0, skillDB.skillPrefabs.Length);

            if (!indexList.Contains(randomIndex))
            {
                indexList.Add(randomIndex);

                selectedIndices[i] = randomIndex;
                skillUINames[i].text = skillDB.skillPrefabs[randomIndex].skillName + " (LV." + skillDB.skillPrefabs[randomIndex].skillLevel + ")";
                skillUIInfo[i].text = skillDB.skillPrefabs[randomIndex].skillInfo;
                ++i;
            }
        }
    }

    public void OnClickSelectButton(int index)
    {
        // 선택된 스킬의 레벨을 증가시킨다.
        skillDB.skillPrefabs[selectedIndices[index]].skillLevel += 1;

        transform.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
