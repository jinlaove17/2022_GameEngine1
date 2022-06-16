using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOUND_TYPE
{
    BGM,
    SFX
}

[CreateAssetMenu(fileName = "Sound DB", menuName = "Create Sound DB", order = 2)]
public class SoundDB : ScriptableObject
{
    public SoundData[] soundBundles;
}

[Serializable]
public class SoundData
{
    public string soundName;
    public AudioClip soundClip;
}
