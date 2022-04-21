using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSelectionUI : MonoBehaviour
{
    private TMP_Text[] skillNames = null;
    private TMP_Text[] skillInfo = null;

    private Dictionary<string, int> allSkillLevel = new Dictionary<string, int>()
    {
        { "Flame Bead", 0 },
        { "Slash Blast", 0 },
        { "Magnetic Drag", 0 },
        { "Meteo", 0 },
        { "Ice Age", 0 },
    };

    private List<string> allSkillNames = new List<string>()
    {
        "Flame Bead",
        "Slash Blast",
        "Magnetic Drag",
        "Meteo",
        "Ice Age"
    };

    private List<string> allSkillInfo = new List<string>()
    {
        "Throw the fire bead to forward.",
        "Slash to forward.",
        "Attracts nearby enemies.",
        "Drop the Metro.",
        "Freeze around."
    };

    private void Awake()
    {
        Transform skillUIs = transform.Find("SkillUIs");
        int skillUICount = skillUIs.childCount;

        if (skillUICount > 0)
        {
            skillNames = new TMP_Text[skillUICount];
            skillInfo = new TMP_Text[skillUICount];

            for (int i = 0; i < skillUICount; ++i)
            {
                TMP_Text skillNameText = skillUIs.GetChild(i).GetChild(0).GetComponentInChildren<TMP_Text>();
                TMP_Text skillInfoText = skillUIs.GetChild(i).GetChild(1).GetComponentInChildren<TMP_Text>();

                skillNames[i] = skillNameText;
                skillInfo[i] = skillInfoText;
            }
        }

        transform.gameObject.SetActive(false);
    }

    public void UpdateSkillSelectionUI()
    {
        List<int> indexList = new List<int>();

        for (int i = 0; i < 3;)
        {
            int randomIndex = Random.Range(0, allSkillNames.Count);

            if (!indexList.Contains(randomIndex))
            {
                indexList.Add(randomIndex);

                skillNames[i].text = allSkillNames[randomIndex] + " (LV." + allSkillLevel[allSkillNames[randomIndex]] + ")";
                skillInfo[i].text = allSkillInfo[randomIndex];
                ++i;
            }
        }
    }

    public void OnClickSelectButton(int index)
    {
        // 선택된 스킬의 레벨을 증가시킨다.
        allSkillLevel[skillNames[index].text.Split(" (")[0]] += 1;
        transform.gameObject.SetActive(false);

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
