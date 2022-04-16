using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public MonsterGenerator monsterGenerator = null;

    public bool isGameOver = false;

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
}