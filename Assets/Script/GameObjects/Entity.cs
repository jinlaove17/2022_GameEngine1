using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool isHit = false;
    private bool isFalling = false;

    private int health = 100;

    public bool IsAlive
    {
        get
        {
            return health > 0;
        }
    }

    public bool IsHit
    {
        get
        { 
            return isHit;
        }

        set
        { 
            isHit = value;
        }
    }

    public bool IsFalling
    {
        get
        {
            return isFalling;
        }

        set
        {
            isFalling = value;
        }
    }

    public int Health
    {
        get
        { 
            return health;
        }

        set
        { 
            health = value;

            if (health <= 0)
            {
                health = 0;
            }
        }
    }
}
