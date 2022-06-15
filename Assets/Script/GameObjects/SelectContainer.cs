using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectContainer : MonoBehaviour
{
    public Characters crr_character;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        crr_character = Characters.Remy;
    }

    public void Set_Character(Characters ch)
    {
        crr_character = ch;
    }
}
