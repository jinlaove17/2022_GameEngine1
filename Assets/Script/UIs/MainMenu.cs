using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM("TitleScene");
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void OnClickHelpButton()
    {
        Debug.Log("도움말");
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
#else
        Debug.Log("Exit Game");
#endif
    }

    public void MouseOver()
    {
        SoundManager.Instance.PlaySFX("Button", 0.5f);
    }
}
