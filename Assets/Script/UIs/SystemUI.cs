using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Text mainGuideText;
    public Text subGuideText;

    private Animator animator;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        ShowGuide();
    }

    public void ShowGuide()
    {
        animator.Play("ShowGuide", -1, 0.0f);
    }

    public void ShowBloodEffect()
    {
        animator.Play("ShowBloodEffect", -1, 0.0f);
    }
}
