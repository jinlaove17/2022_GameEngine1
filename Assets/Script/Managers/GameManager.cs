using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public Player player = null;
    public MonsterGenerator monsterGenerator = null;
    public SkillSelectionUI skillSelectionUI = null;

    public bool isGameOver = false;
    public int stage = 0;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    private void Start()
    {
        if (monsterGenerator != null)
        {
            StartCoroutine(monsterGenerator.CreateMonster());
        }
    }

    public void IncreasePlayerExp(float expIncreament)
    {
        StartCoroutine(player.IncreaseExp(expIncreament));
    }
}
