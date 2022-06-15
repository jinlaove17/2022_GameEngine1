using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public SoundDB soundDB;

    private AudioSource[] audioPlayers;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject soundManager = new GameObject(nameof(SoundManager));

                    instance = soundManager.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        audioPlayers = transform.GetComponents<AudioSource>();

        // 데이터베이스를 딕셔너리로 재구성한다.
        foreach (SoundData soundData in soundDB.soundBundles)
        {
            audioClips.Add(soundData.soundName, soundData.soundClip);
        }
    }

    public void PlayBGM(string clipName, float pitch = 1.0f)
    {
        if (clipName != null)
        {
            if (audioClips.ContainsKey(clipName))
            {
                if (audioPlayers[(int)(SOUND_TYPE.BGM)].isPlaying)
                {
                    audioPlayers[(int)(SOUND_TYPE.BGM)].Stop();
                }

                audioPlayers[(int)(SOUND_TYPE.BGM)].clip = audioClips[clipName];
                audioPlayers[(int)(SOUND_TYPE.BGM)].pitch = pitch;
                audioPlayers[(int)(SOUND_TYPE.BGM)].Play();
            }
        }
    }

    public void PlaySFX(string clipName, float pitch = 1.0f)
    {
        if (clipName != null)
        {
            if (audioClips.ContainsKey(clipName))
            {
                audioPlayers[(int)(SOUND_TYPE.SFX)].PlayOneShot(audioClips[clipName], pitch);
            }
        }
    }
}
