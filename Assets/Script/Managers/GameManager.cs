using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Player player;
    public MonsterGenerator monsterGenerator;
    public GameObject triggers;
    public SystemUI systemUI;

    private bool isGameOver = false;

    private double totalTime;
    public Text totalTimeText;

    private int stage;
    private int restMonsterCount;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject gameManager = new GameObject(nameof(GameManager));

                    instance = gameManager.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }

        set
        {
            isGameOver = value;
        }
    }

    public int Stage
    {
        get
        {
            return stage;
        }

        set
        {
            if (stage <= 2)
            {
                stage = value;
            }
        }
    }

    public int RestMonsterCount
    {
        get
        {
            return restMonsterCount;
        }

        set
        {
            restMonsterCount = value;

            if (restMonsterCount <= 0)
            {
                restMonsterCount = 0;

                if (stage <= 2)
                {
                    // 다음 스테이지로 가는 문을 여는 트리거를 활성화시킨다.
                    triggers.transform.GetChild(0).GetChild(2 * stage).gameObject.SetActive(true);
                    triggers.transform.GetChild(0).GetChild(2 * stage + 1).gameObject.SetActive(true);

                    // 이전 스테이지로 가는 문들을 닫는 트리거를 활성화시킨다.
                    triggers.transform.GetChild(1).GetChild(stage).gameObject.SetActive(true);

                    systemUI.mainGuideText.text = "현재 스테이지의 모든 몬스터를 제거했습니다!";
                    systemUI.subGuideText.text = "다음 스테이지로 이동하세요!";
                    systemUI.ShowGuide();
                }
            }
        }
    }

    private void Awake()
    {
        SetPlayer();
    }
    private void SetPlayer()
    {
        Characters ch = GameObject.Find("SelectContainer").transform.GetComponent<SelectContainer>().crr_character;

        GameObject.Find("Entity").transform.Find("Player").transform.Find(ch.ToString()).gameObject.SetActive(true);

        player = GameObject.FindWithTag("Player").transform.GetComponent<Player>();
    }

    private void Start()
    {
        if (monsterGenerator != null)
        {
            StartCoroutine(monsterGenerator.Spawn());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        totalTime += Time.deltaTime;
        totalTimeText.text = TimeSpan.FromSeconds(totalTime).ToString(@"mm\:ss");
    }

    public void IncreasePlayerExp(float expIncreament)
    {
        StartCoroutine(player.IncreaseExp(expIncreament));
    }

    public void PrepareNextStage()
    {
        Stage += 1;
        monsterGenerator.PrepareNextStage();
    }
}
