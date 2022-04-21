using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SOUND_TYPE { BGM, SFX }

    private static SoundManager instance = null;

    // 0: Bgm, 1: Sfx
    private AudioSource[] audioPlayer = new AudioSource[(int)(SOUND_TYPE.SFX + 1)];

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<SoundManager>();
            }

            return instance;
        }
    }

    public void PlayBGM(string clipName, float pitch = 1.0f)
    {
        if (clipName != null)
        {
            if (audioClips.ContainsKey(clipName))
            {
                if (audioPlayer[(int)(SOUND_TYPE.BGM)].isPlaying)
                {
                    audioPlayer[(int)(SOUND_TYPE.BGM)].Stop();
                }

                audioPlayer[(int)(SOUND_TYPE.BGM)].clip = audioClips[clipName];
                audioPlayer[(int)(SOUND_TYPE.BGM)].pitch = pitch;
                audioPlayer[(int)(SOUND_TYPE.BGM)].Play();
            }
        }
    }

    public void PlaySFX(string clipName, float pitch = 1.0f)
    {
        if (clipName != null)
        {
            if (audioClips.ContainsKey(clipName))
            {
                audioPlayer[(int)(SOUND_TYPE.SFX)].PlayOneShot(audioClips[clipName], pitch);
            }
        }
    }
}
