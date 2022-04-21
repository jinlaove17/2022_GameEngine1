using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupSystem : MonoBehaviour
{
    public GameObject popup;
    Animator anim;

    public static PopupSystem Instance { get; private set; }
    public Text txtTitle, txtContent;
    Action onClickOkay, onClickCancel;
    private void Awake()
    {
        Instance = this;
        popup.GetComponent<Animator>();
    }

    /*private void Update()
    {
        if (anim.GetCurrentAnimatorClipInfo(0).IsName("close"))
        {
            if (anim.GetCurrentAnimatorClipInfo(0).normalizedTime >= 1)
            {
                popup.SetActive(false);
            }
        }
    }*/

    public void OpenPopUp(
        string title,
        string content,
        Action onClickOkay,
        Action onClickCancel)
    {
        txtTitle.text = title;
        txtContent.text = content;
        this.onClickOkay = onClickOkay;
        this.onClickCancel = onClickCancel;
        popup.SetActive(true);
    }

    public void OnClickOkay()
    {
        if (onClickOkay != null)
            onClickOkay();
        ClosePopup();
    }
    public void OnClickCancel()
    {
        if (onClickCancel != null)
            onClickCancel();
        ClosePopup();
    }
    void ClosePopup()
    {
        anim.SetTrigger("close");
    }
}
