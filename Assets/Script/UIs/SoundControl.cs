using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        if (!PlayerPrefs.HasKey("Pandora_s Place") || !PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted",0);
            PlayerPrefs.SetFloat("Pandora_s Place", 1);
        //    Load();
        }
        else
        {
        //    Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = muted;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    //    Save();
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            muted = true;
            AudioListener.pause = true;
        }
        else
        {
            muted = false;
            AudioListener.pause = false;
        }
       // Save();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
    
    //옵션 저장용
    /*private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("Pandora_s Place");
    }
    
    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
        PlayerPrefs.SetFloat("Pandora_s Place", volumeSlider.value);
    }*/
}
