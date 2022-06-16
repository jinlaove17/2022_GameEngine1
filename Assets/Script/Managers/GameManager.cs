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

    public Text totalscoreText; //점수를 표시하는 Text객체를 에디터에서 받아옵니다.
    public Text timescoreText; //점수를 표시하는 Text객체를 에디터에서 받아옵니다.
    public Text dealscoreText; //점수를 표시하는 Text객체를 에디터에서 받아옵니다.
    public Text hitscoreText; //점수를 표시하는 Text객체를 에디터에서 받아옵니다.

    private double totalscore= 0; //점수를 관리합니다.
    private double timescore = 1000;
    private double dealscore = 0;
    private double hitscore = 0;
    
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
                    // ���� ���������� ���� ���� ���� Ʈ���Ÿ� Ȱ��ȭ��Ų��.
                    triggers.transform.GetChild(0).GetChild(2 * stage).gameObject.SetActive(true);
                    triggers.transform.GetChild(0).GetChild(2 * stage + 1).gameObject.SetActive(true);

                    // ���� ���������� ���� ������ �ݴ� Ʈ���Ÿ� Ȱ��ȭ��Ų��.
                    triggers.transform.GetChild(1).GetChild(stage).gameObject.SetActive(true);

                    systemUI.mainGuideText.text = "���� ���������� ��� ���͸� �����߽��ϴ�!";
                    systemUI.subGuideText.text = "���� ���������� �̵��ϼ���!";
                    systemUI.ShowGuide();
                }
            }
        }
    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform.GetComponent<Player>();
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
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
        GameManager.instance.TimeScore(1);

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
    
    public void TimeScore(int num) //점수를 추가해주는 함수를 만들어 줍니다.
    {
        timescore += num * -0.01;
        totalscore += num;
        timescoreText.text = "" + Math.Ceiling(timescore); //텍스트에 반영합니다.
    }
    public void DealScore(int num) //점수를 추가해주는 함수를 만들어 줍니다.
    {
        dealscore += num; //점수를 더해줍니다.
        totalscore += num;
        dealscoreText.text = "" + Math.Ceiling(dealscore); //텍스트에 반영합니다.
    }
    public void HitScore(int num) //점수를 추가해주는 함수를 만들어 줍니다.
    {
        hitscore += num; //점수를 더해줍니다.
        totalscore += num;
        hitscoreText.text = "Score : " + Math.Ceiling(hitscore); //텍스트에 반영합니다.
    }
    public void TotalScore() //점수를 추가해주는 함수를 만들어 줍니다.
    {
        totalscoreText.text = "Score : " + Math.Ceiling(totalscore); //텍스트에 반영합니다.
    }
}
