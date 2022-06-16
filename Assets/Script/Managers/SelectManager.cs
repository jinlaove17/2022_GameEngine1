using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Characters
{
    Jody,
    Lewis,
    Megan,
    Remy
}

public class SelectManager : MonoBehaviour
{
    public GameObject disk;
    public GameObject target;
    public static Characters crr_character;
    public Button selectButton;
    public Button nextButton;
    public Button previousButton;
    public float smoothRotate = 2.0f;
    public TMPro.TMP_Text tmp_text;
    private float rot = -45.0f;
    public SelectContainer selectContainer;

    void Start()
    {
        crr_character = Characters.Remy;
        selectButton.onClick.AddListener(OnSelect);
        previousButton.onClick.AddListener(OnPrevious);
        nextButton.onClick.AddListener(OnNext);
    }

    void Update()
    {
        tmp_text.text = crr_character.ToString();
    }

    private void OnSelect()
    {
        print("I'm SelectButton");
        SceneManager.LoadScene("GameScene");
    }
    private void OnPrevious()
    {
        if (crr_character == Characters.Jody)
            crr_character = Characters.Remy;
        else
            crr_character--;

        selectContainer.Set_Character(crr_character);

        rot = rot - 90f;
        target.transform.rotation = Quaternion.Euler(0, rot, 0);
    }
    private void OnNext()
    {
        if (crr_character == Characters.Remy)
            crr_character = Characters.Jody;
        else
            crr_character++;

        selectContainer.Set_Character(crr_character);


        rot = rot + 90f;
        target.transform.localRotation = Quaternion.Euler(0, rot, 0);
    }
    private void LateUpdate()
    {
        float currTAngle = Mathf.LerpAngle(disk.transform.eulerAngles.y, target.transform.eulerAngles.y,
            smoothRotate * Time.deltaTime);

        disk.transform.localRotation = Quaternion.Euler(0, currTAngle, 0);
    }
}
